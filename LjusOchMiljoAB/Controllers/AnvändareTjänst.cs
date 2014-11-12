using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LjusOchMiljoAB.Models;
using System.Web.Helpers;
using Microsoft.Security.Application;

namespace LjusOchMiljoAB.Controllers
{
	/* 
	 * AnvändareTjänst är tjänsten som hantera kontakt med AnvändareRepository
	 * för att autentisera användare.
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

		/*
		 * HämtaAnvändareMedNamn hämtar en Anvandare objekt (om den finns) som
		 * har samma användarnamn.
		 */
		public Anvandare HämtaAnvändareMedNamn(string användarnamn)
		{
			return (repository.HämtaAnvändareMedNamn(användarnamn));
		}

		/*
		 * BekräftaLösenord jämför angiven lösenordens hash med den från
		 * databasen.
		 */
		public Status BekräftaLösenord(Anvandare anvandare, string lösenord)
		{
			if(anvandare.Raknare != null && anvandare.Raknare > 4)
			{
				anvandare.Laste = true;

				repository.RedigeraAnvändare(anvandare);
				return Status.Låste;
			}
			else if (anvandare.LosenordHash != null && Crypto.VerifyHashedPassword(anvandare.LosenordHash, lösenord))
			{
				return Status.Lyckades;
			}
			else
			{
				if (anvandare.Raknare == null) anvandare.Raknare = 1;
				else anvandare.Raknare += 1;

				repository.RedigeraAnvändare(anvandare);
				return Status.Misslyckades;
			}
		}

		//Bara för test
		public void SättLösenord(Anvandare anvandare, string lösenord)
		{
			//anvandare.LosenordHash = Crypto.HashPassword(lösenord);
		}

		//Bara för test
		public void SkapaAnvändare(Anvandare användare)
		{
			//repository.SkapaAnvändare(användare);
		}

		/*
		 * Förstör finns för att fria upp minne.
		 */
		public void Förstör()
		{
			repository.Förstör();
		}
	}
}