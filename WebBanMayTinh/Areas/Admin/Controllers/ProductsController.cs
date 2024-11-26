using Antlr.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using WebBanMayTinh.Models;
using WebBanMayTinh.Models.ViewModel;
using PagedList.Mvc;
using PagedList;

namespace WebBanMayTinh.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();

        // GET: Admin/Products
        [HttpGet]
        public ActionResult Index(string searchTerm,decimal? maxprice , decimal? minprice, int? page,string sortOrder)
        {
            var products_Linq = db.Products.AsQueryable();


            var ProductSearchVM = new ProductSearchVM();



            if (!string.IsNullOrEmpty(searchTerm))
            {
                ProductSearchVM.SearchTerm= searchTerm;
                products_Linq = products_Linq.Where(p => p.ProductName.Contains(searchTerm) || p.Category.CategoryName.Contains(searchTerm));
            }


            if (minprice.HasValue)
            {
                ProductSearchVM.Min_Price = minprice.Value;
                products_Linq = products_Linq.Where(p => p.ProductPrice >= minprice.Value);
            }



            if (maxprice.HasValue)
            {
                ProductSearchVM.Max_Price = maxprice.Value;
                products_Linq = products_Linq.Where(p => p.ProductPrice <= maxprice.Value);
            }



            ////Nó sẽ bỏ vào list để hiển thị theo 1 danh sách 
            //ProductSearchVM.Products = products_Linq.ToList();




            //OrderBy:Sort
            switch (sortOrder)
            {
                //Theo tên giảm dần
                case "NameDesc":
                    products_Linq = products_Linq.OrderByDescending(p => p.ProductName); break;


                //Theo Tên tăng dần
                case "NameAsc":
                    products_Linq = products_Linq.OrderBy(p => p.ProductName); break;



                //Theo giá tăng giảm dần
                case "PriceDesc":
                    products_Linq = products_Linq.OrderByDescending(p => p.ProductPrice); break;


                //Theo giá tăng tăng dần
                case "PriceASC":
                    products_Linq = products_Linq.OrderBy(p => p.ProductPrice); break;


                //Mặc định là xếp theo tên tăng dần
                default:
                    products_Linq = products_Linq.OrderBy(p => p.ProductName); break; 
            }
            ProductSearchVM.SortOrder = sortOrder;


            //Hiển thị theo danh sách đã phân trang
            int pageNumber = page ?? 1; //Mặc định là trang 1 nếu ko có truyền vào số trang tương ứng

            int pagesize = 2; //Hiển tối đa được bao nhiêu sản phẩm trên 1 trang

            ProductSearchVM.Products = products_Linq.ToPagedList(pageNumber,pagesize); //truyền list sản phẩm của product đã truy vấn ra để phân trang 

            return View(ProductSearchVM);
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,ProductName,ProductStatus,ProductPrice,ProductImage,DiscountPercentage,DiscountValue,CEO")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,ProductName,ProductStatus,ProductPrice,ProductImage,DiscountPercentage,DiscountValue")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);


            if (product == null)
            {
                return HttpNotFound();
            }


            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
