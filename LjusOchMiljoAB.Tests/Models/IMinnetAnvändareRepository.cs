using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LjusOchMiljoAB.Models;

namespace LjusOchMiljoAB.Tests.Models
{
	/*
	 * IMinnetAnvändareRepository implementerar IAnvändareRepository.  Den används
	 * som 'mock databas' eller lösas databas vid testning.  I att den implementerar
	 * IAnvändareRepository så kan en AnvändareTjänst skapas som använder den
	 * som databas kontakt.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 18
	 * Version: 0.17
	 */
	class IMinnetAnvändareRepository : IAnvändareRepository
	{
		//instansvariabler
		//Listan db blir vår lösas databas
		private List<Anvandare> db = new List<Anvandare>();

		public Exception ExceptionToThrow { get; set; }

		/*
		 * RedigeraAnvändare använder SparaÄndringar för att spara ändringar
		 * till användaren användareAttÄndra.
		 * 
		 * in: användareAttÄndra är en användare av objekttypen Anvandare
		 */
		public async Task RedigeraAnvändare(Anvandare användareAttÄndra)
		{
			await Task.Delay(0);
			SparaÄndringar(användareAttÄndra);
		}

		/*
		 * HämtaAnvandareMedNamn hittar en anvandare i listan som har ett namn lika
		 * med strängen namn, eller returnerar ett default värde om ingenting hittas.
		 * 
		 * in: strängen användarnamn som en användarnamn av en användare
		 * ut: en användare av objekttypen Anvandare med given Namn (eller default)
		 */
		public async Task<Anvandare> HämtaAnvändareMedNamn(string användarnamn)
		{
			await Task.Delay(0);
			return db.FirstOrDefault(d => d.Anvandarnamn == användarnamn);
		}

		/*
		 * SparaÄndringar sparar ändringar till en användare till listan db
		 * 
		 * in: användareAttÄndra är en användare av objekttypen Anvandare
		 */
		public void SparaÄndringar(Anvandare användareAttÄndra)
		{
			foreach (Anvandare användare in db)
			{
				//Ändra bara användaren som har ändrats
				if (användare.Anvandarnamn == användareAttÄndra.Anvandarnamn)
				{
					//Tar bort den gamla och lägga till den nya
					db.Remove(användare);
					db.Add(användareAttÄndra);
					break;
				}
			}
		}

		/*
		 * SparaÄndringar är bara en återspegling för IAnvändareRepository och
		 * gör ingenting äv värde.
		 */
		public async Task<int> SparaÄndringar()
		{
			await Task.Delay(0);
			return 1;
		}

		/*
		 * Förstör databasen för att fri upp minne (i det här fallet är databasen
		 * listan db).
		 */
		public async Task Förstör()
		{
			await Task.Delay(0);
			db = null;
		}

		/*
		 * SkapaAnvändare används för att lägga till användare till listan db
		 * 
		 * in: användareAttTillägga är en användare av objekttypen Anvandare
		 */
		public void SkapaAnvändare(Anvandare användareAttTillägga)
		{
			db.Add(användareAttTillägga);
		}
	}
}
