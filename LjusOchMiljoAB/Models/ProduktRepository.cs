using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace LjusOchMiljoAB.Models
{
	/*
	 * ProduktRepository har hand om entiteten Produkt (dvs
	 * databaskontakt för Produkt tabellen).  Den implementera interface
	 * IProdukterRepository.  Detta görs för att göra det lättare vid testning
	 * men även lättare för databasbytt och ändringar där huvudkoden behöver inte
	 * ändras.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 18
	 * Version: 0.19
	 */
	public class ProduktRepository : IProduktRepository
	{
		//instansvariabler
		//Databas objekten
		private LOM_DBEntities db = new LOM_DBEntities();

		/*
		 * HämtaProduktMedID hämtar en produkt (rad) från tabellen Produkt som
		 * matcher strängen id för ID kolumnen.  ID är den unika nycklen för
		 * tabellen.
		 * 
		 * Kastar System.ArgumentNullException
		 * 
		 * in: strängen som representera ID för en produkt
		 * ut: Task för await och en produkt av objekttyp Produkt som har id
		 * som sin ID
		 */
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
				//Exception hantering kunde vara här
				throw ex;
			}

			return produkt;
		}

		/*
		 * HämtaProduktlista hämtar listan av alla produkter (rad) i tabellen
		 * Produkt.
		 * 
		 * Kastar System.ArgumentNullException
		 * 
		 * ut: en IEnumerable av Produkt objekter
		 */
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
				//Exception hantering kunde vara här
				throw ex;
			}

			return produkter;
		}

		/*
		 * Förstör tar bort instansen av databasen och kan användas för minnesrengöring
		 * vid applikationsslut.
		 */
		public async Task Förstör()
		{
			await Task.Delay(0);			
			db.Dispose();
		}
	}
}