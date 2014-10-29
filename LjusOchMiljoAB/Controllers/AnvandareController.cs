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
    public class AnvandareController : Controller
    {
        private LOM_DBEntities db = new LOM_DBEntities();

        // GET: Anvandare
        public ActionResult Index()
        {
            return View(db.Anvandare.ToList());
        }

        // GET: Anvandare/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anvandare anvandare = db.Anvandare.Find(id);
            if (anvandare == null)
            {
                return HttpNotFound();
            }
            return View(anvandare);
        }

        // GET: Anvandare/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Anvandare/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Anvandarnamn,Losenord")] Anvandare anvandare)
        {
            if (ModelState.IsValid)
            {
                db.Anvandare.Add(anvandare);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(anvandare);
        }

        // GET: Anvandare/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anvandare anvandare = db.Anvandare.Find(id);
            if (anvandare == null)
            {
                return HttpNotFound();
            }
            return View(anvandare);
        }

        // POST: Anvandare/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Anvandarnamn,Losenord")] Anvandare anvandare)
        {
            if (ModelState.IsValid)
            {
                db.Entry(anvandare).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anvandare);
        }

        // GET: Anvandare/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anvandare anvandare = db.Anvandare.Find(id);
            if (anvandare == null)
            {
                return HttpNotFound();
            }
            return View(anvandare);
        }

        // POST: Anvandare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Anvandare anvandare = db.Anvandare.Find(id);
            db.Anvandare.Remove(anvandare);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
