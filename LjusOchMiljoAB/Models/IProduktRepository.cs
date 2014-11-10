using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*
 * IProduktRepository är en interface som implementeras av
 * EntityProdukterManagerRepository för huvudprogram (och kontakt med databasen),
 * och InMemoryProdukterRepository för testningen (med lösas databas med listor).
 * 
 * I att ProdukterController tar en IProduktRepository som sin kontakt med
 * databasen så kan man köra riktigt eller köra 'mock databasen' för testning.
 * 
 * Grupp 2
 * Senast ändrat: 2014 11 09
 * Version: 0.17
 */
namespace LjusOchMiljoAB.Models
{
	public interface IProduktRepository
	{
		void SkapaProdukt(Produkt produktAttSkapa);
		void TaBortProdukt(string id);
		void RedigeraProdukt(Produkt produktAttÄndra);
		Produkt HämtaProduktMedID(string id);
		IEnumerable<Produkt> HämtaProduktlista();
		int SparaÄndringar();
		void Förstör();
	}
}