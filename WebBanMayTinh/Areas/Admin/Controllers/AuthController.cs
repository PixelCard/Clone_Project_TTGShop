using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebBanMayTinh.Models.ViewModel;
using WebBanMayTinh.Shared.Constants;

namespace WebBanMayTinh.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        private readonly Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();
        // GET: Admin/Auth
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
                && (u.UserRole == UserRoleConstants.Admin || u.UserRole == UserRoleConstants.Staff || u.UserRole == UserRoleConstants.Manager));

                if (user != null)
                {
                    Session[SessionDataKeys.Email] = user.Email;
                    Session[SessionDataKeys.UserRole] = user.UserRole;
                    FormsAuthentication.SetAuthCookie(user.Email, true);

                    return RedirectToAction("Index", "AdminHome");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");

                }
            }

            return View(model);
        }


        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}