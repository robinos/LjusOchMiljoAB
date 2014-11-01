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
	[TestClass]
	public class HemControllerTest
	{
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
