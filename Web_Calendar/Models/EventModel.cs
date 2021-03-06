﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Calendar.Models
{
	public class EventModel
	{
		public string title { get; set; }
		public string ticket1 { get; set; }
		public string ticket2 { get; set; }
		public string start { get; set; }
		public string end { get; set; }
		public int id { get; set; }
		public string img { get; set; }
		public string color { get; set; }
	}

	public class EventModelCollection
	{
		public List<EventModel> list { get; set; }
	}

	public class EventStats
	{
		public EventStats()
		{
			ticketsOpen = 0;
			ticketOpenValue = 0;
			ticketUserList = new List<TicketUser>();
		}

		public int ticketsOpen { get; set; }
		public decimal ticketOpenValue { get; set; }

		public List<TicketUser> ticketUserList { get; set; }
	}

	public class TicketUser
	{
		public string Name { get; set; }
		public int ticketCount { get; set; }
		public decimal ticketValue { get; set; }

		public decimal amountPaid { get; set; }
	}

	public class UserPaidModel
	{
		public string Name { get; set; }

		public decimal AmountPaid { get; set; }
	}

	public class NoCache : ActionFilterAttribute
	{
		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
			filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
			filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
			filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			filterContext.HttpContext.Response.Cache.SetNoStore();

			base.OnResultExecuting(filterContext);
		}
	}
}