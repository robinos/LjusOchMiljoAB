﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LjusOchMiljoAB.Models;
using LjusOchMiljoAB.Controllers;
using PagedList.Mvc;
using PagedList;
using System.Threading.Tasks;

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
	 * HanteraListan - sätter alla temporär variabel i vyn (ViewBag variabler)
	 * och anropar de viktiga metoder i ProduktTjänst för sortering och filtrering
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 09
	 * Version: 0.19
	 */
	[Authorize]
	public class ProduktController : Controller
	{
		//IProduktTjänst hanterar kommunikation med tjänsten som hanterar
		//produkter åt ProdukterController
		public IProduktTjänst produktTjänst;

		//Vid tom konstruktör, gör en ny tjänst av typen som används för
		//verklig körning
		public ProduktController() : this(new ProduktTjänst()) { }

		//En-parameter konstruktör för testning mot en egen tjänst
		public ProduktController(IProduktTjänst produktTjänst)
		{
			this.produktTjänst = produktTjänst;
		}

		/*
		 * Index är det stora produktindexet där alla produkter visas (5 per enligt
		 * definition i ProduktService) men kan filtreras efter namnsökning eller
		 * vald produktkategori och ordnas efter id eller namn. Id, namn, och
		 * bildfilnamn (som kommer att bli en bild sedan) visas för produkter.
		 * Just nu startar man på sida 1 vid omordning men ordningen håller
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
		public async Task<ActionResult> Index(string ordning, string produktTyp, string sökSträng, string filterSträng, string filterProdukt, int? sida)
		{
			//Hämta IEnumerable produkter från tjänsten 
			var produkter = await HanteraListan(ordning, produktTyp, sökSträng, filterSträng, filterProdukt, sida);

			//"Index" i det här fallet är bara en medellande för testning.
			//HämtaSida hämtar sidoinformation och skickar en IPagedList till vyn
			return View("Index", produktTjänst.HämtaSida(produkter, sida));
		}

		/*
		 * Detaljer är bara den namn den fick från automatgenering.
		 * Den visar detaljer på en särskild produkt (all data för en rad).
		 * 
		 * in: sträng id som representera en produkt id
		 * ut: ActionResult (en vy eller resultat efter kodkörning)
		 */
		public async Task<ActionResult> Detaljer(string id)
		{
			//Om inskickade id är null, visa en default HTTP sida för fel
			if (id == null)
			{
				var result = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				return result;
			}

			//Hämta information för rad som matcher id (som är nyckeln)
			Produkt produkt = await produktTjänst.HämtaProduktMedID(id);

			//Om raden är null (id finns inte), visa en default HTTP sida för
			//att resultatet hittades inte med medellande
			if (produkt == null)
			{
				var result = new HttpNotFoundResult("Kan inte hitta produkten");
				return result;
			}

			//"Details" texten är bara för hjälp vid testning
			//produkter skickas in för att visa för vald produkten i vyn
			return View("Detaljer", produkt);
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
		public async Task<ActionResult> Prislista(string ordning, string produktTyp, string sökSträng, string filterSträng, string filterProdukt, int? sida)
		{
			//Hämta IEnumerable produkter från tjänsten 
			var produkter = await HanteraListan(ordning, produktTyp, sökSträng, filterSträng, filterProdukt, sida);

			//"Pristlista" i det här fallet är bara en medellande för testning.
			//HämtaSida hämtar sidoinformation och skickar en IPagedList till vyn
			return View("Prislista", produktTjänst.HämtaSida(produkter, sida));
		}

		/*
		 * HanteraListan sätter alla temporär variabel i vyn (ViewBag variabler)
		 * och anropar de viktiga metoder i ProduktTjänst
		 *		produktTjänst.HämtaProdukter() - hämta alla produkter i databasen
		 *		produktTjänst.HämtaValLista(produkter, produktTyp) - hämta sträng
		 *			listan av alla produkttyper för DropDownList med vald produktTyp
		 *		produktTjänst.HämtaFiltreradProduktlista(produkter, produktTyp,
		 *			sökSträng) - filtrera produktlistan efter val produkt typ och
		 *			söksträngen
		 *		produktTjänst.HämtaOrdnadProduktlista(produkter, ordning) - ordnar
		 *			produktlistan efter ordning
		 * 
		 * in:	ordning - ordning typ (default "" är vid namn A-Ö/0-9)
		 *		produktTyp - vald typ av produkt att visa (default alla)
		 *		sökSträng - namn söksträng att filtrera med (defaul "")
		 *		filterSträng - för att spara sökSträng mellan sidor
		 *		filterProdukt - för att spara produktType mellan sidor
		 *		sida - sida man kommer att visa som int
		 * ut:	Task för async och en IEnumerable av Produkt objekt som är resultatet
		 */
		public async Task<IEnumerable<Produkt>> HanteraListan(string ordning, string produktTyp, string sökSträng, string filterSträng, string filterProdukt, int? sida)
		{
			//Spara ordningen just nu
			ViewBag.OrdningNu = ordning;

			//Om ordningen skulle vara null, är det bara tom som är default med
			//namnordning A->Ö
			if (String.IsNullOrEmpty(ordning)) ordning = "";

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
				sökSträng = filterSträng;
				produktTyp = filterProdukt;
			}

			//Spara söksträng som filtervärdet
			ViewBag.filterSträng = sökSträng;
			ViewBag.filterProdukt = produktTyp;

			//Hämta IEnumerable produkter från tjänsten 
			var produkter = await produktTjänst.HämtaProdukter();

			//Spara typ listan för att visa upp i valjboxen på formen
			ViewBag.produktTyp = produktTjänst.HämtaValLista(produkter, produktTyp);

			//Filtrera listan utefter söksträng och produktTyp
			produkter = produktTjänst.HämtaFiltreradProduktlista(produkter, produktTyp, sökSträng);

			//Ordna listan utefter Ordning strängen
			produkter = produktTjänst.HämtaOrdnadProduktlista(produkter, ordning);

			//Hantera ordningen.  Det går fram och tillbaka mot ordningen
			//högst till lägst (eller Ö->A), och lägst till högst (eller A->Ö)
			//Default är A->Ö för namn
			switch (ordning)
			{
				case "DesNamn_Ordning":
					ViewBag.NamnOrdning = "";
					break;
				case "DesID_Ordning":
					ViewBag.IDOrdning = "AscID_Ordning";
					break;
				case "AscID_Ordning":
					ViewBag.IDOrdning = "DesID_Ordning";
					break;
				case "AscTyp_Ordning":
					ViewBag.TypOrdning = "DesTyp_Ordning";
					break;
				case "DesTyp_Ordning":
					ViewBag.TypOrdning = "AscTyp_Ordning";
					break;
				case "AscPris_Ordning":
					ViewBag.PrisOrdning = "DesPris_Ordning";
					break;
				case "DesPris_Ordning":
					ViewBag.PrisOrdning = "AscPris_Ordning";
					break;
				default:
					ViewBag.NamnOrdning = "DesNamn_Ordning";
					break;
			}

			return produkter;
		}

		/*
		 * Dispose finns för att fria upp minnet när man avslutar.
		 * 
		 * in: disposing är sann om vyn stängs ner och annars falsk
		 */
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				produktTjänst.Förstör();
			}
			base.Dispose(disposing);
		}
	}
}
