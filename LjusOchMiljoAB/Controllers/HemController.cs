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
	 * [RequireHttps] tvingar sidan att vara HTTPS och använder SSL säkerhet
	 * [HandleError] är inte någonting som används i nuläget
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 11
	 * Version: 0.19
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
		 * [AllowAnonymous] tillåter användare som är inte inloggad att se det
		 */
		[AllowAnonymous]
		public ActionResult Index()
		{
			return View("Index");
		}

		/*
		 * Om sidan ger information om företaget och hemsidan.
		 * Alla kan se Om sidan.
		 * [AllowAnonymous] tillåter användare som är inte inloggad att se det
		 */
		[AllowAnonymous]
		public ActionResult Om()
		{
			ViewBag.Message = "Om hemsidan och Ljus och Miljö AB";

			return View("Om");
		}

		/*
		 * Kontakt sidan ger kontaktinformation för företaget.
		 * Alla kan se Kontakt sidan.
		 * [AllowAnonymous] tillåter användare som är inte inloggad att se det
		 */
		[AllowAnonymous]
		public ActionResult Kontakt()
		{
			ViewBag.Message = "Kontaktsidan";

			return View("Kontakt");
		}

		/*
		 * HttpGET för Inloggningssidan.  Visar formen.
		 * [AllowAnonymous] tillåter användare som är inte inloggad att se det
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
		 * försök på lösenordet) skickas man till Utlåste sidan.
		 * 
		 * [AllowAnonymous] tillåter användare som är inte inloggad att se det
		 * [ValidateAntiForgeryToken] tittar på en gömd kaka i webbformen för att
		 * testa att det verkligen kommer från samma användaren/webbläsaren som fick
		 * HttpGet sidan
		 * 
		 * in:	model: InloggningsModell för inloggningsformen
		 *		returnUrl: sidan man borde returnera till
		 */
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Inloggning(InloggningsModell model, string returnUrl)
		{
			//Om modellen är fel (som en fält är tom, etc) går man tillbaka till
			//inloggningsformen
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			//Status är en enum som finns definerad i IAnvändareTjänst. Det sätts till
			//misslyckade som default
			Status stat = Status.Misslyckades;

			//Santizer används för att ta bort farliga tecken och kod frpn Html
			//inmatningen
			stat = await användareTjänst.BekräftaLösenord(Sanitizer.GetSafeHtmlFragment(model.Anvandarnamn), Sanitizer.GetSafeHtmlFragment(model.Losenord));

			switch (stat)
			{
				//Om lösenordet stämmer (och det var mindre än 5 misslyckade
				//inloggningar innan) logga in användaren
				case Status.Lyckades:
					användareTjänst.Inloggning(Sanitizer.GetSafeHtmlFragment(model.Anvandarnamn));
					//Därefter skickas användaren till hemsidan
					return RedirectToAction("Kategorier", "Hem");
				//Vid 5+ misslyckade inloggningar skickas användare till Utlåste sidan
				case Status.Låste:
					return RedirectToAction("Utlåste", "Hem");
				//Vid Misslyckades som är också default skickas man tillbaka till
				//inloggnings formen
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
		 * Utlåste visas för folk men 5 eller mer misslyckade inloggningar.
		 * [AllowAnonymous] tillåter användare som är inte inloggad att se det
		 */
		[AllowAnonymous]
		public ActionResult Utlåst()
		{
			return View("Utlåst");
		}

		/*
		 * Kategorier visar våra kategorier.  Man måste vara inloggad för att
		 * komma hit.
		 */
		[Authorize]
		public ActionResult Kategorier()
		{
			return View("Kategorier");
		}
	}
}