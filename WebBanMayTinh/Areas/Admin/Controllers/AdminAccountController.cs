using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMayTinh.Models;
using WebBanMayTinh.Shared.Constants;

namespace WebBanMayTinh.Areas.Admin.Controllers
{
    public class AdminAccountController : Controller
    {
        private readonly  Web_Ban_May_TinhEntities dbContext = new Web_Ban_May_TinhEntities();
        private string currentFilter = "";
        public ActionResult Index(string searchString = null, int? page = null)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // Load list users theo role cua nguoi dung hien tai
            // Admin - Load all
            // Staff - Load customers
            // Manager - Load customers + staffs
            string role = Session[SessionDataKeys.UserRole].ToString();


            IQueryable<Customer> customers = dbContext.Customers
                .Include("User");

            if (role == UserRoleConstants.Staff)
            {
                customers = customers.Where(c => c.User.UserRole == UserRoleConstants.Customer);
            }
            else if  (role == UserRoleConstants.Manager)
            {
                customers = customers
                    .Where(c => c.User.UserRole == UserRoleConstants.Customer || c.User.UserRole == UserRoleConstants.Staff);
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            
            var data = customers
                .OrderBy(c => c.CustomerID)
                .ToPagedList(pageNumber, pageSize);
            
            return View(data);
        }
    }
}