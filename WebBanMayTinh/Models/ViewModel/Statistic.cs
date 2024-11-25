using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMayTinh.Models.ViewModel
{
    public class Statistic
    {
        public List<Product> Products { get; set; }

        public decimal quantity { get; set; }

        public decimal TotalRevenue { get; set; } = 0;
    }
}