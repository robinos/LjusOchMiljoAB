using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Web.Mvc;
using LjusOchMiljoAB.Controllers;
using LjusOchMiljoAB.Models;
using LjusOchMiljoAB.Tests.Models;
using PagedList.Mvc;
using PagedList;

namespace LjusOchMiljoAB.Tests.Controllers
{
	[TestClass]
	public class ProduktServiceTest
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

		[TestMethod]
		public void TestProduktServiceNotNull()
		{
			//Arrange
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			
			//Act
			ProduktTjänst produktService = new ProduktTjänst(repository);

			//Assert
			Assert.IsNotNull(produktService);
		}

		[TestMethod]
		public void TestProduktServiceHämtaValLista()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			Produkt produkt2 = HämtaProduktMedID("11111");
			produkt2.Typ = "utomhus";
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktTjänst produktService = new ProduktTjänst(repository);
			List<Produkt> produktLista = new List<Produkt>();
			produktLista.Add(produkt1);
			produktLista.Add(produkt2);
			List<string> strängLista = new List<string>();
			strängLista.Add(produkt1.Typ);
			strängLista.Add(produkt2.Typ);
			SelectList selectLista = new SelectList(strängLista, produkt1.Typ);

			//Act
			SelectList hämtadSelectLista = produktService.HämtaValLista(produktLista, "inomhus");

			List<string> list1 = new List<string>((IEnumerable<string>)selectLista.Items);
			List<string> list2 = new List<string>((IEnumerable<string>)hämtadSelectLista.Items);

			//Assert
			CollectionAssert.Equals(list1, list2);

			if (list1.Count == list2.Count)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					Assert.AreEqual(list1[i], list2[i]);
				}
			}
		}

		[TestMethod]
		public void TestProduktServiceHämtaValListaValdInomhus()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			Produkt produkt2 = HämtaProduktMedID("11111");
			produkt2.Typ = "utomhus";
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktTjänst produktService = new ProduktTjänst(repository);
			List<Produkt> produktLista = new List<Produkt>();
			produktLista.Add(produkt1);
			produktLista.Add(produkt2);
			SelectList selectLista = new SelectList(produktLista, produkt1.Typ);

			//Act
			SelectList hämtadSelectLista = produktService.HämtaValLista(produktLista, "inomhus");

			string valdProdukt1 = (string)selectLista.SelectedValue;
			string valdProdukt2 = (string)hämtadSelectLista.SelectedValue;

			//Assert
			Assert.AreEqual(valdProdukt1, valdProdukt2);
		}

		[TestMethod]
		public void TestProduktServiceHämtaFiltreradProduktlistaMedIngenFiltrering()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			Produkt produkt2 = HämtaProduktMedID("11111");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktTjänst produktService = new ProduktTjänst(repository);

			string produktTyp = "";
			string sökSträng = "";
			IEnumerable<Produkt> produktEnumer = produktService.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);

			//Act
			List<Produkt> hämtadProdukter = (List<Produkt>)produktService.HämtaFiltreradProduktlista(produkter, produktTyp, sökSträng);
			bool sammaStorlek = (produkter.Count == hämtadProdukter.Count);

			//Assert
			Assert.IsTrue(sammaStorlek);

			if (sammaStorlek)
			{
				for (int i = 0; i < hämtadProdukter.Count; i++)
				{
					Assert.IsTrue(ÄrProdukterLika(produkter[i], hämtadProdukter[i]));
				}
			}
		}

		[TestMethod]
		public void TestProduktServiceHämtaFiltreradProduktlistaMedFiltrering00000()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			Produkt produkt2 = HämtaProduktMedID("11111");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktTjänst produktService = new ProduktTjänst(repository);

			string produktTyp = "";
			string sökSträng = "00000";
			IEnumerable<Produkt> produktEnumer = produktService.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);

			List<Produkt> baraProdukt1 = new List<Produkt>();
			baraProdukt1.Add(produkt1);

			//Act
			IEnumerable<Produkt> produktEnumerFilter = produktService.HämtaFiltreradProduktlista(produkter, produktTyp, sökSträng);
			List<Produkt> hämtadProdukter = new List<Produkt>(produktEnumerFilter);
			bool sammaStorlek = (baraProdukt1.Count == hämtadProdukter.Count);

			//Assert
			Assert.IsTrue(sammaStorlek);

			if (sammaStorlek)
			{
				for (int i = 0; i < hämtadProdukter.Count; i++)
				{
					Assert.IsTrue(ÄrProdukterLika(baraProdukt1[i], hämtadProdukter[i]));
				}
			}
		}

		[TestMethod]
		public void TestProduktServiceHämtaOrdnadProduktlistaMedDefaultOrdnad()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("11111");
			Produkt produkt2 = HämtaProduktMedID("00000");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktTjänst produktService = new ProduktTjänst(repository);
			IEnumerable<Produkt> produktEnumer = produktService.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);
			List<Produkt> ordnadProdukter = new List<Produkt>();
			ordnadProdukter.Add(produkt2);
			ordnadProdukter.Add(produkt1);

			//Act
			List<Produkt> hämtadLista = new List<Produkt>(produktService.HämtaOrdnadProduktlista(produkter, ""));
			bool sammaStorlek = (ordnadProdukter.Count == hämtadLista.Count);

			//Assert
			Assert.IsTrue(sammaStorlek);

			if (sammaStorlek)
			{
				for (int i = 0; i < hämtadLista.Count; i++)
				{
					Assert.IsTrue(ÄrProdukterLika(ordnadProdukter[i], hämtadLista[i]));
				}
			}
		}

		[TestMethod]
		public void TestProduktServiceHämtaOrdnadProduktlistaMedAscPrisOrdnad()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			produkt1.Pris = 200.00m;
			Produkt produkt2 = HämtaProduktMedID("11111");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktTjänst produktService = new ProduktTjänst(repository);
			IEnumerable<Produkt> produktEnumer = produktService.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);
			List<Produkt> ordnadProdukter = new List<Produkt>();
			ordnadProdukter.Add(produkt2);
			ordnadProdukter.Add(produkt1);

			//Act
			List<Produkt> hämtadLista = new List<Produkt>(produktService.HämtaOrdnadProduktlista(produkter, "AscPris_Ordning"));
			bool sammaStorlek = (ordnadProdukter.Count == hämtadLista.Count);

			//Assert
			Assert.IsTrue(sammaStorlek);

			if (sammaStorlek)
			{
				for (int i = 0; i < hämtadLista.Count; i++)
				{
					Assert.IsTrue(ÄrProdukterLika(ordnadProdukter[i], hämtadLista[i]));
				}
			}
		}

		[TestMethod]
		public void TestProduktServiceHämtaSida1()
		{
			//Arrange
			Produkt produkt1 = HämtaProduktMedID("00000");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			ProduktTjänst produktService = new ProduktTjänst(repository);
			IEnumerable<Produkt> produktEnumer = produktService.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);

			//Act
			IPagedList pageList = produktService.HämtaSida(produkter, 1);

			//Assert
			Assert.IsTrue(pageList.PageNumber == 1);
			Assert.IsTrue(produkter.Count == pageList.PageCount);
		}

		[TestMethod]
		public void TestProduktServiceHämtaSida2()
		{
			//Arrange
			Produkt produkt1 = HämtaProduktMedID("00001");
			Produkt produkt2 = HämtaProduktMedID("00002");
			Produkt produkt3 = HämtaProduktMedID("00003");
			Produkt produkt4 = HämtaProduktMedID("00004");
			Produkt produkt5 = HämtaProduktMedID("00005");
			Produkt produkt6 = HämtaProduktMedID("00006");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			repository.Add(produkt3);
			repository.Add(produkt4);
			repository.Add(produkt5);
			repository.Add(produkt6);
			ProduktTjänst produktService = new ProduktTjänst(repository);
			IEnumerable<Produkt> produktEnumer = produktService.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);

			//Act
			IPagedList pageList = produktService.HämtaSida(produkter, 2);

			//Assert
			Assert.IsTrue(pageList.PageNumber == 2);
			Assert.IsTrue(produkter.Count == pageList.TotalItemCount);
			Assert.IsTrue(2 == pageList.PageCount);
		}

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
