using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanMayTinh.Models;

namespace WebBanMayTinh.Areas.Admin.Controllers
{
    public class ProductDecriptionsController : Controller
    {
        private Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();

        // GET: Admin/ProductDecriptions
        public ActionResult Index()
        {
            var productDecriptions = db.ProductDecriptions.Include(p => p.Product);
            return View(productDecriptions.ToList());
        }


        // GET: Admin/ProductDecriptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            ProductDecription productDecription = db.ProductDecriptions.Find(id);
            if (productDecription == null)
            {
                return HttpNotFound();
            }
            return View(productDecription);
        }

        // GET: Admin/ProductDecriptions/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }


        // POST: Admin/ProductDecriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductDescriptionID,ProductID,CPU,MAINBOARD,RAM,HARDDISK,VGA")] ProductDecription productDecription)
        {
            if (ModelState.IsValid)
            {
                db.ProductDecriptions.Add(productDecription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productDecription.ProductID);
            return View(productDecription);
        }

        // GET: Admin/ProductDecriptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDecription productDecription = db.ProductDecriptions.Find(id);
            if (productDecription == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productDecription.ProductID);
            return View(productDecription);
        }

        // POST: Admin/ProductDecriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductDescriptionID,ProductID,CPU,MAINBOARD,RAM,HARDDISK,VGA")] ProductDecription productDecription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productDecription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productDecription.ProductID);
            return View(productDecription);
        }

        // GET: Admin/ProductDecriptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDecription productDecription = db.ProductDecriptions.Find(id);
            if (productDecription == null)
            {
                return HttpNotFound();
            }
            return View(productDecription);
        }

        // POST: Admin/ProductDecriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductDecription productDecription = db.ProductDecriptions.Find(id);
            db.ProductDecriptions.Remove(productDecription);
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
