using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LjusOchMiljoAB.Models;
using PagedList.Mvc;
using PagedList;
using System.Threading.Tasks;

namespace LjusOchMiljoAB.Controllers
{
	/*
	 * ProduktService är tjänsten som hantera kontakt med ProduktRepository
	 * för att tar fram listan av produkter eller sök en produkt baserad på
	 * ID.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 11
	 * Version: 0.19
	 */
	public class ProduktTjänst : IProduktTjänst
	{
		//Hur många sidor som visas samtidigt utan att skapa en ny sida
		public int antalProdukter = 5;

		//IProduktRepository hanterar kommunikation med databasen
		private readonly IProduktRepository repository;

		//Vid tom konstruktör, gör en ny repository av typen som används för
		//verklig körning
		public ProduktTjänst() : this(new ProduktRepository()) { }

		//En-parameter konstruktör för testning mot en egen repository
		public ProduktTjänst(IProduktRepository repository)
		{
			this.repository = repository;
		}

		/*
		 * HämtaProdukter får en lista produkter från respository:n och returnerar
		 * listan.
		 * 
		 * ut: Task för await och en IEnumerable av Produkt objekt
		 */
		public async Task<IEnumerable<Produkt>> HämtaProdukter()
		{
			IEnumerable<Produkt> produkter = await repository.HämtaProduktlista();

			return produkter;
		}

		/*
		 * HämtaValLista hämtar listan av alla Typ namn som finns av produkter med
		 * en vald produkt typ (som kan vara "" vilket blir default ('All'))
		 * 
		 * in:	produkter: en IEnumerable av produkt objekt
		 *		produktTyp: en sträng som ska innehålla en Typ av produkt
		 * ut:	en SelectList som innehåller namn av produkt typer med produktTyp
		 *		som vald produkt
		 */
		public SelectList HämtaValLista(IEnumerable<Produkt> produkter, string produktTyp)
		{
			//En lista för att hålla typer för filtrering
			var TypLst = new List<string>();

			//En fråga efter alla typer av produkter i typ ordning
			var TypQry = from d in produkter
						 orderby d.Typ
						 select d.Typ;

			//Listan fylls med alla unika typer som finns 
			TypLst.AddRange(TypQry.Distinct());

			return new SelectList(TypLst, produktTyp);
		}

		/*
		 * HämtaFiltreradProduktlista filtrerar angiven IEnumerable av produkter
		 * efter Typ och en sökning på Namn.
		 * 
		 * in:	produkter: en IEnumerable av produkt objekt
		 *		produktTyp: en sträng som ska innehålla en Typ av produkt
		 *		sökSträng: en sträng som ska innehålla en söksträng efter namn på
		 *			en produkt
		 * ut:	en IEnumerable av Produkt objekt
		 */
		public IEnumerable<Produkt> HämtaFiltreradProduktlista(IEnumerable<Produkt> produkter, string produktTyp, string sökSträng)
		{
			//Gör en lista med alla produkter
			var produktLista = from m in produkter
							   select m;

			//Om där finns en söksträng, filtrera produkter efter namn som innehåller
			//söksträngen
			if (!String.IsNullOrEmpty(sökSträng))
			{
				produkter = produkter.Where(s => s.Namn.ToUpper().Contains(sökSträng.ToUpper()));
			}

			//Om där finns en vald produktyp (där All är en default fall), filtreras
			//produkter efter vald typ
			if (!string.IsNullOrEmpty(produktTyp))
			{
				produkter = produkter.Where(x => x.Typ == produktTyp);
			}

			return produkter;
		}

		/*
		 * HämtaOrdnadProduktlista ändra ordning av angiven IEnumerable av Produkt objekt
		 * baserad på en sträng som representera ordningstypen.  Default är A->Ö för Namn.
		 * 
		 * in:	produkter: en IEnumerable av produkt objekt
		 *		ordning: en sträng som ska representera en ordningstyp
		 * ut:	en IEnumerable av Produkt objekt
		 */
		public IEnumerable<Produkt> HämtaOrdnadProduktlista(IEnumerable<Produkt> produkter, string ordning)
		{
			//Hantera ordningen.  Det går fram och tillbaka mot ordningen
			//högst till lägst (eller Ö->A), och lägst till högst (eller A->Ö)
			//Default är A->Ö för namn
			switch (ordning)
			{
				case "DesNamn_Ordning":
					produkter = produkter.OrderByDescending(n => n.Namn);
					break;
				case "DesID_Ordning":
					produkter = produkter.OrderByDescending(n => n.ID);
					break;
				case "AscID_Ordning":
					produkter = produkter.OrderBy(n => n.ID);
					break;
				case "AscTyp_Ordning":
					produkter = produkter.OrderBy(n => n.Typ);
					break;
				case "DesTyp_Ordning":
					produkter = produkter.OrderByDescending(n => n.Typ);
					break;
				case "AscPris_Ordning":
					produkter = produkter.OrderBy(n => n.Pris);
					break;
				case "DesPris_Ordning":
					produkter = produkter.OrderByDescending(n => n.Pris);
					break;
				default:
					produkter = produkter.OrderBy(n => n.Namn);
					break;
			}

			return produkter;
		}


		/*
		 * ToPagedList är en speciell tillägg till IEnumerables bland annat
		 * mha 'using PagedList och PagedList.Mvc'.  Den kopplar till en
		 * css model (ser PagedList.css under Content mappen) som används i
		 * vyn.  Den tar emot sin egen räkning på sidor och en definition
		 * av hur många element att visa åt gången (definerad som instansvariabeln
		 * antalProdukter i ProduktTjänst).
		 * 
		 * in:	produkter: en IEnumerable av produkt objekt
		 *		sida: en int som representera nuvarande sidonummer
		 * ut:	en IPagedList objekt som är en speciell IEnumerable eller Lista med
		 *			egendefinerade sidor
		 */
		public IPagedList HämtaSida(IEnumerable<Produkt> produkter, int? sida)
		{
			//En variabel som PagedList sätter själv efter vilken sida man är på eller 1 
			int antalSidor = (sida ?? 1);

			return produkter.ToPagedList(antalSidor, antalProdukter);
		}

		/*
		 * HämtaProduktMedID hämtar en produkt från repository:n som har samma id som
		 * den angiven.
		 * 
		 * in:	id: en sträng som ska innehålla en unik id för en produkt
		 * ut:	Task för await och en Produkt objekt
		 */
		public async Task<Produkt> HämtaProduktMedID(string id)
		{
			return (await repository.HämtaProduktMedID(id));
		}

		/*
		 * Förstör finns för att fria upp minnet när man avslutar.
		 */
		public async Task Förstör()
		{
			await repository.Förstör();
		}
	}
}