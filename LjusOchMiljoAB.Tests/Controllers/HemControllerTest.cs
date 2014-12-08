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
	/*
	 * HemControllerTest testar controllern till huvudsidan Hem, med undersidor 
	 * Om, Kontakt, Inloggning och Utlåste.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 18
	 * Version: 0.19
	 */
	[TestClass]
	public class HemControllerTest
	{
		/*
		 * Skapa en AnvändareTjänst
		 */
		private static IAnvändareTjänst GetAnvändareTjänst()
		{
			IMinnetAnvändareTjänst användareTjänst = new IMinnetAnvändareTjänst();
			return användareTjänst;
		}

		/*
		 * Med TestControllerBuilder från MvcContrib.TestHelper är en mock
		 * HemController autogenererad med alla Asp.Net implementation med som
		 * Session, Response, Request, etc.
		 */
		private static HemController CreateController(IAnvändareTjänst användareTjänst)
		{
			TestControllerBuilder builder = new TestControllerBuilder();
			return builder.CreateController<HemController>(användareTjänst);
		}

		/*
		 * Default hämtning
		 */
		Anvandare HämtaAnvandare()
		{
			return HämtaAnvandare(1, "kund", "password");
		}

		/*
		 * Default användare att skapa nya med för testning
		 */
		Anvandare HämtaAnvandare(int id, string namn, string password)
		{
			return new Anvandare
			{
				ID = id,
				Anvandarnamn = namn,
				LosenordHash = Crypto.HashPassword(password),
				Roll = "kund",
				Raknare = 0,
				Laste = false
			};
		}

		/*
		 * Testar att Index (huvudsidan) returnerar "Index" vyn.
		 * 
		 * AssertViewRendered kommer från MvcContrib.TestHelper
		 */
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

		/*
		 * Testar att Om-sidan returnerar default text om företaget/hemsidan
		 */
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

		/*
		 * Testar att Kontakt-sidan returnerar default text om företagkontakt
		 */
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

		/*
		 * Testar att Inloggning (inloggningssidan) returnerar "Inloggning"
		 * vyn vid [HttpGet].
		 * 
		 * AssertViewRendered kommer från MvcContrib.TestHelper
		 */
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

		/*
		 * Testar att Inlognning (inloggningssidan) gör en Redirect till
		 * Index i HemController vid lyckade inloggning vid [HttpPost].
		 * 
		 * *För att använda Inloggning som är async använder man .Result i
		 *		slutet och inte 'await'.
		 * 
		 * AssertViewRendered kommer från MvcContrib.TestHelper
		 */
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

		/*
		 * Testar att Utloggning (utloggningssidan) gör en Redirect till
		 * Index i HemController.
		 * 
		 * AssertActionRedirect kommer från MvcContrib.TestHelper
		 */
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

		/*
		 * Testar att Inlognning (inloggningssidan) gör en Redirect till
		 * Utlåste i HemController vid 5:e+ misslyckade inloggning.
		 * 
		 * *För att använda Inloggning som är async använder man .Result i
		 *		slutet och inte 'await'.
		 * 
		 * AssertViewRendered kommer från MvcContrib.TestHelper
		 */
		[TestMethod]
		public void TestHemInloggningMisslyckades5GångerSkickaTillUtlåsteView()
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
