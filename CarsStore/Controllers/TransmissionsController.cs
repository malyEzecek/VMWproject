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
    public class TransmissionsController : Controller
    {
        private CarsSHopEntities db = new CarsSHopEntities();

        // GET: Transmissions
        public ActionResult Index()
        {
            return View(db.Transmissions.ToList());
        }

        // GET: Transmissions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transmission transmission = db.Transmissions.Find(id);
            if (transmission == null)
            {
                return HttpNotFound();
            }
            return View(transmission);
        }

        // GET: Transmissions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transmissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_trans,Name_trans")] Transmission transmission)
        {
            if (ModelState.IsValid)
            {
                db.Transmissions.Add(transmission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(transmission);
        }

        // GET: Transmissions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transmission transmission = db.Transmissions.Find(id);
            if (transmission == null)
            {
                return HttpNotFound();
            }
            return View(transmission);
        }

        // POST: Transmissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_trans,Name_trans")] Transmission transmission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transmission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transmission);
        }

        // GET: Transmissions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transmission transmission = db.Transmissions.Find(id);
            if (transmission == null)
            {
                return HttpNotFound();
            }
            return View(transmission);
        }

        // POST: Transmissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transmission transmission = db.Transmissions.Find(id);
            db.Transmissions.Remove(transmission);
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
