using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*
 * IProdukterRepository är en interface som implementeras av
 * EntityProdkterManagerRepository för huvudprogram (och kontakt med databasen),
 * och InMemoryProdukterRepository för testningen (med lösas databas med listor).
 * 
 * I att ProdukterController tar en IProdukterRepository som sin kontakt med
 * databasen så kan man köra riktigt eller köra 'mock databasen' för testning.
 * 
 * Grupp 2
 * Senast ändrat: 2014 11 04
 * Version: 0.16b
 */
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