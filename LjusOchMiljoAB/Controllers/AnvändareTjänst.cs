using System;
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
	/// <summary>
	/// AnvändareTjänst är tjänsten som hantera kontakt med en IAnvändareRepository
	/// för att autentisera användare, håller koll på misslyckade lösenordsförsök
	/// och låsa ut användare vid 5 misslyckade försök.
	/// 
	/// -Konstruktor-
	/// AnvändareTjänst - tom konstruktor som använder en ny AnvändareRespository
	///		som skickas till den andra konstruktor (används vid riktig körning).
	///	AnvändareTjänst(IAnvändareRepository repository) - tar emot en
	///		IAnvändareRespository för initialisering
	/// 
	/// -Metoder-
	/// BekräftaLösenord - Returnerar Status.Lyckades, Misslyckades eller Låst
	///		beroende på om lösenordet var korrekt för användaren
	/// Förstör - Anropar Förstör i en IAnvändareRepository för att förstöra den och
	///		fria upp minne
	/// Inloggning - Skapar en inloggningskaka för Forms Autenetisering med
	///		användarens namn (används även för att visa användarens namn).
	/// Utloggning - Tar bort inloggningskakan.
	/// SkapaAnvändare - En tom implementation av SkapaAnvändare i IAnvändareTjänst.
	///		Den finns med för andra klasser av typen IAnvändareTjänst i testning.
	/// 
	/// Version: 1.0
	/// 2014-12-10
	/// Grupp 2
	/// </summary>
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

		/// <summary>
		/// BekräftaLösenord försöker hämta Anvandare objektet som har användarnamn
		/// som namn och sedan jämför angiven lösenordets hash med den från objektet.
		/// Om 5 eller fler misslyckade inloggningar har inträffat blir kontot låste.
		/// Annars om lösenordets hash matchar har det lyckats och annars har det
		/// misslyckats.
		/// Metoden är async för att ta emot från async metoder HämtaAnvändareMedNamn
		///	och RedigeraAnvändare men även för att tjänster ofta använder async metoder
		///	för att vänta på svar och det underlätta om man skulle vilja byta ut 
		///	repositoryn eller tjänsten mot något icke lokal tjänst.
		/// </summary>
		/// <param name="användarnamn">sträng för användarnamn</param>
		/// <param name="lösenord">sträng för lösenord</param>
		/// <returns>Task för async och Status enum som man skickar tillbaka som
		///		Lyckades, Misslyckades, eller Låst</returns>
		public async Task<Status> BekräftaLösenord(string användarnamn, string lösenord)
		{
			//Hämta användare objekt från respositoryn
			Anvandare användare = await repository.HämtaAnvändareMedNamn(användarnamn);

			//Om Anvandare objektet är null misslyckas det direkt
			if (användare == null) return Status.Misslyckades;

			//Om räknaren är redan 4 eller högre för misslyckade inloggningar,
			//(det här blir 5:e gången) låser kontot (just nu måste en admin
			//låser upp den igen)
			if (användare.Raknare != null && användare.Raknare >= 4)
			{
				//Ändra Anvandare objektet så låst blir sann och räknaren ökas
				//(databas kolumnen heter tyvärr Laste)
				användare.Laste = true;
				användare.Raknare += 1;
				await repository.RedigeraAnvändare(användare);

				//Returnera status Låst
				return Status.Låst;
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
				//Ändra Anvandare objektet så räknaren ökar				
				await repository.RedigeraAnvändare(användare);

				//Returnera status Misslyckades
				return Status.Misslyckades;
			}
		}

		/// <summary>
		/// Förstör finns för att fria upp minne och anropar Förstör i repositoryn.
		/// 
		/// Metoden är async för att ta emot från async metoden Förstör.
		/// </summary>
		/// <returns>Task för async (ingen data returneras)</returns>
		public async Task Förstör()
		{
			await repository.Förstör();
		}

		/// <summary>
		/// Inloggning skapar en autentiseringskaka till användaren/webbläsaren för
		/// webbsidan.
		/// </summary>
		/// <param name="användarnamn">sträng för användarnamn</param>
		public void Inloggning(string användarnamn)
		{
			FormsAuthentication.SetAuthCookie(användarnamn, false);
		}

		/// <summary>
		/// Utloggning tar bort autentiseringskakan.
		/// </summary>
		public void Utloggning()
		{
			FormsAuthentication.SignOut();
		}

		/// <summary>
		/// Bara för implementation av SkapaAnvändare fär IAnvändareTjänst.
		/// </summary>
		/// <param name="användareAttTillägga">en användare att lägga till</param>
		public void SkapaAnvändare(Anvandare användareAttTillägga)
		{
			//ingenting
		}
	}
}