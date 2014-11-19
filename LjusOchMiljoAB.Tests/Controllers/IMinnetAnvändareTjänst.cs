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
	/*
	 * Mock AnvändareTjänst för testning.
	 * 
	 * Förhoppningsvis behövs det här inte alls om man kan förstå
	 * MvcContrib.TestHelper bibliotek lite bättre.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 19
	 * Version: 0.19
	 */
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

		/*
		 * En ren kopia av BekräftaLösenord från AnvändareTjänst.
		 * 
		 * in:	användarnamn som sträng
		 *		lösnord som sträng
		 * ut:	Task (för en await) och Status som är en enum definerad i
		 *		IAnvändareTjänst som har värden Lyckades, Misslyckades, eller Låste
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
		 * SättLösenord Finns bara i mock versionen för att kryptera lösnordet när
		 * en användare skapas för testning.
		 * 
		 * in:	användarnamn som sträng
		 *		lösnord som sträng
		 */
		public void SättLösenord(Anvandare anvandare, string lösenord)
		{
			anvandare.LosenordHash = Crypto.HashPassword(lösenord);
		}

		/*
		 * SkapaAnvändare har bara kod i IMinnetAnvändareTjänst och inte i
		 * AnvändareTjänst. Den används för att skapa test användare.
		 */
		public void SkapaAnvändare(Anvandare användare)
		{
			repository.SkapaAnvändare(användare);
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
		 * Tom metod utan FormsAuthentication (som funkar inte så bra med
		 * Unit testar).
		 */
		public void Inloggning(string användarnamn)
		{
		}

		/*
		 * Tom metod utan FormsAuthentication (som funkar inte så bra med
		 * Unit testar).
		 */
		public void Utloggning()
		{
		}
	}
}
