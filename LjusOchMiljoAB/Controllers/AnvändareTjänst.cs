﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LjusOchMiljoAB.Models;
using System.Web.Helpers;
using Microsoft.Security.Application;
using System.Threading.Tasks;
using System.Web.Security;

namespace LjusOchMiljoAB.Controllers
{
	/* 
	 * AnvändareTjänst är tjänsten som hantera kontakt med AnvändareRepository
	 * för att autentisera användare, håller koll på misslyckade lösenordsförsök
	 * och låsa ut användare vid 5 misslyckade försök.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 11
	 * Version: 0.19
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
		 * BekräftaLösenord försöker hämta Anvandare objektet som har användarnamn som
		 * namn och sedan jämför angiven lösenordets hash med den från objektet.  Om
		 * 5 eller fler misslyckade inloggningar har inträffat blir kontot låste.
		 * Annars om lösenordets hash matchar har det lyckats och annars har det
		 * misslyckats.
		 * 
		 * in:	användarnamn som rensade sträng
		 *		lösenord som rensade sträng
		 * ut:	Task för att vara async och Status som enum (Lyckades, Misslyckades,
		 *		eller Låste)
		 */
		public async Task<Status> BekräftaLösenord(string användarnamn, string lösenord)
		{
			//Hämta användare objekt från respository
			Anvandare användare = await repository.HämtaAnvändareMedNamn(användarnamn);

			//Om Anvandare objektet är null misslyckas det direkt
			if (användare == null) return Status.Misslyckades;

			//Om räknaren är redan 4 eller högre för misslyckade inloggningar,
			//(det här blir 5:e gången) låser kontot (just nu måste en admin
			//låser upp den igen)
			if (användare.Raknare != null && användare.Raknare >= 4)
			{
				//Ändra Anvandare objektet så låste blir sann
				användare.Laste = true;
				await repository.RedigeraAnvändare(användare);
				//Returnera status Låste
				return Status.Låste;
			}
			//Annars om lösenordet var rätt, har det lyckats  
			else if (användare.LosenordHash != null && Crypto.VerifyHashedPassword(användare.LosenordHash, lösenord))
			{
				//Returnera status Lyckades
				return Status.Lyckades;
			}
			//Och annars misslyckades det med inloggningen och räknaren av
			//misslyckade inloggningar räknar uppåt +1
			else
			{
				//Om räknaren är null blir den 1, annars läggs till +1
				if (användare.Raknare == null) användare.Raknare = 1;
				else användare.Raknare += 1;
				//Ändra Anvandare objektet så låste blir sann				
				await repository.RedigeraAnvändare(användare);

				//Returnera status Misslyckades
				return Status.Misslyckades;
			}
		}

		/*
		 * Förstör finns för att fria upp minne.
		 * 
		 * ut: Task för en await (behövs för async metoder)
		 */
		public async Task Förstör()
		{
			await repository.Förstör();
		}

		/*
		 * Inloggning ger en autentiseringskaka till användaren/webbläsaren för
		 * webbsidan.
		 * 
		 * in: användarnamn som rensad sträng
		 */
		public void Inloggning(string användarnamn)
		{
			FormsAuthentication.SetAuthCookie(Crypto.Hash(användarnamn), false);
		}

		/*
		 * Utloggning tar bort autentiseringskakan.
		 */
		public void Utloggning()
		{
			FormsAuthentication.SignOut();
		}

		/*
		 * Bara för implementation av SkapaAnvändare fär IAnvändareTjänst
		 */
		public void SkapaAnvändare(Anvandare användareAttTillägga)
		{
			//ingenting
		}
	}
}