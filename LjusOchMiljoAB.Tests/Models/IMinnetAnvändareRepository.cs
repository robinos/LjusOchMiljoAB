using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LjusOchMiljoAB.Models;

namespace LjusOchMiljoAB.Tests.Models
{
	/// <summary>
	/// IMinnetAnvändareRepository implementerar IAnvändareRepository.  Den används
	/// som 'mock databas' eller lösas databas vid testning.  I att den implementerar
	/// IAnvändareRepository så kan en objekt som implementera IAnvändareTjänst skapas
	/// som använder den som databas kontakt.
	/// 
	/// -Metoder-
	/// RedigeraAnvändare - använder SparaÄndringar för att spara ändringar till
	///		angiven Anvandare objekt
	/// HämtaAnvändareMedNamn - hittar en anvandare i listan som har användarnamnet
	/// SparaÄndringar(Anvandare användareAttÄndra) - sparar ändringar till en
	///		användare i listan db
	/// SparaÄndringar - spegling av SparaÄndringar från IAnvändareRespository
	/// Förstör - sätt listan db till null
	/// SkapaAnvändare - lägga till användare till listan db
	/// 
	/// Version: 1.0
	/// 2014-12-12
	/// Grupp 2
	/// </summary>
	class IMinnetAnvändareRepository : IAnvändareRepository
	{
		//instansvariabler
		//Listan db blir vår lösas databas
		private List<Anvandare> db = new List<Anvandare>();

		//En exception att kasta om man skulle testa för exceptioner.  Tyvärr
		//används den inte än.
		public Exception ExceptionToThrow { get; set; }

		/// <summary>
		/// RedigeraAnvändare använder SparaÄndringar för att spara ändringar
		/// till användaren användareAttÄndra.
		/// </summary>
		/// <param name="användareAttÄndra">Anvandare objektet att ändra</param>
		/// <returns>Task för async (ingen data returneras)</returns> 
		public async Task RedigeraAnvändare(Anvandare användareAttÄndra)
		{
			await Task.Delay(0);
			SparaÄndringar(användareAttÄndra);
		}

		/// <summary>
		/// HämtaAnvändareMedNamn hittar en anvandare i listan som har ett namn lika 
		/// med strängen namn, eller returnerar ett default värde om ingenting hittas.
		/// </summary>
		/// <param name="användarnamn">Sträng användarnamn som namn av en användare</param>
		/// <returns>Task för async och Anvandare objektet som hittades eller default</returns>
		public async Task<Anvandare> HämtaAnvändareMedNamn(string användarnamn)
		{
			await Task.Delay(0);
			return db.FirstOrDefault(d => d.Anvandarnamn == användarnamn);
		}

		/// <summary>
		/// SparaÄndringar sparar ändringar till en användare i listan db.
		/// </summary>
		/// <param name="användareAttÄndra">Anvandare objektet att ändra</param>
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

		/// <summary>
		/// SparaÄndringar är bara en återspegling för IAnvändareRepository och
		/// gör ingenting äv värde.
		/// </summary>
		/// <returns>Task för async och 1 som resultat av databasskrivning</returns>
		public async Task<int> SparaÄndringar()
		{
			await Task.Delay(0);
			return 1;
		}

		/// <summary>
		/// Förstör databasen för att fria upp minne (i det här fallet är databasen
		/// listan db).
		/// </summary>
		/// <returns>Task för async (ingen data returneras)</returns> 
		public async Task Förstör()
		{
			await Task.Delay(0);
			db = null;
		}

		/// <summary>
		/// SkapaAnvändare används för att lägga till användare till listan db.
		/// </summary>
		/// <param name="användareAttTillägga">Anvandare objektet att lägga till</param>
		public void SkapaAnvändare(Anvandare användareAttTillägga)
		{
			db.Add(användareAttTillägga);
		}
	}
}
