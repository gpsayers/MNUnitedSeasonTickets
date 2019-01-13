using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Calendar.Models;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace Web_Calendar.Controllers
{
	
	public class HomeController : Controller
	{
		[NoCache]
		public ActionResult Index()
		{
			return View();
		}

		public JsonResult GetEvents()
		{
			List<EventModel> eventList = JsonConvert.DeserializeObject<List<EventModel>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/eventData.json"))));

			return Json(eventList, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetEventStats()
		{
			decimal ticketPrice = 24.42M;
			var stats = new EventStats();

			List<EventModel> eventList = JsonConvert.DeserializeObject<List<EventModel>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/eventData.json"))));

			foreach (var eventItem in eventList)
			{
				if (eventItem.ticket1 == "Open")
				{
					stats.ticketsOpen++;
				}
				else
				{
					var query = stats.ticketUserList.Where(x => x.Name == eventItem.ticket1).FirstOrDefault();

					if (query != null)
					{
						query.ticketCount++;
						query.ticketValue = Math.Round(query.ticketCount * ticketPrice, 2);
					}
					else
					{
						var newTicketUser = new TicketUser
						{
							Name = eventItem.ticket1,
							ticketCount = 1,
							ticketValue = ticketPrice
						};
						stats.ticketUserList.Add(newTicketUser);
					}
				}


				if (eventItem.ticket2 == "Open")
				{
					stats.ticketsOpen++;
				}
				else
				{
					var query = stats.ticketUserList.Where(x => x.Name == eventItem.ticket2).FirstOrDefault();

					if (query != null)
					{
						query.ticketCount++;
						query.ticketValue = Math.Round(query.ticketCount * ticketPrice, 2);
					}
					else
					{
						var newTicketUser = new TicketUser
						{
							Name = eventItem.ticket2,
							ticketCount = 1,
							ticketValue = ticketPrice
						};
						stats.ticketUserList.Add(newTicketUser);
					}
				}

			}
			stats.ticketOpenValue = Math.Round(stats.ticketsOpen * ticketPrice, 2);

			return Json(stats, JsonRequestBehavior.AllowGet);
		}

		public JsonResult SubmitChange(string id, string ticket1, string ticket2)
		{

			if(String.IsNullOrEmpty(ticket1))
			{
				ticket1 = "Open";
			}

			if (String.IsNullOrEmpty(ticket2))
			{
				ticket2 = "Open";
			}

			Regex r = new Regex("^[a-zA-Z0-9]*$");
			if (!r.IsMatch(ticket1) || !r.IsMatch(ticket2))
			{
				Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}

			if (!String.IsNullOrEmpty(id))
			{
				List<EventModel> eventList = JsonConvert.DeserializeObject<List<EventModel>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/eventData.json"))));

				var eventQuery = eventList.Where(x => x.id.ToString() == id);

				if (eventQuery.Any())
				{
					var newEvent = eventQuery.FirstOrDefault();

					newEvent.ticket1 = ticket1;
					newEvent.ticket2 = ticket2;

					if (newEvent.ticket1 == "Open" && newEvent.ticket2 == "Open")
					{
						newEvent.color = "default";
					}
					else if (newEvent.ticket1 != "Open" && newEvent.ticket2 != "Open")
					{
						newEvent.color = "green";
					}
					else
					{
						newEvent.color = "orange";
					}

					var newFileString = JsonConvert.SerializeObject(eventList);
					System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/eventData.json")), newFileString);

					return Json(newEvent, JsonRequestBehavior.AllowGet);
				}

			}

			Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			return Json(new { success = false }, JsonRequestBehavior.AllowGet);

		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}