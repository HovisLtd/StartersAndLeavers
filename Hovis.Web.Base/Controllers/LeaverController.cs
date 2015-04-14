using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hovis.Web.Base.Models;
using Hovis.Web.Base.Controllers;
using PagedList;
using System.Net.Mail;
using System.Text;

namespace Hovis.Web.Base.Controllers
{
    public class LeaverController : Controller
    {
        private StarterAndLeaverEntities db = new StarterAndLeaverEntities();

        // GET: Leaver
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Index(int? page)
        {
            var t_Leaver_Details = db.t_Leaver_Details.Include(t => t.t_Employee_Location);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(t_Leaver_Details.OrderByDescending(c => c.ID).ToPagedList(pageNumber, pageSize));
        }

        // GET: SearchDetails
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult IndexSearch(string currentFilter, string Searchfield, int? page)
        {

            var sfield = Convert.ToString(Searchfield);
            var cFilter = Convert.ToString(currentFilter);

            if (sfield != null)
            {
                page = 1;
            }
            else
            {
                sfield = cFilter;
            }

            ViewBag.CurrentFilter = sfield;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(db.v_AD_Users_Lookup
            .Where(x => x.email.Contains(sfield) || x.physicaldeliveryoffice.Contains(sfield) || x.department.Contains(sfield) || x.samaccountname.Contains(sfield) || x.ManagerEmail.Contains(sfield))
            .OrderBy(x => x.email).ThenBy(x => x.physicaldeliveryoffice).ToPagedList(pageNumber, pageSize));
        }


        // GET: Leaver/Details/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Leaver_Details t_Leaver_Details = db.t_Leaver_Details.Find(id);
            if (t_Leaver_Details == null)
            {
                return HttpNotFound();
            }
            return View(t_Leaver_Details);
        }

        // GET: Leaver/Create
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leaver/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Create([Bind(Include = "ID,DateFirstRequested,RequestorsName,RequestorsTelephoneNumber,RequestorsLocation,LeaversName,DeleteConfirmed,DeleteDate")] t_Leaver_Details t_Leaver_Details)
        {
            if (ModelState.IsValid)
            {
                db.t_Leaver_Details.Add(t_Leaver_Details);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(t_Leaver_Details);
        }

        // GET: Leaver/Edit/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Leaver_Details t_Leaver_Details = db.t_Leaver_Details.Find(id);
            if (t_Leaver_Details == null)
            {
                return HttpNotFound();
            }
            return View(t_Leaver_Details);
        }

        // POST: Leaver/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Edit([Bind(Include = "ID,DateFirstRequested,RequestorsName,RequestorsTelephoneNumber,RequestorsLocation,LeaversName,DeleteConfirmed,DeleteDate")] t_Leaver_Details t_Leaver_Details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_Leaver_Details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(t_Leaver_Details);
        }

        // GET: Starter/Close/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Close(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Leaver_Details t_Leaver_Details = db.t_Leaver_Details.Find(id);
            if (t_Leaver_Details == null)
            {
                return HttpNotFound();
            }
            t_Leaver_Details.DeleteConfirmed = "Yes";
            //t_Servicedesk_Contact.CreationConfirmedBy = User.Identity.Name;
            t_Leaver_Details.DeleteDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(t_Leaver_Details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RequestorsLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", t_Leaver_Details.RequestorsLocation);
            return View(t_Leaver_Details);
        }

               // GET: Starter/Close/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Flag(string samaccountname)
        {
            if (samaccountname == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new t_Leaver_Details();
            model.DateFirstRequested = DateTime.Now;
            model.LeaversName = samaccountname;
            model.RequestorsName = User.Identity.Name;
            //Lets try to look up some of the requestors details
            string StrLocation = "";
            string StrTelNo = "";
            int IntLocation = 1;

            //Look up details from AD table 
            IQueryable<v_AD_Users_Lookup> requestordetails = db.v_AD_Users_Lookup
                .Where(x => x.email == model.RequestorsName);
            requestordetails = requestordetails.Take(1);
            foreach (var item in requestordetails)
            {
                StrLocation = Convert.ToString(item.physicaldeliveryoffice);
                StrTelNo = Convert.ToString(item.mobileNumber);
            }

            // Now find the location number rather than text
            IQueryable<t_Employee_Location> requestorlocations = db.t_Employee_Location
                .Where(x => x.LocationDescription == StrLocation);
            requestorlocations = requestorlocations.Take(1);
            foreach (var locs in requestorlocations)
            {
                IntLocation = locs.LocID;
            }

            // Pass some default values to the view
            model.RequestorsLocation = IntLocation;
            model.RequestorsTelephoneNumber = StrTelNo;
            model.DeleteConfirmed = "No";
            db.t_Leaver_Details.Add(model);
            db.SaveChanges();

            //Build up email info to send
            string Subject = "New leaver Request : " + model.LeaversName;
            var sb = new StringBuilder();
            sb.AppendLine("This person is leaving : " + model.LeaversName);
            sb.AppendLine("");
            sb.AppendLine("Requested By : " + model.RequestorsName);
            sb.AppendLine("");
            sb.AppendLine("Tel No : " + model.RequestorsTelephoneNumber);

            string FromSetting = "LeaverFromAddress";
            string ToSetting = "LeaverToAddress";
            return RedirectToAction("SendNewMail", "CodeHelper", new { ReturnController = "Leaver", ReturnAction = "Index", FromSetting = FromSetting, ToSetting = ToSetting, EmailBody = sb.ToString(), EmailSubject = Subject });

        }

        // GET: Leaver/Delete/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Leaver_Details t_Leaver_Details = db.t_Leaver_Details.Find(id);
            if (t_Leaver_Details == null)
            {
                return HttpNotFound();
            }
            return View(t_Leaver_Details);
        }

        // POST: Leaver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult DeleteConfirmed(long id)
        {
            t_Leaver_Details t_Leaver_Details = db.t_Leaver_Details.Find(id);
            db.t_Leaver_Details.Remove(t_Leaver_Details);
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
