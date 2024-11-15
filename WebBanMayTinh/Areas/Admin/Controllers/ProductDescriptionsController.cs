using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanMayTinh.Models;
using WebBanMayTinh.Models.ViewModel;


namespace WebBanMayTinh.Areas.Admin.Controllers
{
    public class ProductDescriptionsController : Controller
    {
        private Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();

        // GET: Admin/ProductDescriptions
        public ActionResult Index(string searchTerm)
        {
            var ProductDespritionSearchVM = new ProductDetailsSearchVM();


            var productDescriptions = db.ProductDescriptions.AsQueryable();


            if (!string.IsNullOrEmpty(searchTerm))
            {
                //Tìm kiếm theo tên
                ProductDespritionSearchVM.SearchTerm = searchTerm;

                productDescriptions = productDescriptions.Where(pD => pD.Product.ProductName.Contains(searchTerm)); 
            }



            ProductDespritionSearchVM.ProductDescriptions = productDescriptions.ToList();


            return View(ProductDespritionSearchVM);
        }



        // GET: Admin/ProductDescriptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDescription productDescription = db.ProductDescriptions.Find(id);

            if (productDescription == null)
            {
                return HttpNotFound();
            }
            return View(productDescription);
        }

        // GET: Admin/ProductDescriptions/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }

        // POST: Admin/ProductDescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductDescriptionID,ProductID,CPU,MAINBOARD,RAM,SSD")] ProductDescription productDescription)
        {
            if (ModelState.IsValid)
            {
                db.ProductDescriptions.Add(productDescription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productDescription.ProductID);
            return View(productDescription);
        }

        // GET: Admin/ProductDescriptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDescription productDescription = db.ProductDescriptions.Find(id);
            if (productDescription == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productDescription.ProductID);
            return View(productDescription);
        }

        // POST: Admin/ProductDescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductDescriptionID,ProductID,CPU,MAINBOARD,RAM,SSD")] ProductDescription productDescription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productDescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productDescription.ProductID);
            return View(productDescription);
        }

        // GET: Admin/ProductDescriptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDescription productDescription = db.ProductDescriptions.Find(id);
            if (productDescription == null)
            {
                return HttpNotFound();
            }
            return View(productDescription);
        }

        // POST: Admin/ProductDescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductDescription productDescription = db.ProductDescriptions.Find(id);
            db.ProductDescriptions.Remove(productDescription);
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
