using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LjusOchMiljoAB.Models;

namespace LjusOchMiljoAB.Tests.Models
{
	/*
	 * InMemoryProdukterRepository implementerar IProdukterRepository.  Den används
	 * som 'mock databas' eller lösas databas vid testning.  I att den implementerar
	 * IProdukterRepository så kan en ProdukterController skapas som använder den
	 * som databas kontakt.
	 * 
	 * (Tyvärr heter klassen Produkter istället för Produkt för att tabellen i
	 * databasen hette så.  Det borde ändras sedan.)
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 04
	 * Version: 0.17
	 */
	class InMemoryProdukterRepository : IProduktRepository
	{
		//instansvariabler
		//Listan db blir vår lösas databas
		private List<Produkter> db = new List<Produkter>();

		public Exception ExceptionToThrow { get; set; }

		/*
		 * SparaÄndringar sparar ändringar till en produkt till listan db
		 * 
		 * in: produktAttUppdatera är en produkt av objekttypen Produkter
		 */
		public void SparaÄndringar(Produkter produktAttUppdatera)
		{
			foreach (Produkter produkt in db)
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

		/*
		 * Add används för att lägga till produkter till listan db
		 * 
		 * in: produktAttTillägga är en produkt av objekttypen Produkter
		 */
		public void Add(Produkter produktAttTillägga) {
			db.Add(produktAttTillägga);
        }

		/*
		 * HämtaProduktMedID hittar en produkt i listan som har en ID lika med
		 * strängen id, eller returnerar ett default värde om ingenting hittas.
		 * 
		 * in: strängen id som en ID av en produkt
		 * ut: en produkt av objekttypen Produkter med given ID (eller default)
		 */
		public Produkter HämtaProduktMedID(string id)
		{
            return db.FirstOrDefault(d => d.ID == id);
        }

		/*
		 * SkapaProdukt kastar en Exception om ExceptionToThrow är inte null.  Jag
		 * vet inte riktigt varför den finns men det har något att göra med att
		 * återskapa exceptionen som den riktiga applikationen kan kasta.
		 * Annars den skapar en ny produkt med produktAttSkapa genom att kalla på
		 * metoden Add.
		 * 
		 * in: produktAttSkapa är en produkt av objekttypen Produkter
		 */
		public void SkapaProdukt(Produkter produktAttSkapa)
		{
            if (ExceptionToThrow != null)
                throw ExceptionToThrow;

			db.Add(produktAttSkapa);
        }

		/*
		 * SparaÄndringar är bara en återspegling för IProdukterRepository och
		 * gör ingenting äv värde.
		 */
		public int SparaÄndringar()
		{
            return 1;
        }

		/*
		 * HämtaProduktlista returnerar alla produkter.
		 * 
		 * ut: IEnumerable<Produkter> innehåller alla produkter
		 */
		public IEnumerable<Produkter> HämtaProduktlista()
		{
            return db.ToList();
        }

		/*
		 * TaBortProdukt tar bort en produkt i listan som har en ID lika med
		 * strängen id.
		 * 
		 * in: strängen id som en ID av en produkt
		 */
		public void TaBortProdukt(string id)
		{
			db.Remove(HämtaProduktMedID(id));
        }

		/*
		 * RedigeraProdukt är en återspegling för IProdukterRepository och
		 * använder SparaÄndringar för att spara ändringar till produkten
		 * produktAttÄndra.
		 * 
		 * in: produktAttÄndra är en produkt av objekttypen Produkter
		 */
		public void RedigeraProdukt(Produkter produktAttÄndra)
		{
			SparaÄndringar(produktAttÄndra);
		}

		/*
		 * Förstör databasen för att fri upp minne (i det här fallet är databasen
		 * listan db).
		 */
		public void Förstör()
		{
			db = null;
		}
	}
}
