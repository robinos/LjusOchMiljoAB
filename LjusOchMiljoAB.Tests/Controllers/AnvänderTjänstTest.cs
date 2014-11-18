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
	/*
	 * Testar AnvändareTjänst klassen som har hand om kommunikation med
	 * AnvändareRepository för bekräfelse av lösenord vid inloggning.
	 * 
	 * Metoden BekräftaLösenord testas av TestBekräftaGiltigAnvändarnamnOchLösenord,
	 *	TestBekräftaOgiltigAnvändarnamn, TestBekräftaOgiltigLösenord och
	 *	TestBekräftaOgiltigLösenord5GångerBlirLåste
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 18
	 * Version: 0.18
	 */
	[TestClass]
	public class AnvänderTjänstTest
	{
		/*
		 * Default hämtning
		 */
		Anvandare HämtaAnvandareMedNamn()
		{
			return HämtaAnvandareMedNamn(1, "kund", "password");
		}

		/*
		 * Default användare att skapa nya med för testning
		 */
		Anvandare HämtaAnvandareMedNamn(int id, string namn, string password)
		{
			return new Anvandare
			{
				ID = id,
				Anvandarnamn = namn,
				LosenordHash = Crypto.HashPassword(password),
				Roll = "kund",
				Raknare = 0,
				Laste = false
			};
		}

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

		/*
		 * TestBekräftaGiltigAnvändarnamnOchLösenord testar fallet där användarnamn
		 * och lösenordet stämmer.  Man borde få tillbaka Status.Lyckades från enum
		 * Status.
		 * *För att använda BekräftaLösenord som är async använder man .Result i
		 * slutet och inte 'await'. 
		 */
		[TestMethod]
		public void TestBekräftaGiltigAnvändarnamnOchLösenord()
		{
			//Arrange
			IMinnetAnvändareRepository repository = new IMinnetAnvändareRepository();
			Anvandare användare1 = HämtaAnvandareMedNamn(1, "kund", "password");
			repository.Add(användare1);

			//Act
			AnvändareTjänst användareTjänst = new AnvändareTjänst(repository);
			Status status = användareTjänst.BekräftaLösenord("kund", "password").Result;

			//Assert
			Assert.AreEqual(status, Status.Lyckades);
		}

		/*
		 * TestBekräftaOgiltigAnvändarnamn testar fallet där användarnamnet
		 * finns inte.  Man borde få tillbaka Status.Misslyckades från enum
		 * Status.
		 * *För att använda BekräftaLösenord som är async använder man .Result i
		 * slutet och inte 'await'. 
		 */
		[TestMethod]
		public void TestBekräftaOgiltigAnvändarnamn()
		{
			//Arrange
			IMinnetAnvändareRepository repository = new IMinnetAnvändareRepository();
			Anvandare användare1 = HämtaAnvandareMedNamn(1, "kund", "password");
			repository.Add(användare1);

			//Act
			AnvändareTjänst användareTjänst = new AnvändareTjänst(repository);
			Status status = användareTjänst.BekräftaLösenord("admin", "password").Result;

			//Assert
			Assert.AreEqual(status, Status.Misslyckades);
		}

		/*
		 * TestBekräftaOgiltigLösenord testar fallet där lösenordet är inte
		 * korrekt (första gången).  Man borde få tillbaka Status.Misslyckades
		 * från enum Status.
		 * *För att använda BekräftaLösenord och HämtaAnvändareMedNamn som är
		 *	async använder man .Result i slutet och inte 'await'. 
		 */
		[TestMethod]
		public void TestBekräftaOgiltigLösenord()
		{
			//Arrange
			IMinnetAnvändareRepository repository = new IMinnetAnvändareRepository();
			Anvandare användare1 = HämtaAnvandareMedNamn(1, "kund", "password");
			repository.Add(användare1);

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

		/*
		 * TestBekräftaOgiltigLösenord testar fallet där lösenordet är inte
		 * korrekt (femte gången).  Man borde få tillbaka Status.Låste
		 * från enum Status.
		 * *För att använda BekräftaLösenord som är async använder man .Result i
		 * slutet och inte 'await'. 
		 */
		[TestMethod]
		public void TestBekräftaOgiltigLösenord5GångerBlirLåste()
		{
			//Arrange
			IMinnetAnvändareRepository repository = new IMinnetAnvändareRepository();
			Anvandare användare1 = HämtaAnvandareMedNamn(1, "kund", "password");
			//Räknaren sätts till 4 misslyckade försök (på 5 blir det låste)
			användare1.Raknare = 4;
			repository.Add(användare1);

			//Act
			AnvändareTjänst användareTjänst = new AnvändareTjänst(repository);
			Status status = användareTjänst.BekräftaLösenord("kund", "lösenord").Result;

			//Assert
			Assert.AreEqual(status, Status.Låste);
		}
	}
}
