using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LjusOchMiljoAB.Models;
using LjusOchMiljoAB.Tests.Models;
using System.Web.Helpers;

namespace LjusOchMiljoAB.Tests.Controllers
{
	/// <summary>
	/// Mock AnvändareTjänst för testning. Den behövs för att den riktiga
	/// AnvändareTjänst använder sig av FormsAuthentication inloggnings kakor som
	/// enhetstestar kan inte hantera.
	/// 
	/// -Konstruktor-
	/// IMinnetAnvändareTjänst - tom konstruktor som använder en ny 
	///		IMinnetAnvändareRespository som skickas till den andra konstruktor.
	///	IMinnetAnvändareTjänst(IAnvändareRepository repository) - tar emot en
	///		IAnvändareRespository för initialisering
	/// 
	/// -Metoder-
	/// BekräftaLösenord - Returnerar Status.Lyckades, Misslyckades eller Låst
	///		beroende på om lösenordet var korrekt för användaren
	/// SättLösenord - krypterar lösnordet när en användare skapas för testning
	/// SkapaAnvändare - används för att skapa test användare
	/// Förstör - finns för att fria upp minne
	/// Inloggning - tom metod som speglar IAnvändareTjänst
	/// Utloggning - som ovan
	/// 
	/// Version: 1.0
	/// 2014-12-12
	/// Grupp 2
	/// </summary>
	class IMinnetAnvändareTjänst : IAnvändareTjänst
	{
		//IAnvändareRepository hanterar kommunikation med databasen
		private readonly IAnvändareRepository repository;

		//Vid tom konstruktör, gör en ny repository av typen som används för
		//verklig körning
		public IMinnetAnvändareTjänst() : this(new IMinnetAnvändareRepository()) { }

		//En-parameter konstruktör för testning mot en egen repository
		public IMinnetAnvändareTjänst(IAnvändareRepository repository)
		{
			this.repository = repository;
		}

		/// <summary>
		/// En ren kopia av BekräftaLösenord från AnvändareTjänst.
		/// 
		/// BekräftaLösenord försöker hämta Anvandare objektet som har användarnamn
		/// som namn och sedan jämför angiven lösenordets hash med den från objektet.
		/// Om 5 eller fler misslyckade inloggningar har inträffat blir kontot låste.
		/// Annars om lösenordets hash matchar har det lyckats och annars har det
		/// misslyckats.
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
				användare.Last = true;
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
		/// SättLösenord finns bara i mock versionen för att kryptera lösnordet när
		/// en användare skapas för testning.
		/// </summary>
		/// <param name="anvandare">En Anvandare objekt för en användare</param>
		/// <param name="lösenord">Lösenord som sträng</param> 
		public void SättLösenord(Anvandare anvandare, string lösenord)
		{
			anvandare.LosenordHash = Crypto.HashPassword(lösenord);
		}

		/// <summary>
		/// SkapaAnvändare används för att skapa test användare.
		/// </summary>
		/// <param name="användareAttTillägga">en användare att lägga till</param>
		public void SkapaAnvändare(Anvandare användare)
		{
			repository.SkapaAnvändare(användare);
		}

		/// <summary>
		/// Förstör finns för att fria upp minne.
		/// </summary>
		/// <returns>Task för async (ingen data returneras)</returns>
		public async Task Förstör()
		{
			await repository.Förstör();
		}

		/// <summary>
		/// Tom metod utan FormsAuthentication (som funkar inte så bra med
		/// enhetstestar).
		/// </summary>
		/// <param name="användarnamn">sträng för användarnamn</param>
		public void Inloggning(string användarnamn)
		{
		}

		/// <summary>
		/// Tom metod utan FormsAuthentication (som funkar inte så bra med
		/// enhetstestar).
		/// </summary>
		public void Utloggning()
		{
		}
	}
}
