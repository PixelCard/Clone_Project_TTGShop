using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMayTinh.Models.ViewModel
{
    public class SearchVM
    {
        public string searchTerm { get; set; }


        //Biến để phân trang
        public int pagenumber;
        public int page;


        //Products Tìm kiếm đươc phân trang
        public IPagedList<Product> Products_SearchPageList;
    }
}