using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarsStore.Models;

namespace CarsStore.Controllers
{
    public class EnginesController : Controller
    {
        private CarsSHopEntities db = new CarsSHopEntities();

        // GET: Engines
        public ActionResult Index()
        {
            return View(db.Engines.ToList());
        }

        // GET: Engines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Engine engine = db.Engines.Find(id);
            if (engine == null)
            {
                return HttpNotFound();
            }
            return View(engine);
        }

        // GET: Engines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Engines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_eng,Type")] Engine engine)
        {
            if (ModelState.IsValid)
            {
                db.Engines.Add(engine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(engine);
        }

        // GET: Engines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Engine engine = db.Engines.Find(id);
            if (engine == null)
            {
                return HttpNotFound();
            }
            return View(engine);
        }

        // POST: Engines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_eng,Type")] Engine engine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(engine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(engine);
        }

        // GET: Engines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Engine engine = db.Engines.Find(id);
            if (engine == null)
            {
                return HttpNotFound();
            }
            return View(engine);
        }

        // POST: Engines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Engine engine = db.Engines.Find(id);
            db.Engines.Remove(engine);
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
