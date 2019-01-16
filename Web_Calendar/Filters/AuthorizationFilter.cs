using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using Web_Calendar.Controllers;

namespace Web_Calendar.Filters
{
	public class CustomAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter
	{
		public void OnAuthentication(AuthenticationContext filterContext)
		{
			if (HttpContext.Current.Session["userName"] == null || HttpContext.Current.Session["userName"].ToString() != "gsayers")
			{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{
					{ "controller", "Home" },
					{ "action", "Login" }
				});
			}
		}

		public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
		{
			return;
		}
	}

}