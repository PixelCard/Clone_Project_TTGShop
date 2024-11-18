using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMayTinh.Models;
using PagedList.Mvc;
using WebBanMayTinh.Models.ViewModel;

namespace WebBanMayTinh.Controllers
{
    public class ProductDetailsController : Controller
    {
        private Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();
        // GET: ProductDetails
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductPage(int? id, int? quantity, int? page)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Product productWithID = db.Products.Find(id);


            if (productWithID == null)
            {
                return HttpNotFound();
            }


            ////Lấy ra tất cả sản phẩm có cùng danh mục
            //var products_WithSameCategory = db.Products.Where(p => p.ProductID == productWithID.ProductID && p.CategoryID == productWithID.CategoryID).AsQueryable();


            ProductDetailsVM productDetails = new ProductDetailsVM();



            ProductDescription productDescription_withID = db.ProductDescriptions.FirstOrDefault(p=> p.ProductID == id);



            //var productsDescription_WithSameCategory = db.ProductDescriptions.Where(p => p.ProductID == productDescription_withID.ProductID).AsQueryable();


            

                //Sử dụng toán tử 3 ngôi để thay đổi giá trị hiển thị thành số vd : 17.00 sẽ hiển thị thành 17 ( làm tròn xuống)

                var ValuePersentageChanged = productWithID.DiscountPercentage.HasValue ? Math.Floor(productWithID.DiscountPercentage.Value * 100).ToString("0.##"): "0";






            productDetails.products = productWithID;



            productDetails.productdescription = productDescription_withID;




            productDetails.Text_DiscountValue = ValuePersentageChanged;



            if (quantity.HasValue)
            {
                productDetails.quantity = quantity.Value;
            }



            return View(productDetails);
        }
    }
}