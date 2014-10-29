﻿using System;
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
		public void Index()
		{
			// Arrange
			HemController controller = new HemController();

			// Act
			ViewResult result = controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void About()
		{
			// Arrange
			HemController controller = new HemController();

			// Act
			ViewResult result = controller.About() as ViewResult;

			// Assert
			Assert.AreEqual("Your application description page.", result.ViewBag.Message);
		}

		[TestMethod]
		public void Contact()
		{
			// Arrange
			HemController controller = new HemController();

			// Act
			ViewResult result = controller.Contact() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}
