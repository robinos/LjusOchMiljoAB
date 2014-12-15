using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Web.Mvc;
using LjusOchMiljoAB.Controllers;
using LjusOchMiljoAB.Models;
using LjusOchMiljoAB.Tests.Models;
using System.Web.Helpers;

namespace LjusOchMiljoAB.Tests.Controllers
{
	/// <summary>
	/// Testar AnvändareTjänst klassen som vid riktig körning har hand om
	/// kommunikation med en IAnvändareRepository för bekräfelse av lösenord vid
	/// inloggning. En mock repository IMinnetAnvändareRepository används (en
	/// översättning av InMemory inte som en interface).
	/// 
	/// Klassen testas av TestAnvändareTjänstNotNull.
	/// Metoden BekräftaLösenord testas av TestBekräftaGiltigAnvändarnamnOchLösenord,
	///		TestBekräftaOgiltigAnvändarnamn, TestBekräftaOgiltigLösenord och
	///		TestBekräftaOgiltigLösenord5GångerBlirLåste
	/// 
	/// Version: 1.0
	/// 2014-12-10
	/// Grupp 2
	/// </summary>
	[TestClass]
	public class AnvänderTjänstTest
	{
		/// <summary>
		/// HämtaAnvandare (utan parameter) skapar en default hämtning av en användare.
		/// </summary>
		Anvandare HämtaAnvandare()
		{
			return HämtaAnvandare(1, "kund", "password");
		}

		/// <summary>
		/// HämtaAnvandare skapar olika default användare för testning beroende på
		/// parameter.
		/// </summary>
		/// <param name="id">En integer som användare id</param>
		/// <param name="namn">En sträng som användarnamn</param>
		/// <param name="lösenord">En sträng som lösenord</param>
		Anvandare HämtaAnvandare(int id, string namn, string password)
		{
			return new Anvandare
			{
				ID = id,
				Anvandarnamn = namn,
				LosenordHash = Crypto.HashPassword(password),
				Roll = "kund",
				Raknare = 0,
				Last = false
			};
		}

		/// <summary>
		/// TestAnvändareTjänstNotNull testar bara att det går att skapa en
		/// AnvändareTjänst objekt med en mock repository.
		/// </summary>
		[TestMethod]
		public void TestAnvändareTjänstNotNull()
		{
			//Arrange
			IMinnetAnvändareRepository repository = new IMinnetAnvändareRepository();
			
			//Act
			AnvändareTjänst användareTjänst = new AnvändareTjänst(repository);

			//Assert
			Assert.IsNotNull(användareTjänst);
		}

		/// <summary>
		/// TestBekräftaGiltigAnvändarnamnOchLösenord testar fallet där användarnamn
		/// och lösenordet stämmer.  Man borde få tillbaka Status.Lyckades från enum
		/// Status.
		/// 
		/// För att använda BekräftaLösenord som är async använder man .Result i
		/// slutet och inte 'await'. 
		/// </summary>
		[TestMethod]
		public void TestBekräftaGiltigAnvändarnamnOchLösenord()
		{
			//Arrange
			IMinnetAnvändareRepository repository = new IMinnetAnvändareRepository();
			Anvandare användare1 = HämtaAnvandare(1, "kund", "password");
			repository.SkapaAnvändare(användare1);

			//Act
			AnvändareTjänst användareTjänst = new AnvändareTjänst(repository);
			Status status = användareTjänst.BekräftaLösenord("kund", "password").Result;

			//Assert
			Assert.AreEqual(status, Status.Lyckades);
		}

		/// <summary>
		/// TestBekräftaOgiltigAnvändarnamn testar fallet där användarnamnet
		/// finns inte.  Man borde få tillbaka Status.Misslyckades från enum
		/// Status.
		/// 
		/// För att använda BekräftaLösenord som är async använder man .Result i
		/// slutet och inte 'await'. 
		/// </summary>
		[TestMethod]
		public void TestBekräftaOgiltigAnvändarnamn()
		{
			//Arrange
			IMinnetAnvändareRepository repository = new IMinnetAnvändareRepository();
			Anvandare användare1 = HämtaAnvandare(1, "kund", "password");
			repository.SkapaAnvändare(användare1);

			//Act
			AnvändareTjänst användareTjänst = new AnvändareTjänst(repository);
			Status status = användareTjänst.BekräftaLösenord("admin", "password").Result;

			//Assert
			Assert.AreEqual(status, Status.Misslyckades);
		}

		/// <summary>
		/// TestBekräftaOgiltigLösenord testar fallet där lösenordet är inte
		/// korrekt (första gången).  Man borde få tillbaka Status.Misslyckades
		/// från enum Status.
		/// 
		/// För att använda BekräftaLösenord som är async använder man .Result i
		/// slutet och inte 'await'. 
		/// </summary>
		[TestMethod]
		public void TestBekräftaOgiltigLösenord()
		{
			//Arrange
			IMinnetAnvändareRepository repository = new IMinnetAnvändareRepository();
			Anvandare användare1 = HämtaAnvandare(1, "kund", "password");
			repository.SkapaAnvändare(användare1);

			//Act
			AnvändareTjänst användareTjänst = new AnvändareTjänst(repository);
			Status status = användareTjänst.BekräftaLösenord("kund", "lösenord").Result;
			//användare1 från repository
			Anvandare dbAnvändare1 = repository.HämtaAnvändareMedNamn("kund").Result;

			//Assert
			Assert.AreEqual(status, Status.Misslyckades);
			//Räknaren av misslyckade försök borde vara 1 nu
			Assert.AreEqual(dbAnvändare1.Raknare, 1);
		}

		/// <summary>
		/// TestBekräftaOgiltigLösenord5GångerBlirLåst testar fallet där lösenordet
		/// är inte korrekt (femte gången).  Man borde få tillbaka Status.Låst
		/// från enum Status.
		/// 
		/// För att använda BekräftaLösenord som är async använder man .Result i
		/// slutet och inte 'await'. 
		/// </summary>
		[TestMethod]
		public void TestBekräftaOgiltigLösenord5GångerBlirLåst()
		{
			//Arrange
			IMinnetAnvändareRepository repository = new IMinnetAnvändareRepository();
			Anvandare användare1 = HämtaAnvandare(1, "kund", "password");
			//Räknaren sätts till 4 misslyckade försök (på 5 blir det låst)
			användare1.Raknare = 4;
			repository.SkapaAnvändare(användare1);

			//Act
			AnvändareTjänst användareTjänst = new AnvändareTjänst(repository);
			Status status = användareTjänst.BekräftaLösenord("kund", "lösenord").Result;
			Anvandare dbAnvändare1 = repository.HämtaAnvändareMedNamn("kund").Result;

			//Assert
			Assert.AreEqual(dbAnvändare1.Raknare, 5);
			Assert.AreEqual(status, Status.Låst);
		}
	}
}
