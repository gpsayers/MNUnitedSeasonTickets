using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Calendar.Models;

namespace Web_Calendar
{
    public class CustomStart
    {
        public static void Initialize()
        {

        }

        public static void Custom()
        {

        }

        public static void Custom3()
        {
            List<EventModel> eventList = JsonConvert.DeserializeObject<List<EventModel>>(System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/eventData.json")));

            List<UserModel> userModelList = JsonConvert.DeserializeObject<List<UserModel>>(System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/userModelList.json")));


            var ticketModelList = new List<TicketModel>();

            var userTicketModelList = new List<UserTicketModel>();

            var i = 0;

            foreach (var item in eventList)
            {
                userTicketModelList = new List<UserTicketModel>();

                var ticket1query = userModelList.Where(x => x.userName == item.ticket1).FirstOrDefault();

                var ticket2query = userModelList.Where(x => x.userName == item.ticket1).FirstOrDefault();

                userTicketModelList.Add(new UserTicketModel
                {
                    userId = ticket1query.userId,
                    ticketPrice = 23.06m
                });

                userTicketModelList.Add(new UserTicketModel
                {
                    userId = ticket2query.userId,
                    ticketPrice = 23.06m
                });

                ticketModelList.Add(new TicketModel
                {
                    gameId = i,
                    yearId = 0,
                    teamId = 0,
                    gameTime = item.title,
                    color = item.color,
                    start = item.start,
                    end = item.end,
                    userTicketModelList = userTicketModelList
                });

                i++;

            }

            var thislist = new List<YearModel>();

            var yearModel = new YearModel
            {
                yearId = 0,
                yearName = "2019",
                ticketModelList = ticketModelList

            };

            thislist.Add(yearModel);
            thislist.Add(new YearModel { yearId = 1, yearName = "2020", ticketModelList = new List<TicketModel>() });

            var newFileString = JsonConvert.SerializeObject(thislist);
            System.IO.File.WriteAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/yearModel.json"), newFileString);

        }


        public static void Custom1()
        {


            List<EventModel> eventList = JsonConvert.DeserializeObject<List<EventModel>>(System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/eventData.json")));

            var yearModel = new YearModel
            {
                yearId = 0,
                yearName = "2019",
                ticketModelList = new List<TicketModel>()

            };

            var temp = new List<string>();

            foreach (var item in eventList)
            {

                temp.Add(item.ticket1);
                temp.Add(item.ticket2);


            }

            var userList = new List<UserModel>();
            var i = 0;

            foreach (var item in temp.Distinct())
            {

                List<UserPaidModel> paidList = JsonConvert.DeserializeObject<List<UserPaidModel>>(System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/ticketBreakdown.json")));

                decimal? amountPaid = paidList.Where(x => x.Name == item).Select(x => x.AmountPaid).FirstOrDefault();

                userList.Add(new UserModel
                {
                    userName = item,
                    userId = i,
                    totalUserPaid = amountPaid ?? 0m,
                    userModelPaidList = new List<UserModelPaid>() { new UserModelPaid { amountPaid = amountPaid ?? 0m, yearId = 0 } }
                }); ; ; ; ;

                i++;
            }

            var newFileString = JsonConvert.SerializeObject(userList);
            System.IO.File.WriteAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/userModelList.json"), newFileString);

        }

    }
}