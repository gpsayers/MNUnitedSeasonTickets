using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Web_Calendar.Models
{
	public class LoginModel
	{
		[DisplayName("User")]
		public string userName { get; set; }

		[DisplayName("Password")]
		public string password { get; set; }
	}
}