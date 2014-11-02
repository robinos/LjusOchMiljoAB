using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace LjusOchMiljoAB.Models
{
	public class EntityProdukterManagerRepository : IProdukterRepository
	{
		private LOM_DBEntities db = new LOM_DBEntities();

		public void SkapaProdukt(Produkter produktAttSkapa)
		{
			db.Produkter.Add(produktAttSkapa);
			db.SaveChanges();
		}

		public void TaBortProdukt(string id)
		{
			var conToDel = HämtaProduktMedID(id);
			db.Produkter.Remove(conToDel);
			db.SaveChanges();
		}

		public void RedigeraProdukt([Bind(Include = "ID,Namn,Pris,Typ,Farg,Bildfilnamn,Ritningsfilnamn,RefID,Beskrivning,Montering")] Produkter produktAttÄndra)
		{
			db.Entry(produktAttÄndra).State = EntityState.Modified;
			db.SaveChanges();
		}

		public Produkter HämtaProduktMedID(string id)
		{
			return db.Produkter.FirstOrDefault(d => d.ID == id);
		}

		public IEnumerable<Produkter> HämtaProduktlista()
		{
			return db.Produkter.ToList<Produkter>();
		}

		public int SparaÄndringar()
		{
			return db.SaveChanges();
		}

		public void Förstör()
		{
				db.Dispose();
		}
	}
}