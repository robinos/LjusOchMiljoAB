using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjusOchMiljoAB.Models
{
	public interface IProdukterRepository
	{
		void SkapaProdukt(Produkter produktAttSkapa);
		void TaBortProdukt(string id);
		void RedigeraProdukt(Produkter produktAttÄndra);
		Produkter HämtaProduktMedID(string id);
		IEnumerable<Produkter> HämtaProduktlista();
		int SparaÄndringar();
		void Förstör();
	}
}