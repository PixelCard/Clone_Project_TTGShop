using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMayTinh.Models.ViewModel
{
    public class HomePageVM
    {
        public List<Product> TopDiscountProducts { get; set; }
        public List<Product> PaginatedProducts { get; set; }

        // Pagination
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalProducts { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalProducts / PageSize);
    }
}