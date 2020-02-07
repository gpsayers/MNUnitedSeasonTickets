using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Calendar.Models
{
    public class TicketModel
    {
        public int gameId { get; set; }
        public int yearId { get; set; }

        public int teamId { get; set; }

        public string gameTime { get; set; }

        public string color { get; set; }

        public string start { get; set; }
        public string end { get; set; }

        public List<UserTicketModel> userTicketModelList { get; set; }

    }

    public class UserTicketModel
    {
        public int userId { get; set; }

        public decimal ticketPrice { get; set; }

    }
}