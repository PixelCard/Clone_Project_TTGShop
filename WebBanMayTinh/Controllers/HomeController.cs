using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanMayTinh.Controllers
{
    public class HomeController : Controller
    {
        private Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();

        public ActionResult HomePage()
        {
            // Query to fetch products with a discount of 10% or more
            var productsOnSale = db.Products
                .Where(p => p.DiscountValue < p.ProductPrice &&
                            ((p.ProductPrice - p.DiscountValue) / p.ProductPrice) >= 0.1m)
                .ToList();

            return View(productsOnSale);
        }


        public ActionResult TopProduct()
        {
            var topProducts = db.Products
                .Select(p => new
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    ProductPrice = p.ProductPrice,
                    ProductImage = p.ProductImage,
                    TotalSales = p.OrderDetails.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.TotalSales)
                .Take(5)
                .ToList();
            return View(topProducts);
        }
  
    }
}