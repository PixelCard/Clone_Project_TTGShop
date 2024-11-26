using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMayTinh.Models.ViewModel
{
    public class CategoriesVM
    {
        public decimal? Max_Price { get; set; }



        public decimal? Min_Price { get; set; }


        public string MoneySort { get; set; }
       
        public string SortOrder { get; set; }



        //public IEnumerable<ProductDescription> ProductDescriptions {  get; set; }


        //public IEnumerable<Product> Products { get; set; }



        //Danh sách để phân trang 
        public string BannerImage { get; set; }
        public PagedList.IPagedList<Banner> BannerID { get; set; }
        public IPagedList<Product> Products { get; set; }
        public List<Category> categories { get; set; }
    }
}