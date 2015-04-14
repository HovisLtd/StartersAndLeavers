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
    public class EmploymentTypeController : Controller
    {
        private StarterAndLeaverEntities db = new StarterAndLeaverEntities();

        // GET: EmploymentType
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.t_Employment_Type.ToList());
        }

        // GET: EmploymentType/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Employment_Type t_Employment_Type = db.t_Employment_Type.Find(id);
            if (t_Employment_Type == null)
            {
                return HttpNotFound();
            }
            return View(t_Employment_Type);
        }

        // GET: EmploymentType/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmploymentType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "TypeID,TypeDescription")] t_Employment_Type t_Employment_Type)
        {
            if (ModelState.IsValid)
            {
                db.t_Employment_Type.Add(t_Employment_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(t_Employment_Type);
        }

        // GET: EmploymentType/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Employment_Type t_Employment_Type = db.t_Employment_Type.Find(id);
            if (t_Employment_Type == null)
            {
                return HttpNotFound();
            }
            return View(t_Employment_Type);
        }

        // POST: EmploymentType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "TypeID,TypeDescription")] t_Employment_Type t_Employment_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_Employment_Type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(t_Employment_Type);
        }

        // GET: EmploymentType/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Employment_Type t_Employment_Type = db.t_Employment_Type.Find(id);
            if (t_Employment_Type == null)
            {
                return HttpNotFound();
            }
            return View(t_Employment_Type);
        }

        // POST: EmploymentType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            t_Employment_Type t_Employment_Type = db.t_Employment_Type.Find(id);
            db.t_Employment_Type.Remove(t_Employment_Type);
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
