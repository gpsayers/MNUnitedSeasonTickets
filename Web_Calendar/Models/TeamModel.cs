using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Calendar.Models
{
    public class TeamModel
    {
        public int teamId { get; set; }
        public string teamName { get; set; }
        public string teamNameShort { get; set; }
        public string imagePath { get; set; }
    }
}