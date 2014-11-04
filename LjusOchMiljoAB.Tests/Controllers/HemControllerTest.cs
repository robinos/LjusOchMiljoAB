using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LjusOchMiljoAB;
using LjusOchMiljoAB.Controllers;

namespace LjusOchMiljoAB.Tests.Controllers
{
	/*
	 * HemControllerTest testar controllern till huvudsidan Hem, med undersidor 
	 * Om och Kontakt.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 04
	 * Version: 0.16b
	 */
	[TestClass]
	public class HemControllerTest
	{
		/*
		 * Testar att Index (huvudsidan) är inte null
		 */
		[TestMethod]
		public void HemIndexNotNull()
		{
			// Arrange
			HemController controller = new HemController();

			// Act
			ViewResult result = controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		/*
		 * Testar att Om-sidan returnerar default text om företaget/hemsidan
		 */
		[TestMethod]
		public void HemOmHarDefaultText()
		{
			// Arrange
			HemController controller = new HemController();

			// Act
			ViewResult result = controller.Om() as ViewResult;

			// Assert
			Assert.AreEqual("Om hemsidan och Ljus och Miljö AB", result.ViewBag.Message);
		}

		/*
		 * Testar att Kontakt-sidan returnerar default text om företagkontakt
		 */
		[TestMethod]
		public void HemKontaktHarDefaultText()
		{
			// Arrange
			HemController controller = new HemController();

			// Act
			ViewResult result = controller.Kontakt() as ViewResult;

			// Assert
			Assert.AreEqual("Kontaktsidan", result.ViewBag.Message);
		}
	}
}
