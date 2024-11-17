using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMayTinh.Models;
using WebBanMayTinh.Models.ViewModel;
using PagedList.Mvc;
using PagedList;

namespace WebBanMayTinh.Controllers
{
    public class SearchController : Controller
    {
        private Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        

        public ActionResult SearchPage(string searchTerm,int? page)
        {
            var products = db.Products.AsQueryable();

            var Search_VM = new SearchVM();


            if(!string.IsNullOrEmpty(searchTerm))
            {
                Search_VM.searchTerm = searchTerm;

                products = products.Where(p => p.ProductName.Contains(searchTerm) ||  p.Category.CategoryName.Contains(searchTerm));
            }


            int pageNumber = page ?? 1;
            int pageSize = 5;


            Search_VM.Products_SearchPageList = products.OrderBy(p => p.ProductName.ToString()).ToPagedList(pageNumber, pageSize);

            Search_VM.page = pageNumber;

            Search_VM.pagenumber = pageSize;

            return View(Search_VM);
        }

        [HttpPost]
        public ActionResult SearchPage(string searchTerm)
        {
            var products = db.Products.AsQueryable();

            var Search_VM = new SearchVM();


            if (!string.IsNullOrEmpty(searchTerm))
            {
                Search_VM.searchTerm = searchTerm;

                products = products.Where(p => p.ProductName.Contains(searchTerm) || p.Category.CategoryName.Contains(searchTerm));
            }

            return RedirectToAction("SearchPage");
        }
    }
}