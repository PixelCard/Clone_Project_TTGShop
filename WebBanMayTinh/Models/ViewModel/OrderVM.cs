using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanMayTinh.Models.ViewModel
{
    public class OrderVM
    {
        // GET: OrderVM
        public List<CartItem> CartItems { get; set; }
        public int CustomerID { get; set; }

        [Display(Name = "Ngày đặt hàng")]
        public System.DateTime OrderDate { get; set; }

        [Display(Name = "Tổng giá trị")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Trạng thái đơn hàng")]
        public string PaymentMethod { get; set; }

        public string Username { get; set; }

        //Các thuộc tính khác của đơn hàng 
        public List<OrderDetail> OrderDetails { get; set; }

        public PagedList.IPagedList<Order> Orders { get; set; }


        public PagedList.IPagedList<Product> Products { get; set; }
    }
}