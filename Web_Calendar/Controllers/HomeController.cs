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
using Web_Calendar.Filters;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

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
			decimal ticketPrice = 23.06M;
			var stats = new EventStats();

			List<EventModel> eventList = JsonConvert.DeserializeObject<List<EventModel>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/eventData.json"))));

			List<UserPaidModel> paidList = JsonConvert.DeserializeObject<List<UserPaidModel>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/ticketBreakdown.json"))));


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
						query.ticketValue = query.ticketCount * ticketPrice;
					}
					else
					{
						var newTicketUser = new TicketUser
						{
							Name = eventItem.ticket2,
							ticketCount = 1,
							ticketValue = Math.Round(ticketPrice, 2)
						};
						stats.ticketUserList.Add(newTicketUser);
					}
				}

			}
			stats.ticketOpenValue = Math.Round(stats.ticketsOpen * ticketPrice, 2);

			foreach (var statsItem in stats.ticketUserList)
			{
				statsItem.amountPaid = 0;

				foreach (var paidItem in paidList)
				{
					if (statsItem.Name == paidItem.Name)
					{
						statsItem.amountPaid = paidItem.AmountPaid;
					}
				}
			}

			return Json(stats, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetAmmountPaid()
		{

			List<UserPaidModel> paidList = JsonConvert.DeserializeObject<List<UserPaidModel>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/ticketBreakdown.json"))));

			return Json(paidList, JsonRequestBehavior.AllowGet);
		}

		public JsonResult SubmitPaidChange(string name, decimal amountPaid )
		{
			try
			{
				List<UserPaidModel> paidList = JsonConvert.DeserializeObject<List<UserPaidModel>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/ticketBreakdown.json"))));

				var paidItem = paidList.Where(x => x.Name == name).FirstOrDefault();

				if (paidItem == null)
				{
					paidList.Add(new UserPaidModel { Name = name, AmountPaid = amountPaid });
				}
				else
				{
					paidItem.Name = name;
					paidItem.AmountPaid = amountPaid;
				}

				var newFileString = JsonConvert.SerializeObject(paidList);
				System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/ticketBreakdown.json")), newFileString);

				return Json(paidList, JsonRequestBehavior.AllowGet);
			}
			catch
			{
				//if fail return error
				Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}


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

			Regex r = new Regex("^[a-zA-Z0-9 ]*$");
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

		[HttpGet]
		public ActionResult Login()
		{

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginModel model)
		{
			// incremental delay to prevent brute force attacks
			int incrementalDelay;
			if (HttpContext.Application[Request.UserHostAddress] != null)
			{
				// wait for delay if there is one
				incrementalDelay = (int)HttpContext.Application[Request.UserHostAddress];
				await Task.Delay(incrementalDelay * 1000);
			}

			if (ModelState.IsValid && !String.IsNullOrEmpty(model.password) && !String.IsNullOrEmpty(model.userName))
			{
				if (GetHash(model.password) == "3057dd9e2b8b39c2b4bc4cd38ea4af1ddd69c9d05abdddf7c98dc1ae1699a27d" && 
					GetHash(model.userName) == "c262f32d4a2b5ea4629efbae471f827d7588a0b5ae296f6d8d24063747966061")
				{
					Session["userName"] = model.userName;

					// reset incremental delay on successful login
					if (HttpContext.Application[Request.UserHostAddress] != null)
					{
						HttpContext.Application.Remove(Request.UserHostAddress);
					}

					return RedirectToAction("Admin", "Home");


				}


			}

			// login failed
			// increment the delay on failed login attempts
			if (HttpContext.Application[Request.UserHostAddress] == null)
			{
				incrementalDelay = 1;
			}
			else
			{
				incrementalDelay = (int)HttpContext.Application[Request.UserHostAddress] * 2;
			}
			HttpContext.Application[Request.UserHostAddress] = incrementalDelay;

			ViewBag.Message = "The user name or password provided is incorrect.";
			return View();

		}

		[HttpGet]
		[CustomAuthentication]
		public ActionResult Admin()
		{
			var model = new EventModelCollection();

			model.list = JsonConvert.DeserializeObject<List<EventModel>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/eventData.json"))));

			return View();
		}

		[HttpPost]
		[CustomAuthentication]
		public ActionResult Admin(string jsonText)
		{
			try
			{
				var temp = JsonConvert.DeserializeObject<List<EventModel>>(jsonText);
			}
			catch
			{
				ViewBag.Message = "Invalid Json";
				return View();
			}

			System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/eventData.json")), jsonText);
			return RedirectToAction("Index");
		}

		public ActionResult Logout()
		{
			Session.Remove("userName");

			return RedirectToAction("Index");
		}

		private string GetHash(string rawData)
		{
			// Create a SHA256   
			using (SHA256 sha256Hash = SHA256.Create())
			{
				// ComputeHash - returns byte array  
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

				// Convert byte array to a string   
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}
	}
}