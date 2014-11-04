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
		Produkter HämtaProduktMedID()
		{
			return HämtaProduktMedID("00000");
		}

		Produkter HämtaProduktMedID(string id)
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
			ProdukterController controller = GetProdukterController(new InMemoryProdukterRepository());

			// Act
			ViewResult result = controller.Index("Namn_Ordning", "", "", "", 1) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void ProdukterIndexHämtarVyn()
		{
			// Arrange
			ProdukterController controller = GetProdukterController(new InMemoryProdukterRepository());
			// Act
			ViewResult result = controller.Index("Namn_Ordning", "", "", "", 1) as ViewResult;
			// Assert
			Assert.AreEqual("Index", result.ViewName);
		}

		[TestMethod]
		public void ProdukterIndexHämtarAllaProdukterFrånRepository()
		{
			// Arrange
			Produkter produkt1 = HämtaProduktMedID("00000");
			Produkter produkt2 = HämtaProduktMedID("11111");
			InMemoryProdukterRepository repository = new InMemoryProdukterRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProdukterController controller = GetProdukterController(repository);

			// Act
			ViewResult result = controller.Index("Namn_Ordning", "", "", "", 1) as ViewResult;

			// Assert
			var model = (IEnumerable<Produkter>)result.ViewData.Model;
			CollectionAssert.Contains(model.ToList(), produkt1);
			CollectionAssert.Contains(model.ToList(), produkt2);
		}

		[TestMethod]
		public void ProdukterDetailsNotNull()
		{
			// Arrange
			Produkter produkt1 = HämtaProduktMedID("00000"); 
			InMemoryProdukterRepository repository = new InMemoryProdukterRepository();
			repository.Add(produkt1);
			ProdukterController controller = GetProdukterController(repository);

			// Act
			ViewResult result = controller.Details("00000") as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void ProdukterDetailsHämtarVyn()
		{
			// Arrange
			Produkter produkt1 = HämtaProduktMedID("00000");
			InMemoryProdukterRepository repository = new InMemoryProdukterRepository();
			repository.Add(produkt1);
			ProdukterController controller = GetProdukterController(repository);

			// Act
			ViewResult result = controller.Details("00000") as ViewResult;

			// Assert
			Assert.AreEqual("Details", result.ViewName);
		}

		[TestMethod]
		public void ProdukterDetailsHämtarProdukt00000()
		{
			// Arrange
			Produkter produkt1 = HämtaProduktMedID("00000");
			Produkter produkt2 = HämtaProduktMedID("11111");
			InMemoryProdukterRepository repository = new InMemoryProdukterRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProdukterController controller = GetProdukterController(repository);

			// Act
			ViewResult result = controller.Details("00000") as ViewResult;

			// Assert
			var model = result.ViewData.Model;
			Assert.AreEqual(model, produkt1);
			Assert.AreNotEqual(model, produkt2);
		}

		[TestMethod]
		public void ProdukterDetailsHämtarInteObefintligProdukt()
		{
			// Arrange
			Produkter produkt1 = HämtaProduktMedID("00000");
			InMemoryProdukterRepository repository = new InMemoryProdukterRepository();
			repository.Add(produkt1);
			ProdukterController controller = GetProdukterController(repository);

			// Act
			ViewResult result = controller.Details("33333") as ViewResult;

			// Assert
			Assert.IsNull(result);
			//Assert.IsNotNull(result);
			//Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
		}

		[TestMethod]
		public void ProdukterPrislistaNotNull()
		{
			// Arrange
			ProdukterController controller = GetProdukterController(new InMemoryProdukterRepository());

			// Act
			ViewResult result = controller.Prislista("Namn_Ordning", "", "", "", 1) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void ProdukterPrislistaHämtarVyn()
		{
			// Arrange
			ProdukterController controller = GetProdukterController(new InMemoryProdukterRepository());

			// Act
			ViewResult result = controller.Prislista("Namn_Ordning", "", "", "", 1) as ViewResult;

			// Assert
			Assert.AreEqual("Prislista", result.ViewName);
		}

		[TestMethod]
		public void ProdukterPrislistaHämtarAllaProdukterFrånRepository()
		{
			// Arrange
			Produkter produkt1 = HämtaProduktMedID("00000");
			Produkter produkt2 = HämtaProduktMedID("11111");
			InMemoryProdukterRepository repository = new InMemoryProdukterRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProdukterController controller = GetProdukterController(repository);

			// Act
			ViewResult result = controller.Prislista("Namn_Ordning", "", "", "", 1) as ViewResult;

			// Assert
			var model = (IEnumerable<Produkter>)result.ViewData.Model;
			CollectionAssert.Contains(model.ToList(), produkt1);
			CollectionAssert.Contains(model.ToList(), produkt2);
		}
	}
}
