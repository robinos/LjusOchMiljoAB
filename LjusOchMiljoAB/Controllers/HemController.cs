using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LjusOchMiljoAB.Models;

namespace LjusOchMiljoAB.Controllers
{
	/*
	 * HemController har hand om alla metoder för huvudsidan inklusive Om
	 * och Kontakt undersidor.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 04
	 * Version: 0.17
	 */
	[RequireHttps]
	[HandleError]
	public class HemController : Controller
	{
		/*
		 * Index är själva huvudsidan av applikationen (med vy Views ->
		 * Hem -> Index.cshtml).
		 */
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