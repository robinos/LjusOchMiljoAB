using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LjusOchMiljoAB.Models;
using PagedList.Mvc;
using PagedList;

namespace LjusOchMiljoAB.Controllers
{
	/*
	 * ProduktController är controller för uppvisning av produkter på
	 * söksidan, prislistan och produktsidan (details heter det just nu).
	 * 
	 * Index - produktlistan (med bildfilnamn just nu istället för bilder) med
	 * sök och sortering (och fler 'sidor')
	 * 
	 * Prislista - produktlistan i ren text med priser
	 * 
	 * Details - produktsidan med alla data om en viss produkt (bild och ritningar
	 * är fortfarande bara filnamn)
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 04
	 * Version: 0.16b
	 */
	public class ProdukterController : Controller
	{
		//IProdukterRepository hanterar kommunikation med databasen
        IProdukterRepository repository;

		//Vid tom konstruktör, gör en ny repository av typen som används för
		//verklig körning
        public ProdukterController() : this(new EntityProdukterManagerRepository()) { }
		
		//En-parameter konstruktör för testning mot en egen repository
		public ProdukterController(IProdukterRepository repository)
		{
            this.repository = repository;
        }

		/*
		 * Index är det stora produktindexet där alla produkter visas (5 per sida)
		 * men kan filtreras efter namnsökning eller vald produktkategori och
		 * ordnas efter id eller namn. Id, namn, och bildfilnamn (som kommer att
		 * bli en bild sedan) visas för produkter.
		 * Just nu startar man på sida 1 vid omordningen men ordningen håller
		 * genom alla sidor.
		 * 
		 * in:	Ordning - ordningen (upåt eller neråt) för id eller namn
		 *		produktTyp - från drop-down listan av produkter för att filtrera vid
		 *			typ
		 *		sökSträng - från sök fältet vid sökning för att filtrera vid namn
		 *		filterVärde - nyvarande filtrering
		 *		sida - vilken sida man är på (som bestäms är css PagedList)
		 * ut: ActionResult (en vy eller resultat efter kodkörning)
		 */
		public ActionResult Index(string Ordning, string produktTyp, string sökSträng, string filterVärde, int? sida)
		{
			//Spara ordningen just nu
			ViewBag.OrdningNu = Ordning;
			
			//Om ordningen skulle vara null, är det bara tom som är default med
			//namnordning A->Ö
			if (String.IsNullOrEmpty(Ordning)) Ordning = "";

			//default ordningar för första körningen
			ViewBag.NamnOrdning = "";
			ViewBag.IDOrdning = "AscID_Ordning";

			//Om man har just sökt någonting, går tillbaka till sida 1,
			//annars lägger filtervärder i söksträngen
			if (sökSträng != null)
			{
				sida = 1;
			}
			else
			{
				sökSträng = filterVärde;
			}

			//Spara söksträng som filtervärdet
			ViewBag.filterVärde = sökSträng;

			//En lista för att hålla typer för filtrering
			var TypLst = new List<string>();

			//Hämta produkter tabellen från respository som IEnumerable
			var produkter = repository.HämtaProduktlista();

			//En fråga efter alla typer av produkter i typ ordning
			var TypQry = from d in produkter
						 orderby d.Typ
						 select d.Typ;

			//Listan fylls med alla unika typer som finns 
			TypLst.AddRange(TypQry.Distinct());

			//Spara typ listan för att visa upp i valjboxen på formen
			ViewBag.produktTyp = new SelectList(TypLst);

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

			//Hantera ordningen.  Det går fram och tillbaka mot ordningen
			//högst till lägst (eller Ö->A), och lägst till högst (eller A->Ö)
			//Default är A->Ö för namn
			switch (Ordning)
			{
				case "DesNamn_Ordning":
					produkter = produkter.OrderByDescending(n => n.Namn);
					ViewBag.NamnOrdning = "AscNamn_Ordning";
					break;
				case "DesID_Ordning":
					produkter = produkter.OrderByDescending(n => n.ID);
					ViewBag.IDOrdning = "AscID_Ordning";
					break;
				case "AscID_Ordning":
					produkter = produkter.OrderBy(n => n.ID);
					ViewBag.IDOrdning = "DesID_Ordning";
					break;
				default:
					produkter = produkter.OrderBy(n => n.Namn);
					ViewBag.NamnOrdning = "DesNamn_Ordning";
					break;
			}

			//Hur många sidor som visas samtidigt utan att skapa en ny sida
			int antalProdukter = 5;

			//En variabel som PagedList sätter själv efter vilken sida man är på 
			int antalSidor = (sida ?? 1);

			//"Index" i det här fallet är bara en medellande för testning
			//ToPagedList är en speciell tillägg till IEnumerables bland annat
			//mha 'using PagedList och PagedList.Mvc'.  Den kopplar till en
			//css model (ser PagedList.css under Content mappen) som används i
  			//vyn.  Den tar emot sin egen räkning på sidor, och ovan definition
			//av hur många element att visa åt gången.
			return View("Index", produkter.ToPagedList(antalSidor, antalProdukter));
		}

		/*
		 * Details kunde omdöpas.  Det är bara den namn den fick från automatgenering.
		 * Den visar detaljer på en särskild produkt (all data för en rad).
		 * 
		 * in: sträng id som representera en produkt id
		 * ut: ActionResult (en vy eller resultat efter kodkörning)
		 */
		public ActionResult Details(string id)
		{
			//Om inskickade id är null, visa en default HTTP sida för fel
			if (id == null)
			{
				var result = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				return result;
			}

			//Hämta information för rad som matcher id (som är nyckeln)
			Produkter produkter = repository.HämtaProduktMedID(id);

			//Om raden är null (id finns inte), visa en default HTTP sida för
			//att resultatet hittades inte med medellande
			if (produkter == null)
			{
				var result = new HttpNotFoundResult("Kan inte hitta produkten");
				return result;
			}

			//"Details" texten är bara för hjälp vid testning
			//produkter skickas in för att visa för vald produkten i vyn
			return View("Details", produkter);
		}

		/*
		 * Prislista är en variation av Index som visar id, namn, typ och pris för
		 * alla produkter.  Man borde kunna bryta ut lite av det som är gemansamt
		 * till en funktion.
		 * 
		 * in:	Ordning - ordningen (upåt eller neråt) för id, namn, typ, eller pris
		 *		produktTyp - från drop-down listan av produkter för att filtrera vid
		 *			typ
		 *		sökSträng - från sök fältet vid sökning för att filtrera vid namn
		 *		filterVärde - nyvarande filtrering
		 *		sida - vilken sida man är på (som bestäms är css PagedList)
		 * ut: ActionResult (en vy eller resultat efter kodkörning)
		 */
		public ActionResult Prislista(string Ordning, string produktTyp, string sökSträng, string filterVärde, int? sida)
		{
			//Spara ordningen just nu
			ViewBag.OrdningNu = Ordning;

			//Om ordningen skulle vara null, är det bara tom som är default med
			//namnordning A->Ö
			if (String.IsNullOrEmpty(Ordning)) Ordning = "";

			//default ordningar för första körningen
			ViewBag.NamnOrdning = "";
			ViewBag.IDOrdning = "AscID_Ordning";
			ViewBag.TypOrdning = "AscTyp_Ordning";
			ViewBag.PrisOrdning = "AscPris_Ordning";

			//Om man har just sökt någonting, går tillbaka till sida 1,
			//annars lägger filtervärder i söksträngen
			if (sökSträng != null)
			{
				sida = 1;
			}
			else
			{
				sökSträng = filterVärde;
			}

			//Spara söksträng som filtervärdet
			ViewBag.filterVärde = sökSträng;

			//En lista för att hålla typer för filtrering
			var TypLst = new List<string>();

			//Hämta produkter tabellen från respository som IEnumerable
			var produkter = repository.HämtaProduktlista();

			//En fråga efter alla typer av produkter i typ ordning
			var TypQry = from d in produkter
						 orderby d.Typ
						 select d.Typ;

			//Listan fylls med alla unika typer som finns 
			TypLst.AddRange(TypQry.Distinct());

			//Spara typ listan för att visa upp i valjboxen på formen
			ViewBag.produktTyp = new SelectList(TypLst);

			//Gör en lista med alla produkter
			var produktLista = from m in produkter
							   select m;

			//Om där finns en söksträng, filtrera produkter efter namn som innehåller
			//söksträngen
			if (!String.IsNullOrEmpty(sökSträng))
			{
				produkter = produkter.Where(s => s.Namn.Contains(sökSträng));
			}

			//Om där finns en vald produktyp (där All är en speciellfall), filtreras
			//produkter efter vald typ
			if (!string.IsNullOrEmpty(produktTyp))
			{
				produkter = produkter.Where(x => x.Typ == produktTyp);
			}

			//Hantera ordningen.  Det går fram och tillbaka mot ordningen
			//högst till lägst (eller Ö->A), och lägst till högst (eller A->Ö)
			//Default är A->Ö för namn
			switch (Ordning)
			{
				case "DesNamn_Ordning":
					produkter = produkter.OrderByDescending(n => n.Namn);
					ViewBag.NamnOrdning = "";
					break;
				case "DesID_Ordning":
					produkter = produkter.OrderByDescending(n => n.ID);
					ViewBag.IDOrdning = "AscID_Ordning";
					break;
				case "AscID_Ordning":
					produkter = produkter.OrderBy(n => n.ID);
					ViewBag.IDOrdning = "DesID_Ordning";
					break;
				case "AscTyp_Ordning":
					produkter = produkter.OrderBy(n => n.Typ);
					ViewBag.TypOrdning = "DesTyp_Ordning";
					break;
				case "DesTyp_Ordning":
					produkter = produkter.OrderByDescending(n => n.Typ);
					ViewBag.TypOrdning = "AscTyp_Ordning";
					break;
				case "AscPris_Ordning":
					produkter = produkter.OrderBy(n => n.Pris);
					ViewBag.PrisOrdning = "DesPris_Ordning";
					break;
				case "DesPris_Ordning":
					produkter = produkter.OrderByDescending(n => n.Pris);
					ViewBag.PrisOrdning = "AscPris_Ordning";
					break;
				default:
					produkter = produkter.OrderBy(n => n.Namn);
					ViewBag.NamnOrdning = "DesNamn_Ordning";
					break;
			}

			//Hur många sidor som visas samtidigt utan att skapa en ny sida
			int antalProdukter = 5;

			//En variabel som PagedList sätter själv efter vilken sida man är på 
			int antalSidor = (sida ?? 1);

			//"Prislista" i det här fallet är bara en medellande för testning
			//ToPagedList är en speciell tillägg till IEnumerables bland annat
			//mha 'using PagedList och PagedList.Mvc'.  Den kopplar till en
			//css model (ser PagedList.css under Content mappen) som används i
			//vyn.  Den tar emot sin egen räkning på sidor, och ovan definition
			//av hur många element att visa åt gången.
			return View("Prislista", produkter.ToPagedList(antalSidor, antalProdukter));
		}


		//Allt förbi denna punkten är automatgenerat och kommer inte att användas
		//(förutom dispose) och kan tas bort sedan. 

		/*
		// GET: Produkter/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Produkter/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ID,Namn,Pris,Typ,Farg,Bildfilnamn,Ritningsfilnamn,RefID,Beskrivning,Montering")] Produkter produkter)
		{
			if (ModelState.IsValid)
			{
				repository.SkapaProdukt(produkter);
				return RedirectToAction("Index");
			}

			return View(produkter);
		}

		// GET: Produkter/Edit/5
		public ActionResult Edit(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Produkter produkter = repository.HämtaProduktMedID(id);
			if (produkter == null)
			{
				return HttpNotFound();
			}
			return View(produkter);
		}

		// POST: Produkter/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,Namn,Pris,Typ,Farg,Bildfilnamn,Ritningsfilnamn,RefID,Beskrivning,Montering")] Produkter produkter)
		{
			if (ModelState.IsValid)
			{
				repository.RedigeraProdukt(produkter);
				return RedirectToAction("Index");
			}
			return View(produkter);
		}

		// GET: Produkter/Delete/5
		public ActionResult Delete(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Produkter produkter = repository.HämtaProduktMedID(id);
			if (produkter == null)
			{
				return HttpNotFound();
			}
			return View(produkter);
		}

		// POST: Produkter/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(string id)
		{
			repository.TaBortProdukt(id);
			return RedirectToAction("Index");
		}
		*/
		  
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				repository.Förstör();
			}
			base.Dispose(disposing);
		}
	}
}
