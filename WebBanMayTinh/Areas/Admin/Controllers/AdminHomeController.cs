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
        private Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            var productsQuery = db.Products.AsQueryable();

            Statistic statistic = new Statistic();

            ProductDetailsVM productDetailsVM = new ProductDetailsVM();

            statistic.quantity = productDetailsVM.quantity;




            statistic.Products = productsQuery.ToList();



            return View(statistic);
        }
    }
}