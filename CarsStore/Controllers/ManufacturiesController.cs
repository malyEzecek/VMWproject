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
    public class ManufacturiesController : Controller
    {
        private CarsSHopEntities db = new CarsSHopEntities();

        // GET: Manufacturies
        public ActionResult Index()
        {
            return View(db.Manufacturies.ToList());
        }

        // GET: Manufacturies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufactury manufactury = db.Manufacturies.Find(id);
            if (manufactury == null)
            {
                return HttpNotFound();
            }
            return View(manufactury);
        }

        // GET: Manufacturies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manufacturies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_man,Name_man,Descrip")] Manufactury manufactury)
        {
            if (ModelState.IsValid)
            {
                db.Manufacturies.Add(manufactury);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manufactury);
        }

        // GET: Manufacturies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufactury manufactury = db.Manufacturies.Find(id);
            if (manufactury == null)
            {
                return HttpNotFound();
            }
            return View(manufactury);
        }

        // POST: Manufacturies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_man,Name_man,Descrip")] Manufactury manufactury)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manufactury).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manufactury);
        }

        // GET: Manufacturies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufactury manufactury = db.Manufacturies.Find(id);
            if (manufactury == null)
            {
                return HttpNotFound();
            }
            return View(manufactury);
        }

        // POST: Manufacturies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manufactury manufactury = db.Manufacturies.Find(id);
            db.Manufacturies.Remove(manufactury);
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
