using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LjusOchMiljoAB;
using MvcContrib.TestHelper;
using LjusOchMiljoAB.Controllers;
using LjusOchMiljoAB.Models;
using LjusOchMiljoAB.Tests.Models;
using System.Web.Security;
using Microsoft.Security.Application;
using System.Web.Helpers;


namespace LjusOchMiljoAB.Tests.Controllers
{
	/// <summary>
	/// HemControllerTest testar controllern till huvudsidan Hem, med undersidor
	/// Om, Kontakt, Inloggning, Utlåst och Kategorier.
	/// 
	/// Index metoden (hemsidan) testas av TestHemDefaultReturnerarIndexView
	/// Kontakt metoden testas av TestHemKontaktHarDefaultText
	/// Om metoden testas av TestHemOmHarDefaultText
	/// Kategori metoden testas av TestHemKategoriReturnerarKategoriView
	/// Inloggning metoden testas av TestHemInloggningReturnerarInloggningView,
	///		TestHemInloggningLyckadesReturneraTillKategorierView och
	///		TestHemInloggningMisslyckades5GångerSkickaTillUtlåstView
	/// Utloggning metoden testas av TestHemUtloggningSkickarTillIndexView
	/// 
	/// Version: 1.0
	/// 2014-12-10
	/// Grupp 2
	/// </summary>
	[TestClass]
	public class HemControllerTest
	{
		/// <summary>
		/// GetAnvändareTjänst är en static metod skapar en IMinnetAnvändareTjänst.
		/// Det är en mock version av AnvändareTjänst som används vid testning.
		/// </summary>
		private static IAnvändareTjänst GetAnvändareTjänst()
		{
			IMinnetAnvändareTjänst användareTjänst = new IMinnetAnvändareTjänst();
			return användareTjänst;
		}

		/// <summary>
		/// CreateController är en static metod som skapar en mock HemController
		/// som sätts att använda en IAnvändareTjänst.
		/// 
		/// Mock HemController skapas med TestControllerBuilder från
		/// MvcContrib.TestHelper och autogenereras med alla Asp.Net implementeringar
		/// som Session, Response, Request, etc.
		/// </summary>
		/// <param name="användareTjänst">En IAnvändareTjänst</param>
		private static HemController CreateController(IAnvändareTjänst användareTjänst)
		{
			TestControllerBuilder builder = new TestControllerBuilder();
			return builder.CreateController<HemController>(användareTjänst);
		}

		/// <summary>
		/// HämtaAnvandare (utan parameter) skapar en default hämtning av en användare.
		/// </summary>
		Anvandare HämtaAnvandare()
		{
			return HämtaAnvandare(1, "kund", "password");
		}

		/// <summary>
		/// HämtaAnvandare skapar olika default användare för testning beroende på
		/// parameter.
		/// </summary>
		/// <param name="id">En integer som användare id</param>
		/// <param name="namn">En sträng som användarnamn</param>
		/// <param name="lösenord">En sträng som lösenord</param>
		Anvandare HämtaAnvandare(int id, string namn, string lösenord)
		{
			return new Anvandare
			{
				ID = id,
				Anvandarnamn = namn,
				LosenordHash = Crypto.HashPassword(lösenord),
				Roll = "kund",
				Raknare = 0,
				Last = false
			};
		}

		/// <summary>
		/// TestHemDefaultReturnerarIndexView testar att Index metoden för
		/// huvudsidan visar "Index" vyn.
		/// 
		/// AssertViewRendered kommer från MvcContrib.TestHelper.
		/// </summary>
		[TestMethod]
		public void TestHemDefaultReturnerarIndexView()
		{
			// Arrange              
			var controller = CreateController(GetAnvändareTjänst());

			// Act
			var result = controller.Index();

			// Assert
			result.AssertViewRendered().ForView("Index");
		}

		/// <summary>
		/// TestHemOmHarDefaultText testar att Om-sidan returnerar default text
		/// om företaget och hemsidan.
		/// </summary>
		[TestMethod]
		public void TestHemOmHarDefaultText()
		{
			// Arrange
			var controller = CreateController(GetAnvändareTjänst());

			// Act
			ViewResult result = controller.Om() as ViewResult;

			// Assert
			Assert.AreEqual("Om hemsidan och Ljus och Miljö AB", result.ViewBag.Message);
		}

		/// <summary>
		/// TestHemKontaktHarDefaultText testar att Kontakt-sidan returnerar default
		/// text om företagkontakt.
		/// </summary>
		[TestMethod]
		public void TestHemKontaktHarDefaultText()
		{
			// Arrange
			var controller = CreateController(GetAnvändareTjänst());

			// Act
			ViewResult result = controller.Kontakt() as ViewResult;

			// Assert
			Assert.AreEqual("Kontaktsidan", result.ViewBag.Message);
		}

		/// <summary>
		/// TestHemKategorierReturnerarKategorierView testar att Kategorier metoden
		/// för kategorisidan visar "Kategorier" vyn.
		/// 
		/// AssertViewRendered kommer från MvcContrib.TestHelper
		/// </summary>
		[TestMethod]
		public void TestHemKategorierReturnerarKategorierView()
		{
			// Arrange              
			var controller = CreateController(GetAnvändareTjänst());

			// Act
			var result = controller.Kategorier();

			// Assert
			result.AssertViewRendered().ForView("Kategorier");
		}

		/// <summary>
		/// TestHemInloggningReturnerarInloggningView testar att Inloggning
		/// metoden vid HttpGet för inloggningssidan visar "Inloggning".
		/// 
		/// AssertViewRendered kommer från MvcContrib.TestHelper.
		/// </summary>
		[TestMethod]
		public void TestHemInloggningReturnerarInloggningView()
		{
			// Arrange
			var controller = CreateController(GetAnvändareTjänst());
			string returnUrl = "Index";

			// Act
			var result = controller.Inloggning(returnUrl);

			// Assert
			result.AssertViewRendered().ForView("Inloggning");
		}

		/// <summary>
		/// TestHemInloggningLyckadesReturneraTillKategorierView testar att
		/// Inloggning metoden vid HttpPost för inloggningssidan skickar
		/// användaren till Kategorier i HemController vid lyckade inloggning.
		/// 
		/// För att använda Inloggning som är async använder man .Result i
		/// slutet och inte 'await'.
		/// 
		/// AssertViewRendered kommer från MvcContrib.TestHelper
		/// </summary>
		[TestMethod]
		public void TestHemInloggningLyckadesReturneraTillKategorierView()
		{
			// Arrange
			IMinnetAnvändareTjänst användareTjänst = new IMinnetAnvändareTjänst();
			Anvandare användare1 = HämtaAnvandare(1, "testkund", "testlösenord");
			användareTjänst.SkapaAnvändare(användare1);
			var controller = CreateController(användareTjänst);

			string returnUrl = "Index";
			InloggningsModell modell = new InloggningsModell()
				{ Anvandarnamn = "testkund", Losenord = "testlösenord" };

			// Act
			var result = controller.Inloggning(modell, returnUrl).Result;

			// Assert
			result.AssertActionRedirect().ToAction<HemController>(c => c.Kategorier());
		}

		/// <summary>
		/// TestHemUtloggningSkickarTillIndexView testar att Utloggning metoden vid
		/// HttpPost för utloggningssidan skickar användaren till hemsidan (Index).
		/// 
		/// AssertActionRedirect kommer från MvcContrib.TestHelper.
		/// </summary>
		[TestMethod]
		public void TestHemUtloggningSkickarTillIndexView()
		{
			// Arrange
			var controller = CreateController(GetAnvändareTjänst());

			// Act
			var result = (RedirectToRouteResult)controller.Utloggning();

			// Assert
			result.AssertActionRedirect().ToAction<HemController>(c => c.Index());
		}

		/// <summary>
		/// TestHemInloggningMisslyckades5GångerSkickaTillUtlåstView testar att
		/// Inloggning metoden vid HttpPost för inloggningssidan skickar användaren
		/// till Utlåst i HemController vid 5:e+ misslyckade inloggning.
		/// 
		/// För att använda Inloggning som är async använder man .Result i
		/// slutet och inte 'await'.
		/// 
		/// AssertViewRendered kommer från MvcContrib.TestHelper
		/// </summary>
		[TestMethod]
		public void TestHemInloggningMisslyckades5GångerSkickaTillUtlåstView()
		{
			// Arrange
			IMinnetAnvändareTjänst användareTjänst = new IMinnetAnvändareTjänst();
			Anvandare användare1 = HämtaAnvandare(1, "testkund", "testlösenord");
			användare1.Raknare = 4;
			användareTjänst.SkapaAnvändare(användare1);
			var controller = CreateController(användareTjänst);

			string returnUrl = "Index";
			InloggningsModell modell = new InloggningsModell() { Anvandarnamn = "testkund", Losenord = "lösenord" };

			// Act
			var result = controller.Inloggning(modell, returnUrl).Result;

			// Assert
			result.AssertActionRedirect().ToAction<HemController>(c => c.Utlåst());
		}
	}
}
