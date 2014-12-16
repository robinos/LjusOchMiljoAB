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
	/// <summary>
	/// ProduktTjänst är tjänsten som hantera kontakt med ProduktRepository
	/// för att tar fram listan av produkter eller sök en produkt baserad på
	/// ID.
	/// 
	/// -Konstruktor-
	/// ProduktTjänst - tom konstruktor som använder en ny ProduktRespository
	///		som skickas till den andra konstruktor (används vid riktig körning).
	///	ProduktTjänst(IProduktRepository repository) - tar emot en
	///		IProduktRepository för initialisering
	/// 
	/// -Metoder-
	/// HämtaProdukter - hämtar och returnerar en lista produkter från respository:n eller
	///		en tom lista vid null från respository:n
	/// HämtaValLista - hämtar listan av alla Typ namn som finns av produkter och en vald
	///		produkttyp
	/// HämtaFiltreradProduktlista - filtrerar angiven IEnumerable av produkter efter namn
	///		och typ
	/// HämtaOrdnadProduktlista - ändra ordning av angiven IEnumerable av Produkt objekt
	///		baserad på en sträng som representera ordningstypen
	/// HämtaSida - hämtar produkter i en IPagedList för en visningssida av produkter
	/// HämtaProduktMedID - hämtar en produkt från repository:n men angiven id
	/// Förstör - anropar Förstör i respositoryn:n för att fria upp minne 
	/// 
	/// Version: 1.0
	/// 2014-12-14
	/// Grupp 2
	/// </summary>
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

		/// <summary>
		/// HämtaProdukter hämtar en lista produkter från respository:n och returnerar
		/// listan. Om listan skulle vara null hämtas en tom lista.
		/// </summary>
		/// <returns>Task för async och en IEnumerable av Produkt objekt</returns>
		public async Task<IEnumerable<Produkt>> HämtaProdukter()
		{
			IEnumerable<Produkt> produkter = await repository.HämtaProduktlista();

			if (produkter == null)
				produkter = new List<Produkt>();

			return produkter;
		}

		/// <summary>
		/// HämtaValLista hämtar listan av alla Typ namn som finns av produkter och
		/// en vald produkttyp (som kan vara "" vilket blir default ('All'))
		/// </summary>
		/// <param name="produkter">en IEnumerable av produkt objekt</param>
		/// <param name="produktTyp">en sträng som ska innehålla en Typ av produkt</param>
		/// <returns>En SelectList för en DropDownBox</returns>
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

		/// <summary>
		/// HämtaFiltreradProduktlista filtrerar angiven IEnumerable av produkter
		/// efter Typ och en sökning på Namn.
		/// </summary>
		/// <param name="produkter">en IEnumerable av produkt objekt</param>
		/// <param name="produktTyp">en sträng som ska innehålla en Typ av produkt</param>
		///	<param name="produktTyp">en sträng som ska innehålla en söksträng efter namn på
		///		en produkt</param> 
		/// <returns>en IEnumerable av Produkt objekt</returns>
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

		/// <summary>
		/// HämtaOrdnadProduktlista ändra ordning av angiven IEnumerable av Produkt objekt
		/// baserad på en sträng som representera ordningstypen.  Default är A->Ö för Namn.
		/// </summary>
		/// <param name="produkter">en IEnumerable av produkt objekt</param>
		/// <param name="ordning">en sträng som ska representera en ordningstyp</param>
		/// <returns>en IEnumerable av Produkt objekt</returns>
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

		/// <summary>
		/// HämtaSida hämtar produkter i en IPagedList för en visningssida av
		/// produkter.
		/// 
		/// ToPagedList är en speciell tillägg till IEnumerables bland annat
		/// mha 'using PagedList och PagedList.Mvc'.  Den kopplar till en
		/// css model (ser PagedList.css under Content mappen) som används i
		/// vyn.  Den tar emot sin egen räkning på sidor och en definition
		/// av hur många element att visa åt gången (definerad som instansvariabeln
		/// antalProdukter i ProduktTjänst).
		/// </summary>
		/// <param name="produkter">en IEnumerable av produkt objekt</param>
		/// <param name="sida">en int som representera nuvarande sidonummer</param>
		/// <returns>en IPagedList objekt som är en speciell IEnumerable eller Lista med
		///		egendefinerade sidor</returns>
		public IPagedList HämtaSida(IEnumerable<Produkt> produkter, int? sida)
		{
			//En variabel som PagedList sätter själv efter vilken sida man är på eller 1 
			int antalSidor = (sida ?? 1);

			return produkter.ToPagedList(antalSidor, antalProdukter);
		}

		/// <summary>
		/// HämtaProduktMedID hämtar en produkt från repository:n som har samma id som
		/// den angiven.
		/// </summary>
		/// <param name="id">en sträng som ska innehålla en unik id för en produkt</param>
		/// <returns>Task för await och en Produkt objekt</returns>
		public async Task<Produkt> HämtaProduktMedID(string id)
		{
			return(await repository.HämtaProduktMedID(id)); 
		}

		/// <summary>
		/// Förstör finns för att fria upp minnet när man avslutar applikationen.
		/// </summary>
		/// <returns>Task för await (ingen data returneras)</returns>
		public async Task Förstör()
		{
			await repository.Förstör();
		}
	}
}