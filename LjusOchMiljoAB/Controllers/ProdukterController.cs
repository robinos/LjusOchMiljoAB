using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LjusOchMiljoAB.Models;

namespace LjusOchMiljoAB.Controllers
{
	public class ProdukterController : Controller
	{
		//private LOM_DBEntities db = new LOM_DBEntities();

        IProdukterRepository repository;

        public ProdukterController() : this(new EntityProdukterManagerRepository()) { }
		public ProdukterController(IProdukterRepository repository)
		{
            this.repository = repository;
        }

		public ActionResult Index(string produktTyp, string sökSträng)
		{
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

			return View(produkter);
		}

		// GET: Produkter/Details/5
		public ActionResult Details(string id)
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

		// GET: Produkter/Prislista
		public ActionResult Prislista()
		{
			return View(repository.HämtaProduktlista());
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
