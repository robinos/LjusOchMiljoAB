using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace LjusOchMiljoAB.Models
{
	/// <summary>
	/// ProduktRepository är implementationen av IProduktRepository för
	/// kontakt med databas tabellen Produkt.
	/// 
	/// -Metoder-
	/// HämtaProduktMedID - hämtar en produkt (rad) från tabellen Produkt med angiven id
	/// HämtaProduktlista - hämtar listan av alla produkter (rad) i tabellen Produkt
	/// Förstör - tar bort instansen av databasen för att fria upp minne
	/// 
	/// Version: 1.0
	/// 2014-12-14
	/// Grupp 2
	/// </summary>
	public class ProduktRepository : IProduktRepository
	{
		//instansvariabler
		//Databas objekten
		private LOM_DBEntities db = new LOM_DBEntities();

		/// <summary>
		/// HämtaProduktMedID hämtar en produkt (rad) från tabellen Produkt som
		/// matcher strängen id för ID kolumnen.  ID är den unika nycklen för tabellen.
		/// 
		/// Kastar System.ArgumentNullException
		/// </summary>
		/// <param name="id">en sträng som ska innehålla en unik id för en produkt</param>
		/// <returns>Task för await och en Produkt objekt</returns>
		public async Task<Produkt> HämtaProduktMedID(string id)
		{
			await Task.Delay(0);

			Produkt produkt = null;

			try
			{
				produkt = db.Produkt.FirstOrDefault(d => d.ID == id);
			}
			catch (Exception ex)
			{
				//Exception loggning kunde vara här
				Console.WriteLine(ex);
			}

			return produkt;
		}

		/// <summary>
		/// HämtaProduktlista hämtar listan av alla produkter (rad) i tabellen Produkt.
		/// 
		/// Kastar System.ArgumentNullException
		/// </summary>
		/// <returns>Task för async och en IEnumerable av Produkt objekt</returns>
		public async Task<IEnumerable<Produkt>> HämtaProduktlista()
		{
			await Task.Delay(0);

			IEnumerable<Produkt> produkter = null;

			try
			{
				produkter = db.Produkt.ToList<Produkt>();
			}
			catch (Exception ex)
			{
				//Exception loggning kunde vara här
				Console.WriteLine(ex);
			}

			return produkter;
		}

		/// <summary>
		/// Förstör tar bort instansen av databasen och kan användas för att fria upp
		/// minne vid applikationsslut.
		/// </summary>
		/// <returns>Task för await (ingen data returneras)</returns>
		public async Task Förstör()
		{
			await Task.Delay(0);			
			db.Dispose();
		}
	}
}