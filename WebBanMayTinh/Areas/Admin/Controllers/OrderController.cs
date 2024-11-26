using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using System.Net;
using WebBanMayTinh.Models;

namespace WebBanMayTinh.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        // GET: Admin/Order
        private Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();
        public ActionResult Index(int? page)
        {
            var item = db.Orders.OrderByDescending(x => x.OrderID).ToList();
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

        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}
        //// POST: Admin/Categories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Order order = db.Orders.Find(id);
        //    db.Orders.Remove(order);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}


        [HttpPost]
        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Attach(item);
                if (trangthai == 0)
                {
                    item.PaymentStatus = "Chưa thanh toán";
                }
                if (trangthai == 1)
                {
                    item.PaymentStatus = "Đang vận chuyển";
                }
                if (trangthai == 2)
                {
                    item.PaymentStatus = "Hoàn thành";
                }
                db.Entry(item).Property(x => x.PaymentStatus).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "False", Success = false });
        }
    }
}
