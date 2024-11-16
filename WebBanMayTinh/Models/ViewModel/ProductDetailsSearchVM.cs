using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMayTinh.Models.ViewModel
{
    public class ProductDetailsSearchVM
    {
        public string SearchTerm;


        public decimal? Max_Price;


        public decimal? Min_Price;


        public string SortOrder;

        //public IEnumerable<ProductDescription> ProductDescriptions {  get; set; }


        //public IEnumerable<Product> Products { get; set; }



        //Danh sách để phân trang 
        public PagedList.IPagedList<ProductDescription> ProductDescriptions {   get; set; }


        public PagedList.IPagedList<Product> Products { get; set; } 
    }
}