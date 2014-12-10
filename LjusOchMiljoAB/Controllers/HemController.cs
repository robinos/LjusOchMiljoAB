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
	/// <summary>
	/// HemController har hand om alla metoder för huvudsidan och inloggning
	/// inklusive Om, Kontakt och Inloggning undersidor.
	/// 
	/// [RequireHttps] tvingar sidan att vara HTTPS och använder SSL säkerhet
	/// [HandleError] är inte någonting som används i nuläget
	///		
	/// -Konstruktor-
	/// HemController - tom konstruktor som använder en ny AnvändareTjänst som
	///		skickas till den andra konstruktor (används vid riktig körning).
	///	HemController(IAnvändareTjänst användareTjänst) - tar emot en IAnvändareTjänst
	///		för initialisering
	/// 
	/// -Metoder-
	/// Index - Hemsidans kod.  Hemsidan kan ses av alla.
	/// Om - Omsidans kod. Omsidan kan ses av alla.
	/// Kontakt - Kontaktsidans kod. Kontaktsidan kan ses av alla.
	/// [HttpGET] Inloggning - Koden vid uppvisning av Inloggningssidan.
	///		Inloggningssidan kan ses av alla.
	/// [HttpPost] Inloggning - Koden som tar in inmatningar från användaren. 
	/// Utloggning - Koden för utloggning. 
	/// Utlåst - Koden för Utlåst. Utlåstsidan kan ses av alla men man kommer bara dit
	///		vanligtvis av att ha fått kontot utlåst.
	/// Kategorier - Koden för kategorisidan. Bara inloggade användare kan se sidan.
	/// 
	/// Version: 1.0
	/// 2014-12-10
	/// Grupp 2
	/// </summary>
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

		//En-parameter konstruktör för initialisera av en IAnvändareTjänst
		public HemController(IAnvändareTjänst användareTjänst)
		{
			this.användareTjänst = användareTjänst;
		}

		/// <summary>
		/// Index är koden för hemsidan. Den visar upp Index vyn som är hemsidan
		///		(Views-Hem->Index.cshtml)
		/// [AllowAnonymous] tillåter användare som är inte inloggad att se den.
		/// </summary>
		/// <returns>ActionResult av att visa vyn</returns>
		[AllowAnonymous]
		public ActionResult Index()
		{
			return View("Index");
		}

		/// <summary>
		/// Om är koden för omsidan. Den visar upp Om vyn som är information om
		///		företaget och hemsidan (Views-Hem->Om.cshtml)
		/// [AllowAnonymous] tillåter användare som är inte inloggad att se den.
		/// </summary>
		/// <returns>ActionResult av att visa vyn</returns>
		[AllowAnonymous]
		public ActionResult Om()
		{
			ViewBag.Message = "Om hemsidan och Ljus och Miljö AB";

			return View("Om");
		}

		/// <summary>
		/// Kontakt är koden för kontaktsidan. Den visar upp Om vyn som är
		///		information om företaget (Views-Hem->Kontakt.cshtml)
		/// [AllowAnonymous] tillåter användare som är inte inloggad att se den.
		/// </summary>
		/// <returns>ActionResult av att visa vyn</returns>
		[AllowAnonymous]
		public ActionResult Kontakt()
		{
			ViewBag.Message = "Kontaktsidan";

			return View("Kontakt");
		}

		/// <summary>
		/// HttpGet för Inloggningsidan. Visar upp inloggningsformen.
		/// [AllowAnonymous] tillåter användare som är inte inloggad att se den.
		/// </summary>
		/// <param name="returnUrl">sidan där man är (från vyn)</param>
		/// <returns>ActionResult av att visa vyn</returns>
		[AllowAnonymous]
		public ActionResult Inloggning(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View("Inloggning");
		}

		/// <summary>
		/// HttpPost för Inloggningsidan. Den validerar modellen och sedan försöker
		/// bekräfta lösenordet.  Om det lyckas fortsätter man.  Om det misslyckas
		/// stannar man på formen.  Om man blir utlåste (för många misslyckade
		/// försök på lösenordet) skickas man till Utlåst sidan.
		/// Metoden är async för att ta emot från async metoden BekräftaLösenord men
		/// även för att tjänster ofta använder async metoder för att vänta på svar
		/// och det underlätta om man skulle vilja byta ut tjänsten mot något icke
		/// lokal tjänst.
		/// 
		/// [AllowAnonymous] tillåter användare som är inte inloggad att se den.
		/// [ValidateAntiForgeryToken] tittar på en gömd kaka i webbformen för att
		/// testa att det verkligen kommer från samma användaren/webbläsaren som fick
		/// HttpGet sidan.
		/// 
		/// Version: 1.0
		/// 2014-12-10
		/// Grupp 2
		/// </summary>
		/// <param name="model">InloggningsModell med användarnamn och lösenord och
		///		begränsningar på inmatningar</param>
		/// <returns>Task för async och ActionResult av att visa vyn</returns>
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
			//Misslyckade som default
			Status stat = Status.Misslyckades;

			//Santizer används för att ta bort farliga tecken och kod från Html
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
				//Vid 5+ misslyckade inloggningar skickas användare till Utlåst sidan
				case Status.Låst:
					return RedirectToAction("Utlåst", "Hem");
				//Vid Misslyckades som är också default skickas man tillbaka till
				//inloggnings formen
				case Status.Misslyckades:
				default:
					ModelState.AddModelError("", "Ogiltig inloggningsförsök.");
					return View(model);
			}
		}

		/// <summary>
		/// Utloggning är koden för att logga ut. Detta ändrar menyvalet till
		/// "logga in" men Utloggning är inte kopplad till en full vy.
		/// </summary>
		/// <returns>ActionResult av att visa vyn</returns>
		public ActionResult Utloggning()
		{
			//logga ut från FormsAuthentication med hjälp av användareTjänst
			användareTjänst.Utloggning();
			//man skickas tillbaka till hemsidan
			return RedirectToAction("Index", "Hem");
		}

		/// <summary>
		/// Utlåst är koden för Utlåstsidan som visas för folk men 5 eller mer
		///		misslyckade inloggningar. (Views-Hem->Låst.cshtml)
		/// [AllowAnonymous] tillåter användare som är inte inloggad att se den.
		/// </summary>
		/// <returns>ActionResult av att visa vyn</returns>
		[AllowAnonymous]
		public ActionResult Utlåst()
		{
			return View("Utlåst");
		}

		/// <summary>
		/// Kategorier är koden för kategorisidan. Den visar upp Om vyn som är
		///		information om företaget (Views-Hem->Utlåst.cshtml)
		/// [Authorize] tillåter bara inloggad användare att se den.  Försök att
		///		komma till sidan utan att vara inloggad resulterar att man skickas
		///		till inloggningssidan.
		/// </summary>
		/// <returns>ActionResult av att visa vyn</returns>
		[Authorize]
		public ActionResult Kategorier()
		{
			return View("Kategorier");
		}
	}
}