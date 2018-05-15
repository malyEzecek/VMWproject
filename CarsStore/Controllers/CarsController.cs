using System;
using System.IO;
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
    public class CarsController : Controller
    {
        private CarsSHopEntities db = new CarsSHopEntities();

        // GET: Cars
        public ViewResult Index(string name)
        {
            var cars = (from c in db.Cars select c).ToList();
            if ( !String.IsNullOrEmpty(name))
            {
                cars = cars.Where(c => c.Name_car.Contains(name)).ToList();
            }

            List<Int16?> queries = new List<Int16?>();
            queries.Add(null);
            queries.Add(null);
            queries.Add(null);
            queries.Add(null);
            queries.Add(null);
            queries.Add(null);
            queries.Add(null);

            string path = @"C:\Users\Julia\Downloads\words_sorted.txt";

            using (StreamReader sr = new StreamReader(path))
            {
                
                while (sr.Peek() >= 0)
                {
                    var timer = System.Diagnostics.Stopwatch.StartNew();

                    string word = sr.ReadLine().ToString();
                    var list = cars.Where(c => c.Name_car.Contains(word)).ToList();
                    System.Diagnostics.Debug.WriteLine(word);

                    switch (queries[0])
                    {
                        case 0:
                            list = cars.Where(car => car.Price < 50000).ToList();
                            break;
                        case 1:
                            list = cars.Where(car => car.Price < 70000).ToList();
                            break;
                        case 2:
                            list = cars.Where(car => car.Price < 90000).ToList();
                            break;
                        case 3:
                            list = cars.Where(car => car.Price < 100000).ToList();
                            break;
                        case 4:
                            list = cars.Where(car => car.Price > 100000).ToList();
                            break;
                    }

                    switch (queries[1])
                    {
                        case 0:
                            list = cars.Where(car => car.Consumption <= 5).ToList();
                            break;
                        case 1:
                            list = cars.Where(car => car.Consumption <= 7).ToList();
                            break;
                        case 2:
                            list = cars.Where(car => car.Consumption <= 9).ToList();
                            break;
                    }
                    switch (queries[2])
                    {
                        case 0:
                            list = cars.Where(car => car.Doors == 2).ToList();
                            break;
                        case 1:
                            list = cars.Where(car => car.Doors == 3).ToList();
                            break;
                        case 2:
                            list = cars.Where(car => car.Doors == 4).ToList();
                            break;
                        case 3:
                            list = cars.Where(car => car.Doors == 5).ToList();
                            break;
                    }
                    switch (queries[3])
                    {
                        case 0:
                            list = cars.Where(car => car.Cub_capacity <= 1000).ToList();
                            break;
                        case 1:
                            list = cars.Where(car => car.Cub_capacity <= 1350).ToList();
                            break;
                        case 2:
                            list = cars.Where(car => car.Cub_capacity <= 1850).ToList();
                            break;
                        case 3:
                            list = cars.Where(car => car.Cub_capacity <= 2500).ToList();
                            break;
                        case 4:
                            list.Where(car => car.Cub_capacity > 2500).ToList();
                            break;
                    }
                    switch (queries[4])
                    {
                        case 0:
                            list = cars.Where(car => car.Seats <= 3).ToList();
                            break;
                        case 1:
                            list = cars.Where(car => car.Seats == 5 || car.Seats == 4).ToList();
                            break;
                        case 2:
                            list = cars.Where(car => car.Seats == 6 || car.Seats == 7).ToList();
                            break;
                    }

                    switch (queries[5])
                    {
                        case 0:
                            list = cars.Where(car => car.Achievement >= 70 && car.Achievement <= 85).ToList();
                            break;
                        case 1:
                            list = cars.Where(car => car.Achievement >= 85 && car.Achievement <= 100).ToList();
                            break;
                        case 2:
                            list = cars.Where(car => car.Achievement >= 100 && car.Achievement <= 125).ToList();
                            break;
                    }
                    timer.Stop();
                    var elapsed = timer.Elapsed;

                    System.Diagnostics.Debug.WriteLine("\n\n\nAmount of searching/filtering results : " + list.Count);
                    System.Diagnostics.Debug.WriteLine("Time of searching/filtering results : " + elapsed.ToString("mm':'ss':'fff"));
                }
            }
            
            /*switch (queries[0])
            {
                case 0:
                    cars = cars.Where(car => car.Price < 50000).ToList();
                    break;
                case 1:
                    cars = cars.Where(car => car.Price < 70000).ToList();
                    break;
                case 2:
                    cars = cars.Where(car => car.Price < 90000).ToList();
                    break;
                case 3:
                    cars = cars.Where(car => car.Price < 100000).ToList();
                    break;
                case 4:
                    cars = cars.Where(car => car.Price > 100000).ToList();
                    break;
            }

            switch (queries[1])
            {
                case 0:
                    cars = cars.Where( car => car.Consumption <= 5 ).ToList();
                    break;
                case 1:
                    cars = cars.Where(car => car.Consumption <= 7).ToList();
                    break;
                case 2:
                    cars = cars.Where(car => car.Consumption <= 9).ToList();
                    break;
            }
            switch (queries[2])
            {
                case 0:
                    cars = cars.Where(car => car.Doors == 2).ToList();
                    break;
                case 1:
                    cars = cars.Where(car => car.Doors == 3).ToList();
                    break;
                case 2:
                    cars = cars.Where(car => car.Doors == 4).ToList();
                    break;
                case 3:
                    cars = cars.Where(car => car.Doors == 5).ToList();
                    break;
            }
            switch (queries[3])
            {
                case 0:
                    cars = cars.Where(car => car.Cub_capacity <= 1000).ToList();
                    break;
                case 1:
                    cars = cars.Where(car => car.Cub_capacity <= 1350).ToList();
                    break;
                case 2:
                    cars = cars.Where(car => car.Cub_capacity <= 1850).ToList();
                    break;
                case 3:
                    cars.Where(car => car.Cub_capacity <= 2500).ToList();
                    break;
                case 4:
                    cars.Where(car => car.Cub_capacity > 2500).ToList();
                    break;
            }
            switch (queries[4])
            {
                case 0:
                    cars = cars.Where(car => car.Seats <= 3).ToList();
                    break;
                case 1:
                    cars = cars.Where(car => car.Seats == 5 || car.Seats == 4).ToList();
                    break;
                case 2:
                    cars = cars.Where(car => car.Seats == 6 || car.Seats == 7).ToList();
                    break;
            }

            switch (queries[5])
            {
                case 0:
                    cars = cars.Where(car => car.Achievement >= 70 && car.Achievement <= 85).ToList();
                    break;
                case 1:
                    cars = cars.Where(car => car.Achievement >= 85 && car.Achievement <= 100).ToList();
                    break;
                case 2:
                    cars = cars.Where(car => car.Achievement >= 100 && car.Achievement <= 125).ToList();
                    break;
            }
            */
           // timer.Stop();
           // var elapsed = timer.Elapsed;

           // System.Diagnostics.Debug.WriteLine("\n\n\nAmount of searching/filtering results : " + cars.Count);
           // System.Diagnostics.Debug.WriteLine("Time of searching/filtering results : " + elapsed.ToString("mm':'ss':'fff"));

           // ViewBag.TotalAmount = cars.Count + " elements";
           // ViewBag.TimeOfSearching = elapsed.ToString("mm':'ss':'fff");

            if (cars.Count > 100)
            {
                return View("../Cars/Result");
            }
                
            return View(cars);
        }
        

        // GET: Cars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_car,Name_car,Price,Consumption,Doors,Achievement,Cub_capacity,Seats,Delivery,Navigation,Climatisation,ABS,ESP,Leather_interior,Xenons")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_car,Name_car,Price,Consumption,Doors,Achievement,Cub_capacity,Seats,Delivery,Navigation,Climatisation,ABS,ESP,Leather_interior,Xenons")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
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
