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
	public class ProdukterController : Controller
	{
        IProdukterRepository repository;

        public ProdukterController() : this(new EntityProdukterManagerRepository()) { }
		public ProdukterController(IProdukterRepository repository)
		{
            this.repository = repository;
        }

		public ActionResult Index(string Ordning, string produktTyp, string sökSträng, string filterVärde, int? sida)
		{
			ViewBag.OrdningNu = Ordning;
			ViewBag.SidaNu = sida;

			/*if (!String.IsNullOrEmpty(produktTyp)) ViewBag.ProduktTyp = produktTyp;
			if (!String.IsNullOrEmpty(sökSträng)) ViewBag.SökSträng = sökSträng;
			if (String.IsNullOrEmpty(ViewBag.ProduktTyp)) ViewBag.ProduktTyp = "";
			if (String.IsNullOrEmpty(ViewBag.SökSträng)) ViewBag.SökSträng = "";*/
			
			if (String.IsNullOrEmpty(Ordning)) Ordning = "";

			ViewBag.NamnOrdning = "";
			ViewBag.IDOrdning = "AscID_Ordning";

			if (sökSträng != null)
			{
				sida = 1;
			}
			else
			{
				sökSträng = filterVärde;
			}

			ViewBag.filterVärde = sökSträng;

			var TypLst = new List<string>();
			var produkter = repository.HämtaProduktlista();

			var TypQry = from d in produkter
						 orderby d.Typ
						 select d.Typ;

			TypLst.AddRange(TypQry.Distinct());

			ViewBag.produktTyp = new SelectList(TypLst);

			var produktLista = from m in produkter
							select m;

			if (!String.IsNullOrEmpty(sökSträng))
			{
				produkter = produkter.Where(s => s.Namn.ToUpper().Contains(sökSträng.ToUpper()));
			}

			if (!string.IsNullOrEmpty(produktTyp))
			{
				produkter = produkter.Where(x => x.Typ == produktTyp);
				Ordning = "AscTyp_Ordning";
			}

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
				case "AscTyp_Ordning":
					produkter = produkter.OrderBy(n => n.Typ);
					break;
				default:
					produkter = produkter.OrderBy(n => n.Namn);
					ViewBag.NamnOrdning = "DesNamn_Ordning";
					break;
			}

			int antalProdukter = 5;
			int antalSidor = (sida ?? 1);
			return View("Index", produkter.ToPagedList(antalSidor, antalProdukter));
		}

		// GET: Produkter/Details/5
		public ActionResult Details(string id)
		{
			if (id == null)
			{
				var result = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				return result;
			}
			Produkter produkter = repository.HämtaProduktMedID(id);
			if (produkter == null)
			{
				var result = new HttpNotFoundResult("Kan inte hitta produkten");
				return result;
			}
			return View("Details", produkter);
		}

		// GET: Produkter/Prislista
		public ActionResult Prislista(string Ordning, string produktTyp, string sökSträng, string filterVärde, int? sida)
		{
			ViewBag.OrdningNu = Ordning;
			ViewBag.SidaNu = sida;

			/*if (!String.IsNullOrEmpty(produktTyp)) ViewBag.ProduktTyp = produktTyp;
			if (!String.IsNullOrEmpty(sökSträng)) ViewBag.SökSträng = sökSträng;
			if (String.IsNullOrEmpty(ViewBag.ProduktTyp)) ViewBag.ProduktTyp = "";
			if (String.IsNullOrEmpty(ViewBag.SökSträng)) ViewBag.SökSträng = "";*/

			if (String.IsNullOrEmpty(Ordning)) Ordning = "";

			ViewBag.NamnOrdning = "";
			ViewBag.IDOrdning = "AscID_Ordning";
			ViewBag.TypOrdning = "AscTyp_Ordning";
			ViewBag.PrisOrdning = "AscPris_Ordning";

			if (sökSträng != null)
			{
				sida = 1;
			}
			else
			{
				sökSträng = filterVärde;
			}

			ViewBag.filterVärde = sökSträng;

			var TypLst = new List<string>();
			var produkter = repository.HämtaProduktlista();

			var TypQry = from d in produkter
						 orderby d.Typ
						 select d.Typ;

			TypLst.AddRange(TypQry.Distinct());

			ViewBag.produktTyp = new SelectList(TypLst);

			var produktLista = from m in produkter
							   select m;

			if (!String.IsNullOrEmpty(sökSträng))
			{
				produkter = produkter.Where(s => s.Namn.Contains(sökSträng));
			}

			if (!string.IsNullOrEmpty(produktTyp))
			{
				produkter = produkter.Where(x => x.Typ == produktTyp);
			}

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

			int antalProdukter = 5;
			int antalSidor = (sida ?? 1);
			return View("Prislista", produkter.ToPagedList(antalSidor, antalProdukter));
		}

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
