using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanMayTinh.Models;
using WebBanMayTinh.Models.ViewModel;
using WebBanMayTinh.Shared.Constants;


namespace WebBanMayTinh.Areas.Admin.Controllers
{
    public class AdminAccountController : Controller
    {
        private readonly Web_Ban_May_TinhEntities dbContext = new Web_Ban_May_TinhEntities();

        public ActionResult Index(string searchString = null, int? page = null, string userRole = null)
        {
            if (searchString != null)
            {
                page = 1;
            }

            ViewBag.CurrentFilter = searchString;

            // Load list users theo role cua nguoi dung hien tai
            // Admin - Load all
            // Staff - Load customers
            // Manager - Load customers + staffs
            string role = Session[SessionDataKeys.UserRole].ToString();


            IQueryable<Customer> customers = dbContext.Customers.Include("User");

            if (role == UserRoleConstants.Staff)
            {
                customers = customers.Where(c => c.User.UserRole == UserRoleConstants.Customer);
            }
            else if (role == UserRoleConstants.Manager)
            {
                customers = customers
                    .Where(c => c.User.UserRole == UserRoleConstants.Customer || c.User.UserRole == UserRoleConstants.Staff);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.CustomerName.Contains(searchString) || c.CustomerPhone.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(userRole))
            {
                customers = customers.Where(c => c.User.UserRole == userRole);
            }
            ViewBag.Role = userRole;


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

            List<SelectListItem> availableRoles = new List<SelectListItem>();

            if (currentRole == UserRoleConstants.Admin)
            {
                availableRoles = new List<SelectListItem> {
                    new SelectListItem()
                    {
                        Text = "Nhân viên",
                        Value = UserRoleConstants.Staff,
                    },
                    new SelectListItem()
                    {
                        Text = "Khách hàng",
                        Value = UserRoleConstants.Customer,
                    },
                    new SelectListItem()
                    {
                        Text = "Quản lý",
                        Value = UserRoleConstants.Manager,
                    },
                    new SelectListItem()
                    {
                        Text = "Admin",
                        Value = UserRoleConstants.Admin,
                    }
                };
            }

            ViewBag.AvailableRoles = availableRoles; // Gửi danh sách quyền đến View

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = dbContext.Users.SingleOrDefault(u => u.Email == model.Email);

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
                dbContext.Users.Add(user);
                dbContext.SaveChanges();

                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    Email_User = user.Email,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    CustomerBirthDay = model.CustomerBirthDay,
                    CustomerGender = genderselection,
                };

                dbContext.Customers.Add(customer);
                dbContext.SaveChanges();
                return RedirectToAction("Login", "Auth");
            }

            List<SelectListItem> availableRoles = new List<SelectListItem>();

            string currentRole = Session[SessionDataKeys.UserRole]?.ToString();

            if (currentRole == UserRoleConstants.Admin)
            {
                availableRoles = new List<SelectListItem> {
                    new SelectListItem()
                    {
                        Text = "Nhân viên",
                        Value = UserRoleConstants.Staff,
                    },
                    new SelectListItem()
                    {
                        Text = "Khách hàng",
                        Value = UserRoleConstants.Customer,
                    },
                    new SelectListItem()
                    {
                        Text = "Quản lý",
                        Value = UserRoleConstants.Manager,
                    },
                    new SelectListItem()
                    {
                        Text = "Admin",
                        Value = UserRoleConstants.Admin,
                    }
                };
            }

            ViewBag.AvailableRoles = availableRoles; // Gửi danh sách quyền đến View

            return View(model);
        }
        [HttpGet]  // Hoặc không ghi nếu mặc định là [HttpGet]
        public ActionResult Edit(string email)
        {
            var customer = dbContext.Customers.SingleOrDefault(c => c.Email_User == email);
            if (customer == null)
            {
                return HttpNotFound("Không tìm thấy thông tin khách hàng.");
            }

            return View(customer);  // Trả về view Edit với dữ liệu khách hàng
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer model)
        {
            if (ModelState.IsValid)
            {
                var user = dbContext.Users.SingleOrDefault(u => u.Email == model.Email_User);
                var customer = dbContext.Customers.SingleOrDefault(c => c.Email_User == model.Email_User);

                if (user == null || customer == null)
                {
                    return HttpNotFound("Không tìm thấy thông tin tài khoản.");
                }

                // Cập nhật thông tin khách hàng
                customer.CustomerName = model.CustomerName;
                customer.CustomerPhone = model.CustomerPhone;
                customer.CustomerAddress = model.CustomerAddress;
                customer.CustomerBirthDay = model.CustomerBirthDay;
                customer.CustomerGender = model.CustomerGender;

                dbContext.SaveChanges();

                return RedirectToAction("Index", "AdminAccount");
            }

            return View(model);  // Nếu model không hợp lệ, trả lại view
        }



        // GET: Admin/Products/Delete/5
        public ActionResult Delete(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm người dùng và khách hàng dựa trên email
            var user = dbContext.Users.SingleOrDefault(u => u.Email == email);
            var customer = dbContext.Customers.SingleOrDefault(c => c.Email_User == email);

            if (user == null || customer == null)
            {
                return HttpNotFound();
            }

            // Trả về view xác nhận xóa tài khoản
            return View(customer);
        }


        // POST: Admin/Account/Delete/{email}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string email)
        {
            // Tìm người dùng và khách hàng dựa trên email
            var user = dbContext.Users.SingleOrDefault(u => u.Email == email);
            var customer = dbContext.Customers.SingleOrDefault(c => c.Email_User == email);

            if (user == null || customer == null)
            {
                return HttpNotFound();
            }

            // Xóa người dùng và khách hàng khỏi cơ sở dữ liệu
            dbContext.Users.Remove(user);
            dbContext.Customers.Remove(customer);
            dbContext.SaveChanges();

            // Sau khi xóa thành công, chuyển hướng đến trang danh sách tài khoản
            return RedirectToAction("Index", "AdminAccount");
        }


    }



}
