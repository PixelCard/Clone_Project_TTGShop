using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMayTinh.Models.ViewModel
{
    public class EditUserVM
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }
        public string UserGender { get; set; }
        public DateTime? UserBirthDay { get; set; }
    }

}