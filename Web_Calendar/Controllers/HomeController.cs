using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Calendar.Models;
using System.IO;

namespace Web_Calendar.Controllers
{
	public class HomeController : Controller
	{
		[NoCache]
		public ActionResult Index()
		{
			return View();
		}

		public JsonResult SubmitChange(string id, string ticket1, string ticket2)
		{
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