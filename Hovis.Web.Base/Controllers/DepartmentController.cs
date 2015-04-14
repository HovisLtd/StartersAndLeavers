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
    public class DepartmentController : Controller
    {
        private StarterAndLeaverEntities db = new StarterAndLeaverEntities();

        // GET: Department
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.t_Employee_Department.ToList());
        }

        // GET: Department/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Employee_Department t_Employee_Department = db.t_Employee_Department.Find(id);
            if (t_Employee_Department == null)
            {
                return HttpNotFound();
            }
            return View(t_Employee_Department);
        }

        // GET: Department/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "DeptID,DeptDescription")] t_Employee_Department t_Employee_Department)
        {
            if (ModelState.IsValid)
            {
                db.t_Employee_Department.Add(t_Employee_Department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(t_Employee_Department);
        }

        // GET: Department/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Employee_Department t_Employee_Department = db.t_Employee_Department.Find(id);
            if (t_Employee_Department == null)
            {
                return HttpNotFound();
            }
            return View(t_Employee_Department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "DeptID,DeptDescription")] t_Employee_Department t_Employee_Department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_Employee_Department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(t_Employee_Department);
        }

        // GET: Department/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Employee_Department t_Employee_Department = db.t_Employee_Department.Find(id);
            if (t_Employee_Department == null)
            {
                return HttpNotFound();
            }
            return View(t_Employee_Department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            t_Employee_Department t_Employee_Department = db.t_Employee_Department.Find(id);
            db.t_Employee_Department.Remove(t_Employee_Department);
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
