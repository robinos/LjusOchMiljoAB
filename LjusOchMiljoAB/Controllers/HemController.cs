using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LjusOchMiljoAB.Models;

namespace LjusOchMiljoAB.Controllers
{
	[HandleError]
	public class HemController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Om()
		{
			ViewBag.Message = "Om hemsidan och Ljus och Miljö AB";

			return View();
		}

		public ActionResult Kontakt()
		{
			ViewBag.Message = "Kontaktsidan";

			return View();
		}
	}
}