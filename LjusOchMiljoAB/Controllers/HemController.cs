using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LjusOchMiljoAB.Models;
using Microsoft.Security.Application;

namespace LjusOchMiljoAB.Controllers
{
	/*
	 * HemController har hand om alla metoder för huvudsidan och inloggning
	 * inklusive Om, Kontakt och Inloggning undersidor.
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
		//[Authorize]
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

		// GET: /Hem/Inloggning
		[AllowAnonymous]
		public ActionResult Inloggning(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		// POST: /Hem/Inloggning
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Inloggning(InloggningsModell model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Anvandare användare = användareTjänst.HämtaAnvändareMedNamn(Sanitizer.GetSafeHtmlFragment(model.Anvandarnamn));
			Status stat = Status.Misslyckades;

			if (användare != null)
			{
				stat = användareTjänst.BekräftaLösenord(användare, Sanitizer.GetSafeHtmlFragment(model.Losenord));
			}

			switch (stat)
			{
				case Status.Lyckades:
					FormsAuthentication.SetAuthCookie(Sanitizer.GetSafeHtmlFragment(model.Anvandarnamn), false);
					return RedirectToAction("Index");
				case Status.Låste:
					return View("Lockout");
				case Status.Misslyckades:
				default:
					ModelState.AddModelError("", "Ogiltig inloggningsförsök.");
					return View(model);
			}

			//	if (returnUrl != null && Url.IsLocalUrl(returnUrl))
			//		return Redirect(returnUrl);
			//	else
			//		return RedirectToAction("Index");
			//}
		}

		public ActionResult Utloggning()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index");
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