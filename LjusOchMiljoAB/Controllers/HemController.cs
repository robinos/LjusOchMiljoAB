using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LjusOchMiljoAB.Models;

namespace LjusOchMiljoAB.Controllers
{
	/*
	 * HemController har hand om alla metoder för huvudsidan inklusive Om
	 * och Kontakt undersidor.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 11
	 * Version: 0.18
	 */
	[RequireHttps]
	[HandleError]
	public class HemController : Controller
	{
		//IAnvändareTjänst hanterar kommunikation med tjänsten som hanterar
		//användare åt HemController
		public IAnvändareTjänst användareTjänst;

		//Vid tom konstruktör, gör en ny tjänst av typen som används för
		//verklig körning
		public HemController() : this(new AnvändareTjänst()) { }

		//En-parameter konstruktör för testning mot en egen tjänst
		public HemController(IAnvändareTjänst användareTjänst)
		{
			this.användareTjänst = användareTjänst;
		}

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

		[HttpGet]
		public ActionResult Inloggning()
		{
			return this.View();
		}

		[HttpPost]
		public ActionResult Inloggning(string användarnamn, string lösenord, string returnUrl)
		{
			Anvandare användare = användareTjänst.HämtaAnvändareMedNamn(användarnamn);
			if (användare != null && användareTjänst.BekräftaLösenord(användare, lösenord))
			{
				FormsAuthentication.SetAuthCookie(användarnamn, false);
				if (returnUrl != null && Url.IsLocalUrl(returnUrl))
					return Redirect(returnUrl);
				else
					return RedirectToAction("Index");
			}

			ModelState.AddModelError("", "Ogiltig användarnamn eller lösenord");
			return View();
		}

		public ActionResult Utloggning()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index");
		}

		[Authorize]
		public ActionResult SimpleAuthorization()
		{
			return this.Content("Du är autentiserad!");
		}

		[Authorize(Users = "kund")]
		public ActionResult AuthorizedAsFlytzen()
		{
			return this.Content("Du är autentiserad som kund.");
		}

		[Authorize(Roles = "Administrator")]
		public ActionResult AuthorizedAsAdministrator()
		{
			return this.Content("Du är autentiserad som administratör");
		}

		/*
		[HttpGet]
		public ActionResult SkapaAnvändare()
		{
			return this.View();
		}

		[HttpPost]
		public ActionResult SkapaAnvändare(string användarnamn, string lösenord, string roll)
		{
			var newUser = new Anvandare()
			{
				ID = 1,
				Anvandarnamn = användarnamn,
				Roll = roll,
				Raknare = 0,
				Laste = false
			};
			användareTjänst.SättLösenord(newUser, lösenord);
			användareTjänst.SkapaAnvändare(newUser);
			return RedirectToAction("Index");
		}*/
	}
}