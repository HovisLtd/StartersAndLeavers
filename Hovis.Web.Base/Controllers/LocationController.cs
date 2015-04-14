using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hovis.Web.Base.Models;

namespace Hovis.Web.Base.Controllers
{
    public class LocationController : Controller
    {
        private StarterAndLeaverEntities db = new StarterAndLeaverEntities();

        // GET: Location
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.t_Employee_Location.ToList());
        }

        // GET: Location/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Employee_Location t_Employee_Location = db.t_Employee_Location.Find(id);
            if (t_Employee_Location == null)
            {
                return HttpNotFound();
            }
            return View(t_Employee_Location);
        }

        // GET: Location/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "LocID,LocationDescription")] t_Employee_Location t_Employee_Location)
        {
            if (ModelState.IsValid)
            {
                db.t_Employee_Location.Add(t_Employee_Location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(t_Employee_Location);
        }

        // GET: Location/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Employee_Location t_Employee_Location = db.t_Employee_Location.Find(id);
            if (t_Employee_Location == null)
            {
                return HttpNotFound();
            }
            return View(t_Employee_Location);
        }

        // POST: Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "LocID,LocationDescription")] t_Employee_Location t_Employee_Location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_Employee_Location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(t_Employee_Location);
        }

        // GET: Location/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Employee_Location t_Employee_Location = db.t_Employee_Location.Find(id);
            if (t_Employee_Location == null)
            {
                return HttpNotFound();
            }
            return View(t_Employee_Location);
        }

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            t_Employee_Location t_Employee_Location = db.t_Employee_Location.Find(id);
            db.t_Employee_Location.Remove(t_Employee_Location);
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
