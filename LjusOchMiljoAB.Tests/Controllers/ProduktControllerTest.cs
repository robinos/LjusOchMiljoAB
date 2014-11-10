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
	/*
	 * ProduktControllerTest testar controllern till produktlistorna.  Den
	 * använder en databas så lösas (mock) databaskontakt uppnås med hjälp av
	 * InMemoryProduktRepository som implementerar IProduktRespository.
	 * Även en inre klass MockHttpContext används för testerna.
	 * 
	 * Metoder/sidor som testas är Index (produktlistan), Prislistan, och
	 * Details (produktsidan). 
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 09
	 * Version: 0.17
	 */
	[TestClass]
	public class ProduktControllerTest
	{
		/*
		 * Default hämtning
		 */
		Produkt HämtaProduktMedID()
		{
			return HämtaProduktMedID("00000");
		}

		/*
		 * Default produkt
		 */
		Produkt HämtaProduktMedID(string id)
		{
			return new Produkt
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

		/*
		 * Skapa en ProduktService
		 */
		private static IProduktService GetProduktService(IProduktRepository repository)
		{
			ProduktService produktService = new ProduktService(repository);
			return produktService;
		}

		/*
		 * Skapa en ProdukterController och koppla det till MockHttpContext för tester
		 */
		private static ProduktController GetProduktController(IProduktService produktService)
		{
			ProduktController controller = new ProduktController(produktService);

			controller.ControllerContext = new ControllerContext()
			{
				Controller = controller,
				RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
			};
			return controller;
		}

		/*
		 * Inre klass som kopplas till en ProduktController för testning för
		 * lösas HTTP Context
		 */
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

		/*
		 * ProduktIndexNotNull testar att sidan är inte null.
		 */
		[TestMethod]
		public void ProduktIndexNotNull()
		{
			// Arrange
			ProduktController controller = GetProduktController(GetProduktService(new InMemoryProduktRepository()));

			// Act
			ViewResult result = controller.Index("Namn_Ordning", "", "", "", "", 1) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		/*
		 * ProdukterIndexHämtarVyn testar att sidan hämtar vyn mha text som
		 * skickas från ProdukterController för testning.
		 */
		[TestMethod]
		public void ProduktIndexHämtarVyn()
		{
			// Arrange
			ProduktController controller = GetProduktController(GetProduktService(new InMemoryProduktRepository()));
			// Act
			ViewResult result = controller.Index("Namn_Ordning", "", "", "", "", 1) as ViewResult;
			// Assert
			Assert.AreEqual("Index", result.ViewName);
		}

		/*
		 * ProdukterIndexHämtarAllaProdukterFrånRepository testar att vyn för
		 * alla produkter verkligen visas alla produkter som finns.
		 */
		[TestMethod]
		public void ProdukterIndexHämtarAllaProdukterFrånRepository()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			Produkt produkt2 = HämtaProduktMedID("11111");
			InMemoryProduktRepository repository = new InMemoryProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktController controller = GetProduktController(GetProduktService(repository));

			// Act
			ViewResult result = controller.Index("Namn_Ordning", "", "", "", "", 1) as ViewResult;

			// Assert
			var model = (IEnumerable<Produkt>)result.ViewData.Model;
			CollectionAssert.Contains(model.ToList(), produkt1);
			CollectionAssert.Contains(model.ToList(), produkt2);
		}

		/*
		 * ProdukterDetailsNotNull testar att sidan är inte null.
		 */
		[TestMethod]
		public void ProdukterDetailsNotNull()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000"); 
			InMemoryProduktRepository repository = new InMemoryProduktRepository();
			repository.Add(produkt1);
			ProduktController controller = GetProduktController(GetProduktService(repository));

			// Act
			ViewResult result = controller.Details("00000") as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		/*
		 * ProdukterDetailsHämtarVyn testar att sidan hämtar vyn mha text som
		 * skickas från ProdukterController för testning.
		 */
		[TestMethod]
		public void ProdukterDetailsHämtarVyn()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			InMemoryProduktRepository repository = new InMemoryProduktRepository();
			repository.Add(produkt1);
			ProduktController controller = GetProduktController(GetProduktService(repository));

			// Act
			ViewResult result = controller.Details("00000") as ViewResult;

			// Assert
			Assert.AreEqual("Details", result.ViewName);
		}

		/*
		 * ProdukterDetailsHämtarProdukt00000 testar att just produkter 00000
		 * hittas och visas och inte någon annan produkt.
		 */
		[TestMethod]
		public void ProdukterDetailsHämtarProdukt00000()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			Produkt produkt2 = HämtaProduktMedID("11111");
			InMemoryProduktRepository repository = new InMemoryProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktController controller = GetProduktController(GetProduktService(repository));

			// Act
			ViewResult result = controller.Details("00000") as ViewResult;

			// Assert
			var model = result.ViewData.Model;
			Assert.AreEqual(model, produkt1);
			Assert.AreNotEqual(model, produkt2);
		}

		/*
		 * ProdukterDetailsHämtarInteObefintligProdukt testar att details hämtar
		 * INTE en produkt som inte finns.
		 */
		[TestMethod]
		public void ProdukterDetailsHämtarInteObefintligProdukt()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			InMemoryProduktRepository repository = new InMemoryProduktRepository();
			repository.Add(produkt1);
			ProduktController controller = GetProduktController(GetProduktService(repository));

			// Act
			ViewResult result = controller.Details("33333") as ViewResult;

			// Assert
			Assert.IsNull(result);
			//Assert.IsNotNull(result);
			//Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
		}

		/*
		 * ProdukterPrislistaNotNull testar att sidan är inte null.
		 */
		[TestMethod]
		public void ProdukterPrislistaNotNull()
		{
			// Arrange
			ProduktController controller = GetProduktController(GetProduktService(new InMemoryProduktRepository()));

			// Act
			ViewResult result = controller.Prislista("Namn_Ordning", "", "", "", "", 1) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		/*
		 * ProdukterPrislistaHämtarVyn testar att sidan hämtar vyn mha text som
		 * skickas från ProdukterController för testning.
		 */
		[TestMethod]
		public void ProdukterPrislistaHämtarVyn()
		{
			// Arrange
			ProduktController controller = GetProduktController(GetProduktService(new InMemoryProduktRepository()));

			// Act
			ViewResult result = controller.Prislista("Namn_Ordning", "", "", "", "", 1) as ViewResult;

			// Assert
			Assert.AreEqual("Prislista", result.ViewName);
		}

		/*
		 * ProdukterPrislistaHämtarAllaProdukterFrånRepository testar att vyn för
		 * priser på alla produkter verkligen visas alla produkter som finns.
		 */
		[TestMethod]
		public void ProdukterPrislistaHämtarAllaProdukterFrånRepository()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			Produkt produkt2 = HämtaProduktMedID("11111");
			InMemoryProduktRepository repository = new InMemoryProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktController controller = GetProduktController(GetProduktService(repository));

			// Act
			ViewResult result = controller.Prislista("Namn_Ordning", "", "", "", "", 1) as ViewResult;

			// Assert
			var model = (IEnumerable<Produkt>)result.ViewData.Model;
			CollectionAssert.Contains(model.ToList(), produkt1);
			CollectionAssert.Contains(model.ToList(), produkt2);
		}
	}
}
