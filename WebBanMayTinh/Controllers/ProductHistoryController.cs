using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanMayTinh.Models;
using PagedList.Mvc;
using PagedList;

namespace WebBanMayTinh.Controllers
{
    public class ProductHistoryController : Controller
    {
        // GET: ProductHistrory
        private Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();

        public ActionResult ProductHistory(int? page)
        {

            var item = db.Orders.OrderByDescending(x => x.OrderDate).ToList();


            if (page == null)
            {
                page = 1;
            }
            var pageSize = 10;
            var pageNumber = page ?? 1;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            return View(item.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Index(int? page)
        {

            var item = db.Orders.OrderByDescending(x => x.OrderDate).ToList();


            if (page == null)
            {
                page = 1;
            }
            var pageSize = 10;
            var pageNumber = page ?? 1;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            return View(item.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}