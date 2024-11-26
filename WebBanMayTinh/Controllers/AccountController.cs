using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Remoting.Messaging;
using WebBanMayTinh.Models.ViewModel;
using WebBanMayTinh.Models;
using System.Web.Security;
using WebBanMayTinh.Shared.Constants;
using System.Security.Cryptography;
using System.Text;

namespace WebBanMayTinh.Controllers
{
    public class AccountController : Controller
    {
        private Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = HashingHelper.HashPassword(model.Password);
                var user = db.Users.SingleOrDefault(u => u.Email == model.Email
                && u.Password == hashedPassword
                );

                if (user != null)
                {
                    Session[SessionDataKeys.Email] = user.Email;
                    Session[SessionDataKeys.UserRole] = user.UserRole;

                    FormsAuthentication.SetAuthCookie(user.Email, true);

                    return RedirectToAction("HomePage", "Home");
                }


                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.SingleOrDefault(u => u.Email == model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email_User", "Email đã tồn tại!");
                    return View(model);
                }

                string genderselection = model.CustomerGender;
                if (genderselection == "Nam")
                {
                    genderselection = "Nam";
                }
                else if (genderselection == "Nữ")
                {
                    genderselection = "Nữ";
                }

                string hashedPassword = HashingHelper.HashPassword(model.Password);

                var user = new User
                {
                    Email = model.Email,
                    Password = hashedPassword,
                    UserRole = UserRoleConstants.Customer
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
                return RedirectToAction("HomePage", "Home");
            }

            return View(model);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("HomePage", "Home");
        }


        public ActionResult AccountInfo()
        {
            // Lấy email từ Session
            string email = Session[SessionDataKeys.Email]?.ToString();

            // Lấy thông tin user từ bảng User và Customer
            var user = db.Users.SingleOrDefault(u => u.Email == email);
            var customer = db.Customers.SingleOrDefault(c => c.Email_User == email);

            if (user == null || customer == null)
            {
                return HttpNotFound("Không tìm thấy thông tin tài khoản.");
            }

            // Tạo ViewModel với dữ liệu
            var model = new RegisterVM
            {
                Email = user.Email,
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
                CustomerAddress = customer.CustomerAddress,
                CustomerBirthDay = customer.CustomerBirthDay,
                CustomerGender = customer.CustomerGender
            };

            return View(model);
        }
    }
}



