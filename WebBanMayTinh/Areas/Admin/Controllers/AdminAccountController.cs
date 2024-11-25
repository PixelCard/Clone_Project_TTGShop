using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMayTinh.Models;
using WebBanMayTinh.Models.ViewModel;
using WebBanMayTinh.Shared.Constants;

namespace WebBanMayTinh.Areas.Admin.Controllers
{
    public class AdminAccountController : Controller
    {
        private readonly  Web_Ban_May_TinhEntities dbContext = new Web_Ban_May_TinhEntities();
        private string currentFilter = "";
        private readonly Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();
        public ActionResult Index(string searchString = null, int? page = null)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // Load list users theo role cua nguoi dung hien tai
            // Admin - Load all
            // Staff - Load customers
            // Manager - Load customers + staffs
            string role = Session[SessionDataKeys.UserRole].ToString();


            IQueryable<Customer> customers = dbContext.Customers
                .Include("User");

            if (role == UserRoleConstants.Staff)
            {
                customers = customers.Where(c => c.User.UserRole == UserRoleConstants.Customer);
            }
            else if  (role == UserRoleConstants.Manager)
            {
                customers = customers
                    .Where(c => c.User.UserRole == UserRoleConstants.Customer || c.User.UserRole == UserRoleConstants.Staff);
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            
            var data = customers
                .OrderBy(c => c.CustomerID)
                .ToPagedList(pageNumber, pageSize);
            
            return View(data);
        }
        public ActionResult Create()
        {
            string currentRole = Session[SessionDataKeys.UserRole]?.ToString();

            // Lấy danh sách quyền có thể chọn khi đăng ký
            List<string> availableRoles = new List<string>();

            if (currentRole == UserRoleConstants.Admin)
            {
                availableRoles = new List<string> { UserRoleConstants.Admin, UserRoleConstants.Manager, UserRoleConstants.Staff, UserRoleConstants.Customer };
            }
            else if (currentRole == UserRoleConstants.Manager)
            {
                availableRoles = new List<string> { UserRoleConstants.Staff, UserRoleConstants.Customer };
            }
            else if (currentRole == UserRoleConstants.Staff)
            {
                availableRoles = new List<string> { UserRoleConstants.Customer };
            }

            ViewBag.AvailableRoles = new SelectList(availableRoles); // Gửi danh sách quyền đến View

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.SingleOrDefault(u => u.Email == model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email_User", "Email đã tồn tại!");
                    return View(model);
                }

                // Lấy quyền đăng ký từ model
                string role = model.UserRole;  // UserRole sẽ được chọn từ form đăng ký

                if (string.IsNullOrEmpty(role))
                {
                    ModelState.AddModelError("Role", "Vui lòng chọn quyền.");
                    return View(model);
                }

                string genderselection = model.CustomerGender;
                string hashedPassword = HashingHelper.HashPassword(model.Password);

                var user = new User
                {
                    Email = model.Email,
                    Password = hashedPassword,
                    UserRole = role
                };
                db.Users.Add(user);
                db.SaveChanges();

                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    Email_User = user.Email,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    CustomerBirthDay = model.CustomerBirthDay,
                    CustomerGender = genderselection,
                };

                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Login", "Auth");
            }

            return View(model);
        }

    }
}