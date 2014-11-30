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
				Namn = id+"Lampa",
				Pris = 10.00m,
				Typ = "inomhus",
				Farg = "Blå",
				Bildfilnamn = "00000Lampa",
				Ritningsfilnamn = "00000Ritningen",
				RefID = id,
				Beskrivning = "En fantasktisk test lampa!  I alla storlekar.",
				Montering = "Använder skiftnyckel 5 och skruvmejsel C, annars följa diagram."			
			};
		}

		/*
		 * Skapa en ProduktTjänst
		 */
		private static IProduktTjänst GetProduktTjänst(IProduktRepository repository)
		{
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);
			return produktTjänst;
		}

		/*
		 * Skapa en ProduktController och koppla det till MockHttpContext för tester
		 */
		private static ProduktController GetProduktController(IProduktTjänst produktTjänst)
		{
			ProduktController controller = new ProduktController(produktTjänst);

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
		 * Testar produktlistan med default ordning (namn A->Ö) och ingen filtrering.
		 */
		[TestMethod]
		public void TestProduktControllerHanteraListanDefault()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			Produkt produkt2 = HämtaProduktMedID("11111");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			List<Produkt> produkter = new List<Produkt>();
			produkter.Add(produkt1);
			produkter.Add(produkt2);
			ProduktController controller = GetProduktController(GetProduktTjänst(repository));

			string Ordning = "";
			string produktTyp = "";
			string sökSträng = "";
			string filterSträng = "";
			string filterProdukt = "";
			int sida = 1;

			//Act
			//produktEnumer är det som kommer tillbaka från HanteraListan metoden
			//i ProduktController
			IEnumerable<Produkt> produktEnumer = controller.HanteraListan(Ordning, produktTyp, sökSträng, filterSträng, filterProdukt, sida).Result;
			//En list kan ta en IEnumerable som konstruktör
			List<Produkt> visadeProduktList = new List<Produkt>(produktEnumer);

			bool sammaStorlek = (produkter.Count == visadeProduktList.Count);

			//Assert
			Assert.IsTrue(sammaStorlek);

			if (sammaStorlek)
			{
				//Testa att lika produkter finns i samma platser i listan
				for (int i = 0; i < visadeProduktList.Count; i++)
				{
					Assert.IsTrue(ÄrProdukterLika(produkter[i], visadeProduktList[i]));
				}
			}
		}

		/*
		 * Testar produktlistan med default ordning (namn A->Ö) och filtrering på typ
		 * "inomhus". Där finns fler än 5 inomhus produkter så man kan sätta det på
		 * att visa sida 2.
		 */
		[TestMethod]
		public void TestProduktControllerHanteraListanMedOrdningFilterSida()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("11111");
			Produkt produkt2 = HämtaProduktMedID("10000");
			Produkt produkt3 = HämtaProduktMedID("22222");
			produkt3.Typ = "utomhus";
			Produkt produkt4 = HämtaProduktMedID("13333");
			Produkt produkt5 = HämtaProduktMedID("14444");
			Produkt produkt6 = HämtaProduktMedID("15555");
			Produkt produkt7 = HämtaProduktMedID("16666");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			repository.Add(produkt3);
			repository.Add(produkt4);
			repository.Add(produkt5);
			repository.Add(produkt6);
			repository.Add(produkt7);
			//Produkt2 innan 1 är 'rätt' ordning för listan man borde få ut som
			//blir listan produkter (för jämförelse)
			List<Produkt> produkter = new List<Produkt>();
			produkter.Add(produkt2);
			produkter.Add(produkt1);
			produkter.Add(produkt4);
			produkter.Add(produkt5);
			produkter.Add(produkt6);
			produkter.Add(produkt7);
			ProduktController controller = GetProduktController(GetProduktTjänst(repository));

			string Ordning = "";
			string produktTyp = "inomhus";
			string sökSträng = "";
			string filterSträng = "";
			string filterProdukt = "";
			int sida = 2;

			//Act
			//produktEnumer är det som kommer tillbaka från HanteraListan metoden
			//i ProduktController
			IEnumerable<Produkt> produktEnumer = controller.HanteraListan(Ordning, produktTyp, sökSträng, filterSträng, filterProdukt, sida).Result;
			//En list kan ta en IEnumerable som konstruktör			
			List<Produkt> visadeProduktList = new List<Produkt>(produktEnumer);

			bool sammaStorlek = (produkter.Count == visadeProduktList.Count);

			//Assert
			Assert.AreEqual(produkter.Count, visadeProduktList.Count);

			if (sammaStorlek)
			{
				//Testa att lika produkter finns i samma platser i listan
				for (int i = 0; i < visadeProduktList.Count; i++)
				{
					Assert.IsTrue(ÄrProdukterLika(produkter[i], visadeProduktList[i]));
				}
			}
		}

		/*
		 * ProduktIndexNotNull testar att sidan är inte null.
		 */
		[TestMethod]
		public void TestProduktControllerIndexNotNull()
		{
			// Arrange
			ProduktController controller = GetProduktController(GetProduktTjänst(new IMinnetProduktRepository()));

			// Act
			ViewResult result = controller.Index("Namn_Ordning", "", "", "", "", 1).Result as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		/*
		 * ProduktIndexHämtarVyn testar att sidan hämtar vyn mha text som
		 * skickas från ProduktController för testning.
		 */
		[TestMethod]
		public void TestProduktControllerIndexHämtarVyn()
		{
			// Arrange
			ProduktController controller = GetProduktController(GetProduktTjänst(new IMinnetProduktRepository()));
			// Act
			ViewResult result = controller.Index("Namn_Ordning", "", "", "", "", 1).Result as ViewResult;
			// Assert
			Assert.AreEqual("Index", result.ViewName);
		}

		/*
		 * TestProduktControllerIndexHämtarAllaProdukterFrånRepository testar att
		 * vyn för alla produkter verkligen visar alla produkter som finns i databasen.
		 */
		[TestMethod]
		public void TestProduktControllerIndexHämtarAllaProdukterFrånRepository()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			Produkt produkt2 = HämtaProduktMedID("11111");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktController controller = GetProduktController(GetProduktTjänst(repository));

			// Act
			ViewResult result = controller.Index("Namn_Ordning", "", "", "", "", 1).Result as ViewResult;

			// Assert
			var model = (IEnumerable<Produkt>)result.ViewData.Model;
			CollectionAssert.Contains(model.ToList(), produkt1);
			CollectionAssert.Contains(model.ToList(), produkt2);
		}

		/*
		 * TestProduktControllerDetailsNotNull testar att sidan är inte null.
		 */
		[TestMethod]
		public void TestProduktControllerDetailsNotNull()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000"); 
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			ProduktController controller = GetProduktController(GetProduktTjänst(repository));

			// Act
			ViewResult result = controller.Detaljer("00000").Result as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		/*
		 * TestProduktControllerDetailsHämtarVyn testar att sidan hämtar vyn mha text
		 * som skickas från ProdukterController för testning.
		 */
		[TestMethod]
		public void TestProduktControllerDetailsHämtarVyn()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			ProduktController controller = GetProduktController(GetProduktTjänst(repository));

			// Act
			ViewResult result = controller.Detaljer("00000").Result as ViewResult;

			// Assert
			Assert.AreEqual("Detaljer", result.ViewName);
		}

		/*
		 * ProduktDetailsHämtarProdukt00000 testar att just produkter 00000
		 * hittas och visas och inte någon annan produkt.
		 */
		[TestMethod]
		public void TestProduktControllerDetailsHämtarProdukt00000()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			Produkt produkt2 = HämtaProduktMedID("11111");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktController controller = GetProduktController(GetProduktTjänst(repository));

			// Act
			ViewResult result = controller.Detaljer("00000").Result as ViewResult;

			// Assert
			var model = result.ViewData.Model;
			Assert.AreEqual(model, produkt1);
			Assert.AreNotEqual(model, produkt2);
		}

		/*
		 * TestProduktControllerDetailsHämtarInteObefintligProdukt testar att details
		 * hämtar INTE en produkt som inte finns.
		 * Error result blev inte väntad...behövs mer testning.
		 */
		[TestMethod]
		public void TestProduktControllerDetailsHämtarInteObefintligProdukt()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			ProduktController controller = GetProduktController(GetProduktTjänst(repository));

			// Act
			ViewResult result = controller.Detaljer("33333").Result as ViewResult;

			// Assert
			Assert.IsNull(result);
			//Assert.IsNotNull(result);
			//Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
		}

		/*
		 * TestProduktControllerPrislistaNotNull testar att sidan är inte null.
		 */
		[TestMethod]
		public void TestProduktControllerPrislistaNotNull()
		{
			// Arrange
			ProduktController controller = GetProduktController(GetProduktTjänst(new IMinnetProduktRepository()));

			// Act
			ViewResult result = controller.Prislista("Namn_Ordning", "", "", "", "", 1).Result as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		/*
		 * TestProduktControllerPrislistaHämtarVyn testar att sidan hämtar vyn mha text
		 * som skickas från ProdukterController för testning.
		 */
		[TestMethod]
		public void TestProduktControllerPrislistaHämtarVyn()
		{
			// Arrange
			ProduktController controller = GetProduktController(GetProduktTjänst(new IMinnetProduktRepository()));

			// Act
			ViewResult result = controller.Prislista("Namn_Ordning", "", "", "", "", 1).Result as ViewResult;

			// Assert
			Assert.AreEqual("Prislista", result.ViewName);
		}

		/*
		 * TestProduktControllerPrislistaHämtarAllaProdukterFrånRepository testar att
		 * vyn för priser på alla produkter verkligen visas alla produkter som finns.
		 */
		[TestMethod]
		public void TestProduktControllerPrislistaHämtarAllaProdukterFrånRepository()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			Produkt produkt2 = HämtaProduktMedID("11111");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktController controller = GetProduktController(GetProduktTjänst(repository));

			// Act
			ViewResult result = controller.Prislista("Namn_Ordning", "", "", "", "", 1).Result as ViewResult;

			// Assert
			var model = (IEnumerable<Produkt>)result.ViewData.Model;
			CollectionAssert.Contains(model.ToList(), produkt1);
			CollectionAssert.Contains(model.ToList(), produkt2);
		}

		/*
		 * ÄrProdukterLika är inte en test.  Det är en hjälpmetod för att jämföra
		 * produkter då Produkt klassen är autogenererad och man vill helst inte
		 * ändra den för att skriva en Equals metod.
		 */
		private bool ÄrProdukterLika(Produkt produkt1, Produkt produkt2)
		{
			bool result = false;

			if (produkt1.ID.Equals(produkt2.ID) && produkt1.Namn.Equals(produkt2.Namn) &&
				produkt1.Pris == produkt2.Pris && produkt1.Typ.Equals(produkt2.Typ) &&
				produkt1.Farg.Equals(produkt2.Farg) && produkt1.Bildfilnamn.Equals(produkt2.Bildfilnamn) &&
				produkt1.Ritningsfilnamn.Equals(produkt2.Ritningsfilnamn) &&
				produkt1.RefID.Equals(produkt2.RefID) &&
				produkt1.Beskrivning.Equals(produkt2.Beskrivning) &&
				produkt1.Montering.Equals(produkt2.Montering)) result = true;

			return result;
		}

	}
}
