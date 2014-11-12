using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LjusOchMiljoAB.Models;
using System.Web.Helpers;

namespace LjusOchMiljoAB.Controllers
{
	/* 
	 * AnvändareTjänst x
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 11
	 * Version: 0.18
	 */
	public class AnvändareTjänst : IAnvändareTjänst
	{
		//IAnvändareRepository hanterar kommunikation med databasen
		private readonly IAnvändareRepository repository;

		//Vid tom konstruktör, gör en ny repository av typen som används för
		//verklig körning
		public AnvändareTjänst() : this(new AnvändareRepository()) { }

		//En-parameter konstruktör för testning mot en egen repository
		public AnvändareTjänst(IAnvändareRepository repository)
		{
			this.repository = repository;
		}

		public Anvandare HämtaAnvändareMedNamn(string användarnamn)
		{
			return (repository.HämtaAnvändareMedNamn(användarnamn));
		}

		public bool BekräftaLösenord(Anvandare anvandare, string lösenord)
		{
			return Crypto.VerifyHashedPassword(anvandare.LosenordHash, lösenord);
		}

		//Bara för test
		public void SättLösenord(Anvandare anvandare, string lösenord)
		{
			anvandare.LosenordHash = Crypto.HashPassword(lösenord);
		}

		//Bara för test
		public void SkapaAnvändare(Anvandare användare)
		{
			repository.SkapaAnvändare(användare);
		}

		public void Förstör()
		{
			repository.Förstör();
		}
	}
}