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
	/// <summary>
	/// Testar ProduktTjänst klassen som hanterar att filtrerar, ordna och
	/// dela i sidor för ProduktController.
	/// 
	/// Metoden HämtaProduktMedID testas av TestProduktTjänstHämtaProduktSomExisterar
	///		och TestProduktTjänstHämtaInteProduktSomInteExisterar
	/// 
	/// Metoden HämtaValLista testas av TestProduktTjänstHämtaTomValLista,
	///		TestProduktTjänstHämtaValLista och
	///		TestProduktTjänstHämtaValListaValdInomhus
	/// 
	/// Metoden HämtaFiltreradProduktlista testas av
	///		TestProduktTjänstHämtaFiltreradProduktlistaMedTomProduktlista,
	///		TestProduktTjänstHämtaFiltreradProduktlistaMedIngenFiltrering och
	///		TestProduktTjänstHämtaFiltreradProduktlistaMedFiltrering00000
	/// 
	/// Metoden HämtaOrdnadProduktlista testas av
	///		TestProduktTjänstHämtaOrdnadProduktlistaMedTomProduktlista,
	///		TestProduktTjänstHämtaOrdnadProduktlistaMedDefaultOrdnad och
	///		TestProduktTjänstHämtaOrdnadProduktlistaMedAscPrisOrdnad
	/// 
	/// Metoden HämtaSida testas av TestProduktTjänstHämtaTomSida,
	///		TestProduktTjänstHämtaSida1 och
	///		TestProduktTjänstHämtaSida2
	/// 
	/// Version: 1.0
	/// 2014-12-14
	/// Grupp 2
	/// </summary>
	[TestClass]
	public class ProduktTjänstTest
	{
		/// <summary>
		/// HämtaProduktMedID (utan parameter) skapar en default hämtning av en produkt.
		/// </summary>
		Produkt HämtaProduktMedID()
		{
			return HämtaProduktMedID("0000000000");
		}

		/// <summary>
		/// HämtaProduktMedID skapar olika default produkter för testning beroende på
		/// id parametern.
		/// </summary>
		/// <param name="id">En sträng som produkt id</param>
		Produkt HämtaProduktMedID(string id)
		{
			return new Produkt
			{
				ID = id,
				Namn = id+"Lampa",
				Pris = 10.00m,
				Typ = "inomhus",
				Farg = "Blå",
				Bildfilnamn = id+"Lampa",
				Ritningsfilnamn = id+"Ritningen",
				RefID = id,
				Beskrivning = "En fantasktisk test lampa!  I alla storlekar.",
				Montering = "Använder skiftnyckel 5 och skruvmejsel C, annars följa diagram."
			};
		}

		/// <summary>
		/// TestProduktTjänstNotNull testar bara att det går att skapa en
		/// ProduktTjänst objekt med en mock repository.
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstNotNull()
		{
			//Arrange
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			
			//Act
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);

			//Assert
			Assert.IsNotNull(produktTjänst);
		}

		/// <summary>
		/// TestProduktTjänstHämtaProduktSomExisterar testar att man kan hämta en
		/// produkt med id 0000000000 som existerar.
		/// 
		/// *För att använda HämtaProduktMedID som är async använder man .Result i
		/// slutet och inte 'await'.
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaProduktSomExisterar()
		{
			// Arrange
			string id = "0000000000";
			Produkt produkt1 = HämtaProduktMedID(id);
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);

			//Act
			Produkt hämtadProdukt = produktTjänst.HämtaProduktMedID(id).Result;

			//Assert
			Assert.IsTrue(ÄrProdukterLika(produkt1, hämtadProdukt));
		}

		/// <summary>
		/// TestHämtaInteProduktSomInteExisterar testar att man kan inte hämta en
		/// produkt med id 0000000000 som inte existerar.
		/// 
		/// *För att använda HämtaProduktMedID som är async använder man .Result i
		/// slutet och inte 'await'.
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaInteProduktSomInteExisterar()
		{
			// Arrange
			string id = "0000000000";
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);

			//Act
			Produkt hämtadProdukt = produktTjänst.HämtaProduktMedID(id).Result;

			//Assert
			Assert.AreEqual(hämtadProdukt, null);
		}

		/// <summary>
		/// TestProduktTjänstHämtaTomValLista testar att hämta en tom SelectList från en
		/// tom produktlista. 
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaTomValLista()
		{
			//Arrange
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);
			SelectList selectLista = new SelectList(new List<string>(), "");
			IEnumerable<Produkt> produktEnumer = produktTjänst.HämtaProdukter().Result;

			//Act
			SelectList hämtadSelectLista = produktTjänst.HämtaValLista(produktEnumer, "");

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

		/// <summary>
		/// TestProduktTjänstHämtaValListaValdInomhus testar att hämta en
		/// SelectList av stränger med olika typer av produkter med ingenting vald.
		/// Detta är default-läge för DropDownList om man börjar på produktlistan. 
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaValLista()
		{
			//Arrange
			Produkt produkt1 = HämtaProduktMedID("0000000000");
			Produkt produkt2 = HämtaProduktMedID("1111111111");
			produkt2.Typ = "utomhus";
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);
			List<Produkt> produktLista = new List<Produkt>();
			produktLista.Add(produkt1);
			produktLista.Add(produkt2);
			List<string> strängLista = new List<string>();
			strängLista.Add(produkt1.Typ);
			strängLista.Add(produkt2.Typ);
			SelectList selectLista = new SelectList(strängLista, "");

			//Act
			SelectList hämtadSelectLista = produktTjänst.HämtaValLista(produktLista, "");

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

		/// <summary>
		/// TestProduktTjänstHämtaValListaValdInomhus testar att hämta en
		/// SelectList av stränger med olika typer av produkter med "inomhus" vald.
		/// Detta är default-läge för DropDownList om man börjar med Inomhus kategorin.
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaValListaValdInomhus()
		{
			//Arrange
			Produkt produkt1 = HämtaProduktMedID("0000000000");
			Produkt produkt2 = HämtaProduktMedID("1111111111");
			produkt2.Typ = "utomhus";
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);
			List<Produkt> produktLista = new List<Produkt>();
			produktLista.Add(produkt1);
			produktLista.Add(produkt2);
			SelectList selectLista = new SelectList(produktLista, produkt1.Typ);

			//Act
			SelectList hämtadSelectLista = produktTjänst.HämtaValLista(produktLista, "inomhus");

			string valdProdukt1 = (string)selectLista.SelectedValue;
			string valdProdukt2 = (string)hämtadSelectLista.SelectedValue;

			//Assert
			Assert.AreEqual(valdProdukt1, valdProdukt2);
		}

		/// <summary>
		/// TestProduktTjänstHämtaFiltreradProduktlistaMedTomProduktlista testar
		/// att hämta produktlistan utan några inställningar och med en tom
		/// produktlista.
		/// 
		/// *För att använda HämtaProdukter som är async använder man .Result i
		/// slutet och inte 'await'.
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaFiltreradProduktlistaMedTomProduktlista()
		{
			// Arrange
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);

			string produktTyp = "";
			string sökSträng = "";
			IEnumerable<Produkt> produktEnumer = produktTjänst.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);

			//Act
			List<Produkt> hämtadProdukter = (List<Produkt>)produktTjänst.HämtaFiltreradProduktlista(produkter, produktTyp, sökSträng);
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

		/// <summary>
		/// TestProduktTjänstHämtaFiltreradProduktlistaMedIngenFiltrering testar
		/// att hämta produktlistan utan några inställningar (som när man först
		/// börjar på sidan). IEnumerable man får tillbaka borde innehålla
		/// alla produkter i databasen.
		/// 
		/// *För att använda HämtaProdukter som är async använder man .Result i
		/// slutet och inte 'await'.
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaFiltreradProduktlistaMedIngenFiltrering()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("0000000000");
			Produkt produkt2 = HämtaProduktMedID("1111111111");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);

			string produktTyp = "";
			string sökSträng = "";
			IEnumerable<Produkt> produktEnumer = produktTjänst.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);

			//Act
			List<Produkt> hämtadProdukter = (List<Produkt>)produktTjänst.HämtaFiltreradProduktlista(produkter, produktTyp, sökSträng);
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

		/// <summary>
		/// TestProduktTjänstHämtaFiltreradProduktlistaMedFiltrering00000 testar
		/// att hämta produktlistan med bara namn som innehåller "00000" i namnet.
		/// IEnumerable man får tillbaka borde bara innehålla produkt1.
		/// 
		/// *För att använda HämtaProdukter som är async använder man .Result i
		/// slutet och inte 'await'. 
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaFiltreradProduktlistaMedFiltrering00000()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("0000000000");
			Produkt produkt2 = HämtaProduktMedID("1111111111");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);

			string produktTyp = "";
			string sökSträng = "00000";
			IEnumerable<Produkt> produktEnumer = produktTjänst.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);

			List<Produkt> baraProdukt1 = new List<Produkt>();
			baraProdukt1.Add(produkt1);

			//Act
			IEnumerable<Produkt> produktEnumerFilter = produktTjänst.HämtaFiltreradProduktlista(produkter, produktTyp, sökSträng);
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

		/// <summary>
		/// TestProduktTjänstHämtaOrdnadProduktlistaMedTomProduktlista testar
		/// att hämta en tom produktlistan.
		/// 
		/// *För att använda HämtaProdukter som är async använder man .Result i
		/// slutet och inte 'await'. 
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaOrdnadProduktlistaMedTomProduktlista()
		{
			// Arrange
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);
			IEnumerable<Produkt> produktEnumer = produktTjänst.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);
			List<Produkt> ordnadProdukter = new List<Produkt>();

			//Act
			List<Produkt> hämtadLista = new List<Produkt>(produktTjänst.HämtaOrdnadProduktlista(produkter, ""));
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

		/// <summary>
		/// TestProduktTjänstHämtaOrdnadProduktlistaMedDefaultOrdnad testar
		/// att hämta produktlistan med default ordningen som är A->Ö på Namn.
		/// IEnumerable man får tillbaka borde innehålla produkter i ovan
		/// ordning.
		/// 
		/// *För att använda HämtaProdukter som är async använder man .Result i
		/// slutet och inte 'await'. 
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaOrdnadProduktlistaMedDefaultOrdnad()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("1111111111");
			Produkt produkt2 = HämtaProduktMedID("0000000000");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);
			IEnumerable<Produkt> produktEnumer = produktTjänst.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);
			List<Produkt> ordnadProdukter = new List<Produkt>();
			ordnadProdukter.Add(produkt2);
			ordnadProdukter.Add(produkt1);

			//Act
			List<Produkt> hämtadLista = new List<Produkt>(produktTjänst.HämtaOrdnadProduktlista(produkter, ""));
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

		/// <summary>
		/// TestProduktTjänstHämtaOrdnadProduktlistaMedAscPrisOrdnad testar
		/// att hämta produktlistan med ökande ordningen på Pris.
		/// IEnumerable man får tillbaka borde innehålla produkter i ovan
		/// ordning.
		/// 
		/// *För att använda HämtaProdukter som är async använder man .Result i
		/// slutet och inte 'await'. 
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaOrdnadProduktlistaMedAscPrisOrdnad()
		{
			// Arrange
			Produkt produkt1 = HämtaProduktMedID("0000000000");
			produkt1.Pris = 200.00m;
			Produkt produkt2 = HämtaProduktMedID("1111111111");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);
			IEnumerable<Produkt> produktEnumer = produktTjänst.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);
			List<Produkt> ordnadProdukter = new List<Produkt>();
			ordnadProdukter.Add(produkt2);
			ordnadProdukter.Add(produkt1);

			//Act
			List<Produkt> hämtadLista = new List<Produkt>(produktTjänst.HämtaOrdnadProduktlista(produkter, "AscPris_Ordning"));
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

		/// <summary>
		/// TestProduktTjänstHämtaTomSida testar att hämta en tom sida.
		/// 
		/// *För att använda HämtaProdukter som är async använder man .Result i
		/// slutet och inte 'await'. 
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaTomSida()
		{
			//Arrange
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);
			IEnumerable<Produkt> produktEnumer = produktTjänst.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);

			//Act
			IPagedList pagedList = produktTjänst.HämtaSida(produkter, 1);

			//Assert

			//Sidonumret är 1
			Assert.IsTrue(pagedList.PageNumber == 1);
			//Totalla produkter äver både alla sidor är samma som hela listan
			Assert.IsTrue(produkter.Count == pagedList.PageCount);
			//Där finns 0 sidor totallt
			Assert.IsTrue(0 == pagedList.PageCount);
		}

		/// <summary>
		/// TestProduktTjänstHämtaSida1 testar att hämta en IPagedList på första
		/// sidan över hela produktlistan (av ett produkt).  Alla produkter i fallet
		/// får plats på en sida.
		/// Tyvärr har jag inte hittat ett sätt att testa hur många produkter som
		/// finns på sidan eller få en lista bara för sidan.
		/// 
		/// *För att använda HämtaProdukter som är async använder man .Result i
		/// slutet och inte 'await'. 
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaSida1()
		{
			//Arrange
			Produkt produkt1 = HämtaProduktMedID("0000000000");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);
			IEnumerable<Produkt> produktEnumer = produktTjänst.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);

			//Act
			IPagedList pagedList = produktTjänst.HämtaSida(produkter, 1);

			//Assert

			//Sidonumret är 1
			Assert.IsTrue(pagedList.PageNumber == 1);
			//Totalla produkter äver både alla sidor är samma som hela listan
			Assert.IsTrue(produkter.Count == pagedList.PageCount);
			//Där finns 1 sida totallt
			Assert.IsTrue(1 == pagedList.PageCount);
		}

		/// <summary>
		/// TestProduktTjänstHämtaSida2 testar att hämta en IPagedList på andra
		/// sidan, att alla produkter är lagrad över alla sidor och att där finns
		/// två sidor totallt.
		/// 
		/// Med 6 produkter och en gräns på 5 produkter per sida borde man
		/// få 1 produkt på andrasidan.
		/// Tyvärr har jag inte hittat ett sätt att testa hur många produkter som
		/// finns på sidan eller få en lista bara för sidan.
		/// 
		/// *För att använda HämtaProdukter som är async använder man .Result i
		/// slutet och inte 'await'. 
		/// </summary>
		[TestMethod]
		public void TestProduktTjänstHämtaSida2()
		{
			//Arrange
			Produkt produkt1 = HämtaProduktMedID("0000000001");
			Produkt produkt2 = HämtaProduktMedID("0000000002");
			Produkt produkt3 = HämtaProduktMedID("0000000003");
			Produkt produkt4 = HämtaProduktMedID("0000000004");
			Produkt produkt5 = HämtaProduktMedID("0000000005");
			Produkt produkt6 = HämtaProduktMedID("0000000006");
			IMinnetProduktRepository repository = new IMinnetProduktRepository();
			repository.Add(produkt1);
			repository.Add(produkt2);
			repository.Add(produkt3);
			repository.Add(produkt4);
			repository.Add(produkt5);
			repository.Add(produkt6);
			ProduktTjänst produktTjänst = new ProduktTjänst(repository);
			IEnumerable<Produkt> produktEnumer = produktTjänst.HämtaProdukter().Result;
			List<Produkt> produkter = new List<Produkt>(produktEnumer);

			//Act
			IPagedList pagedList = produktTjänst.HämtaSida(produkter, 2);

			//Assert
			
			//Sidonumret är 2
			Assert.IsTrue(pagedList.PageNumber == 2);
			//Totalla produkter äver både alla sidor är samma som hela listan
			Assert.IsTrue(produkter.Count == pagedList.TotalItemCount);
			//Där finns 2 sidor totallt
			Assert.IsTrue(2 == pagedList.PageCount);
		}

		/// <summary>
		/// ÄrProdukterLika är inte en test.  Det är en hjälpmetod för att jämföra
		/// produkter då Produkt klassen är autogenererad och man vill helst inte
		/// ändra den för att skriva en Equals metod.
		/// </summary>
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
