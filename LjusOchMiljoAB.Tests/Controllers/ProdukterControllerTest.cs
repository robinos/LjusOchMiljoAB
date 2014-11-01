using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LjusOchMiljoAB;
using LjusOchMiljoAB.Controllers;
using LjusOchMiljoAB.Models;
using LjusOchMiljoAB.Tests.Models;

namespace LjusOchMiljoAB.Tests.Controllers
{
	[TestClass]
	public class ProdukterControllerTest
	{
		Produkter HämtaProdukt()
		{
			return HämtaProdukt("00000");
		}

		Produkter HämtaProdukt(string id)
		{
			return new Produkter
			{
				ID = id,
				Namn = "00000Lampa",
				Pris = 10.00m,
				Typ = "Inomhus",
				Farg = "Blå",
				Bildfilnamn = "00000Lampa",
				Ritningsfilnamn = "00000Ritningen",
				RefID = id,
				Beskrivning = "En fantasktisk test lampa!  I alla storlekar.",
				Montering = "Använder skiftnyckel 5 och skruvmejsel C, annars följa diagram."			
			};
		}

		private static ProdukterController GetProdukterController(IProdukterRepository repository)
		{
			ProdukterController controller = new ProdukterController(repository);

			controller.ControllerContext = new ControllerContext()
			{
				Controller = controller,
				RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
			};
			return controller;
		}


		private class MockHttpContext : HttpContextBase
		{
			private readonly IPrincipal _user = new GenericPrincipal(
					 new GenericIdentity("someUser"), null /* roles */);

			public override IPrincipal User
			{
				get
				{
					return _user;
				}
				set
				{
					base.User = value;
				}
			}
		}

		[TestMethod]
		public void ProdukterIndexNotNull()
		{
			// Arrange
			ProdukterController controller = new ProdukterController(new InMemoryProdukterRepository());

			// Act
			ViewResult result = controller.Index("", "") as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

	}
}
