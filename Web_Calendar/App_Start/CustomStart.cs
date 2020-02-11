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