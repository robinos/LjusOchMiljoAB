using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LjusOchMiljoAB.Models;

namespace LjusOchMiljoAB.Tests.Models
{
	/// <summary>
	/// IMinnetProduktRepository implementerar IProdukterRepository.  Den används
	/// som 'mock databas' eller lösas databas vid testning.  I att den implementerar
	/// IProdukterRepository så kan en objekt som implementera IProduktTjänst skapas
	/// som använder den som databas kontakt.
	/// 
	/// -Metoder-
	/// SparaÄndringar(Produkt produktAttUppdatera) - sparar ändringar till en produkt
	///		i listan db	
	///	Add - används för att lägga till en produkt till listan db
	/// HämtaProduktMedID - hämtar en produkt i listan som har angiven id
	/// SkapaProdukt - skapar en ny produkt med produktAttSkapa genom att anropa
	///		metoden Add
	/// HämtaProduktlista - hämtar listan av alla produkter i listan
	/// TaBortProdukt - tar bort en produkt i listan som har angiven id
	/// RedigeraProdukt - använder SparaÄndringar för att spara ändringar till en produkt
	/// Förstör - sätt listan db till null
	/// 
	/// Version: 1.0
	/// 2014-12-14
	/// Grupp 2
	/// </summary>
	class IMinnetProduktRepository : IProduktRepository
	{
		//instansvariabler
		//Listan db blir vår lösas databas
		private List<Produkt> db = new List<Produkt>();

		//En exception att kasta om man skulle testa för exceptioner.  Tyvärr
		//används den inte än.
		public Exception ExceptionToThrow { get; set; }

		/// <summary>
		/// SparaÄndringar sparar ändringar till en produkt i listan db.
		/// </summary>
		/// <param name="produktAttUppdatera">en produkt av objekttypen Produkt</param>
		public void SparaÄndringar(Produkt produktAttUppdatera)
		{
			foreach (Produkt produkt in db)
			{
				//Ändra bara produkten som har ändrats
				if (produkt.ID == produktAttUppdatera.ID)
				{
					//Tar bort den gamla och lägga till den nya
					db.Remove(produkt);
					db.Add(produktAttUppdatera);
					break;
				}
			}
		}

		/// <summary>
		/// Add används för att lägga till en produkt till listan db
		/// </summary>
		/// <param name="produktAttTillägga">en produkt av objekttypen Produkt</param>
		public void Add(Produkt produktAttTillägga) {
			db.Add(produktAttTillägga);
        }

		/// <summary>
		/// HämtaProduktMedID hämtar en produkt i listan som har en ID lika med
		/// strängen id, eller returnerar ett default värde om ingenting hittas.
		/// </summary>
		/// <param name="id">en sträng som ska innehålla en unik id för en produkt</param>
		/// <returns>Task för await och en Produkt objekt</returns>
		public async Task<Produkt> HämtaProduktMedID(string id)
		{
			await Task.Delay(0);
            return db.FirstOrDefault(d => d.ID == id);
        }

		/// <summary>
		/// SkapaProdukt skapar en ny produkt med produktAttSkapa genom att anropa
		/// metoden Add.
		/// </summary>
		/// <param name="produktAttSkapa">en produkt av objekttypen Produkt</param>
		public void SkapaProdukt(Produkt produktAttSkapa)
		{
			db.Add(produktAttSkapa);
        }

		/// <summary>
		/// HämtaProduktlista hämtar listan av alla produkter i listan.
		/// </summary>
		/// <returns>Task för async och en IEnumerable av Produkt objekt</returns>
		public async Task<IEnumerable<Produkt>> HämtaProduktlista()
		{
			await Task.Delay(0);
            return db.ToList();
        }

		/// <summary>
		/// TaBortProdukt tar bort en produkt i listan som har en ID lika med
		/// strängen id.
		/// </summary>
		/// <param name="id">strängen id som en ID av en produkt</param>
		/// <returns>void async (ingen data returneras)</returns> 
		public async void TaBortProdukt(string id)
		{
			db.Remove(await HämtaProduktMedID(id));
        }

		/// <summary>
		/// RedigeraProdukt använder SparaÄndringar för att spara ändringar
		/// till produkten produktAttÄndra.
		/// </summary>
		/// <param name="produktAttÄndra">Produkt objektet att ändra</param>
		public void RedigeraProdukt(Produkt produktAttÄndra)
		{
			SparaÄndringar(produktAttÄndra);
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
	}
}
