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
    public class StandardSettingsController : Controller
    {
        private StarterAndLeaverEntities db = new StarterAndLeaverEntities();

        // GET: StandardSettings
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.t_Application_Standard_Settings.ToList());
        }

        // GET: StandardSettings/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Application_Standard_Settings t_Application_Standard_Settings = db.t_Application_Standard_Settings.Find(id);
            if (t_Application_Standard_Settings == null)
            {
                return HttpNotFound();
            }
            return View(t_Application_Standard_Settings);
        }

        // GET: StandardSettings/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: StandardSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,Setting,SettingValue")] t_Application_Standard_Settings t_Application_Standard_Settings)
        {
            if (ModelState.IsValid)
            {
                db.t_Application_Standard_Settings.Add(t_Application_Standard_Settings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(t_Application_Standard_Settings);
        }

        // GET: StandardSettings/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Application_Standard_Settings t_Application_Standard_Settings = db.t_Application_Standard_Settings.Find(id);
            if (t_Application_Standard_Settings == null)
            {
                return HttpNotFound();
            }
            return View(t_Application_Standard_Settings);
        }

        // POST: StandardSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,Setting,SettingValue")] t_Application_Standard_Settings t_Application_Standard_Settings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_Application_Standard_Settings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(t_Application_Standard_Settings);
        }

        // GET: StandardSettings/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Application_Standard_Settings t_Application_Standard_Settings = db.t_Application_Standard_Settings.Find(id);
            if (t_Application_Standard_Settings == null)
            {
                return HttpNotFound();
            }
            return View(t_Application_Standard_Settings);
        }

        // POST: StandardSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            t_Application_Standard_Settings t_Application_Standard_Settings = db.t_Application_Standard_Settings.Find(id);
            db.t_Application_Standard_Settings.Remove(t_Application_Standard_Settings);
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
