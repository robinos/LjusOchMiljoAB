using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LjusOchMiljoAB
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Hem",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Hem", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Om",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Hem", action = "Om", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Kontakt",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Hem", action = "Kontakt", id = UrlParameter.Optional }
			);

			//routes.MapRoute(
			//	name: "Produkter",
			//	url: "{controller}/{action}/{id}",
			//	defaults: new { controller = "Produkter", action = "Index", id = UrlParameter.Optional }
			//);
		}
	}
}
