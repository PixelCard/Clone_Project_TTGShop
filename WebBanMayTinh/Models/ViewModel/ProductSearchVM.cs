using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMayTinh.Models.ViewModel
{
    public class ProductSearchVM
    {
        //
        public string SearchTerm { get; set; }



        public decimal? Max_Price { get; set; }



        public decimal? Min_Price { get; set; }



        public string SortOrder { get; set; }



        //public IEnumerable<ProductDescription> ProductDescriptions {  get; set; }


        //public IEnumerable<Product> Products { get; set; }



        //Danh sách để phân trang 
        public PagedList.IPagedList<ProductDescription> ProductDescriptions {   get; set; }


        public PagedList.IPagedList<Product> Products { get; set; } 
    }
}