using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Calendar.Models
{
    public class YearModel
    {
        public int yearId { get; set; }
        public string yearName { get; set; }

        public List<TicketModel> ticketModelList { get; set; }

    }
}