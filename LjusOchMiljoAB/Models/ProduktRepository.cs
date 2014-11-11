using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace LjusOchMiljoAB.Models
{
	/*
	 * ProduktRepository har hand om entiteten Produkt (dvs
	 * databaskontakt för Produkt tabellen).  Den implementera interface
	 * IProdukterRepository.  Detta görs för att göra det lättare vid testning
	 * men även lättare för databasbytt och ändringar där huvudkoden behöver inte
	 * ändras.
	 * Bara HämtaProduktMedID, HämtaProduktlista och Förstör metoder används,
	 * resten är bara implementationer för IProdukterRepository.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 11
	 * Version: 0.18
	 */
	public class ProduktRepository : IProduktRepository
	{
		//instansvariabler
		//Databas objekten
		private LOM_DBEntities db = new LOM_DBEntities();

		/*
		 * SkapaProdukt skapar en ny produkt.  Detta används inte av applikationen
		 * och är kommenterad ut så produkter kan inte skapas.
		 * 
		 * in: produktAttSkapa är produkten man vill skapa av objekttyp Produkter 
		 */
		public void SkapaProdukt(Produkt produktAttSkapa)
		{
			//db.Produkt.Add(produktAttSkapa);
			//db.SaveChanges();
		}

		/*
		 * TaBortProdukt tar bort en befintlig produkt.  Detta används inte av
		 * applikationen och är kommenterad ut så produkter kan inte tas bort.
		 * 
		 * in: strängen id är den unika nyckeln till tabellen
		 */
		public void TaBortProdukt(string id)
		{
			//var conToDel = HämtaProduktMedID(id);
			//db.Produkt.Remove(conToDel);
			//db.SaveChanges();
		}

		/*
		 * RedigeraProdukt ändrar en befintlig produkt.  Detta används inte av applikationen
		 * och är kommenterad ut så produkter kan inte ändras.
		 * 
		 * in: produktAttSkapa är produkten man vill skapa av objekttyp Produkter med Bind för
		 * säkerställning av modellen
		 */
		public void RedigeraProdukt([Bind(Include = "ID,Namn,Pris,Typ,Farg,Bildfilnamn,Ritningsfilnamn,RefID,Beskrivning,Montering")] Produkt produktAttÄndra)
		{
			//db.Entry(produktAttÄndra).State = EntityState.Modified;
			//db.SaveChanges();
		}

		/*
		 * HämtaProduktMedID hämtar en produkt (rad) från tabellen som matcher strängen
		 * id för ID kolumnen.  ID är den unika nycklen för tabellen.
		 * 
		 * in: strängen som representera ID för en produkt
		 * ut: en produkt av objekttyp Produkter som har id som sin ID
		 */
		public Produkt HämtaProduktMedID(string id)
		{
			return db.Produkt.FirstOrDefault(d => d.ID == id);
		}

		/*
		 * 
		 */
		public IEnumerable<Produkt> HämtaProduktlista()
		{
			return db.Produkt.ToList<Produkt>();
		}

		/*
		 * SparaÄndringar sparar ändringar till databasen.  Den används inte av applikationen
		 * och är kommenterad ut.
		 * 
		 * ut: int som beskriver om sparningen lyckades (1=lyckades)
		 */
		public int SparaÄndringar()
		{
			//return db.SaveChanges();
			return 1;
		}

		/*
		 * Förstör tar bort instansen av databasen och kan användas för minnesrengöring
		 * vid applikationsslut.
		 */
		public void Förstör()
		{
				db.Dispose();
		}
	}
}