using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LjusOchMiljoAB.Models;
using PagedList.Mvc;
using PagedList;

namespace LjusOchMiljoAB.Controllers
{
	/*
	 * ProduktService
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 09
	 * Version: 0.17
	 */
	public class ProduktService : IProduktService
	{
		//Hur många sidor som visas samtidigt utan att skapa en ny sida
		public int antalProdukter = 5;

		//IProduktRepository hanterar kommunikation med databasen
		private readonly IProduktRepository repository;

		//Vid tom konstruktör, gör en ny repository av typen som används för
		//verklig körning
		public ProduktService() : this(new EntityProdukterManagerRepository()) { }

		//En-parameter konstruktör för testning mot en egen repository
		public ProduktService(IProduktRepository repository)
		{
			this.repository = repository;
		}

		public IEnumerable<Produkter> HämtaProdukter()
		{
			IEnumerable<Produkter> produkter = repository.HämtaProduktlista();

			return produkter;
		}

		public SelectList HämtaValLista(IEnumerable<Produkter> produkter, string produktTyp)
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

		public IEnumerable<Produkter> HämtaFiltreradProduktlista(IEnumerable<Produkter> produkter, string produktTyp, string sökSträng)
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

			//Om där finns en vald produktyp (där All är en speciellfall), filtreras
			//produkter efter vald typ
			if (!string.IsNullOrEmpty(produktTyp))
			{
				produkter = produkter.Where(x => x.Typ == produktTyp);
			}

			return produkter;
		}

		public IEnumerable<Produkter> HämtaOrdnadProduktlista(IEnumerable<Produkter> produkter, string Ordning)
		{
			//Hantera ordningen.  Det går fram och tillbaka mot ordningen
			//högst till lägst (eller Ö->A), och lägst till högst (eller A->Ö)
			//Default är A->Ö för namn
			switch (Ordning)
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

		//ToPagedList är en speciell tillägg till IEnumerables bland annat
		//mha 'using PagedList och PagedList.Mvc'.  Den kopplar till en
		//css model (ser PagedList.css under Content mappen) som används i
		//vyn.  Den tar emot sin egen räkning på sidor, och ovan definition
		//av hur många element att visa åt gången.
		public IPagedList HämtaSida(IEnumerable<Produkter> produkter, int? sida)
		{
			//En variabel som PagedList sätter själv efter vilken sida man är på 
			int antalSidor = (sida ?? 1);

			return produkter.ToPagedList(antalSidor, antalProdukter);
		}

		public Produkter HämtaProduktMedID(string id)
		{
			return (repository.HämtaProduktMedID(id));
		}

		/*
		 * Förstör finns för att fria upp minnet när man avslutar.
		 */
		public void Förstör()
		{
			repository.Förstör();
		}
	}
}