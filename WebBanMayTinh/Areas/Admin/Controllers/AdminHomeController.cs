using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebBanMayTinh.Models;
using WebBanMayTinh.Models.ViewModel;

namespace WebBanMayTinh.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}