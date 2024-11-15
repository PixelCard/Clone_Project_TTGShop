using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMayTinh.Models.ViewModel
{
    public class ProductDetailsSearchVM
    {
        public string SearchTerm;


        public decimal Max_Price;


        public decimal Min_Price;

        public IEnumerable<ProductDescription> ProductDescriptions {  get; set; }


        public IEnumerable<Product> Products { get; set; }
    }
}