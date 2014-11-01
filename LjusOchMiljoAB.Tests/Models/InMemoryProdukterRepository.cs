using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LjusOchMiljoAB.Models;

namespace LjusOchMiljoAB.Tests.Models
{
	class InMemoryProdukterRepository : IProdukterRepository
	{
		private List<Produkter> db = new List<Produkter>();

		public Exception ExceptionToThrow { get; set; }

		public void SparaÄndringar(Produkter produktAttUppdatera)
		{
			foreach (Produkter produkt in db)
			{
				if (produkt.ID == produktAttUppdatera.ID)
				{
					db.Remove(produkt);
					db.Add(produktAttUppdatera);
					break;
				}
			}
		}

        public void Add(Produkter produktAttTillägga) {
			db.Add(produktAttTillägga);
        }

		public Produkter HämtaProduktMedID(string id)
		{
            return db.FirstOrDefault(d => d.ID == id);
        }

		public void SkapaProdukt(Produkter produktAttSkapa)
		{
            if (ExceptionToThrow != null)
                throw ExceptionToThrow;

			db.Add(produktAttSkapa);
        }

		public int SparaÄndringar()
		{
            return 1;
        }

		public IEnumerable<Produkter> HämtaProduktlista()
		{
            return db.ToList();
        }

		public void TaBortProdukt(string id)
		{
			db.Remove(HämtaProduktMedID(id));
        }

		public void RedigeraProdukt(Produkter produktAttÄndra)
		{
			SparaÄndringar(produktAttÄndra);
		}

		public void Förstör()
		{
			db = null;
		}
	}
}
