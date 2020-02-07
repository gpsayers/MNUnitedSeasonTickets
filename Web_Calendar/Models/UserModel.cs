using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Calendar.Models
{
    public class UserModel
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public decimal totalUserPaid { get; set; }
        
        public List<UserModelPaid> userModelPaidList { get; set; }

    }

    public class UserModelPaid
    {

        public int yearId { get; set; }

        public decimal amountPaid { get; set; }
    }

}