using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanMayTinh.Models.ViewModel
{
    public class LoginVM
    {
        internal object Username;

        [Required(ErrorMessage ="Vui lòng nhập email")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Email không hợp lệ")]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name ="Mật khẩu")]
        public string Password { get; set; }
    }
}