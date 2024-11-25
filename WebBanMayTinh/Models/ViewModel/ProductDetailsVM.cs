using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMayTinh.Models.ViewModel
{
    public class ProductDetailsVM
    {
        public Product products { get; set; }

        public ProductDescription productdescription { get; set; }

        public int quantity { get; set; } = 1;
        

        public string Text_DiscountValue { get; set; } = "";


        public decimal TotalPrice => quantity * products.ProductPrice;


        //Các thuộc tính phân trang
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}