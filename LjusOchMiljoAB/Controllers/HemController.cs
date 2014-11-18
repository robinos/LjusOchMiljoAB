using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LjusOchMiljoAB.Models;
using Microsoft.Security.Application;
using System.Threading.Tasks;

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
		 * Alla kan se Index sidan.
		 */
		public ActionResult Index()
		{
			return View("Index");
		}

		/*
		 * Om sidan ger information om företaget och hemsidan.
		 * Alla kan se Om sidan.
		 */
		public ActionResult Om()
		{
			ViewBag.Message = "Om hemsidan och Ljus och Miljö AB";

			return View("Om");
		}

		/*
		 * Kontakt sidan ger kontaktinformation för företaget.
		 * Alla kan se Kontakt sidan.
		 */
		public ActionResult Kontakt()
		{
			ViewBag.Message = "Kontaktsidan";

			return View("Kontakt");
		}

		/*
		 * HttpGET för Inloggningssidan.  Visar formen.
		 * 
		 * in: returnUrl: sidan man borde returnera till
		 */
		[AllowAnonymous]
		public ActionResult Inloggning(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View("Inloggning");
		}

		/*
		 * HttpPost för Inloggningssidan.  Den validerar modellen och sedan försöker
		 * bekräfta lösenordet.  Om det lyckas fortsätter man.  Om det misslyckas
		 * stannar man på formen.  Om man blir utlåste (för många misslyckade
		 * försök på lösenordet) skickas man till Lockout sidan.
		 * 
		 * in:	model: InloggningsModell för inloggningsformen
		 *		returnUrl: sidan man borde returnera till
		 */
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Inloggning(InloggningsModell model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Status stat = Status.Misslyckades;
			stat = await användareTjänst.BekräftaLösenord(Sanitizer.GetSafeHtmlFragment(model.Anvandarnamn), Sanitizer.GetSafeHtmlFragment(model.Losenord));

			switch (stat)
			{
				case Status.Lyckades:
					användareTjänst.Inloggning(Sanitizer.GetSafeHtmlFragment(model.Anvandarnamn));
					//FormsAuthentication.SetAuthCookie(Sanitizer.GetSafeHtmlFragment(model.Anvandarnamn), false);
					return RedirectToAction("Index", "Hem");
				case Status.Låste:
					return View("Lockout");
				case Status.Misslyckades:
				default:
					ModelState.AddModelError("", "Ogiltig inloggningsförsök.");
					return View(model);
			}
		}

		/*
		 * Vid utloggning skickas man tillbaka till huvudsidan.
		 */
		public ActionResult Utloggning()
		{
			//FormsAuthentication.SignOut();
			användareTjänst.Utloggning();
			return RedirectToAction("Index", "Hem");
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