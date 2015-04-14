using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hovis.Web.Base.Models;
using PagedList;
using System.Text;

namespace Hovis.Web.Base.Controllers
{
    public class RequestController : Controller
    {
        private StarterAndLeaverEntities db = new StarterAndLeaverEntities();

        // GET: Request
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Index(int? page)
        {
            var t_Servicedesk_Contact = db.t_Servicedesk_Contact.Include(t => t.t_Employee_Location);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(t_Servicedesk_Contact.OrderByDescending(c => c.ID).ToPagedList(pageNumber, pageSize));
        }

        // GET: Request/Details/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Servicedesk_Contact t_Servicedesk_Contact = db.t_Servicedesk_Contact.Find(id);
            if (t_Servicedesk_Contact == null)
            {
                return HttpNotFound();
            }
            return View(t_Servicedesk_Contact);
        }

        // GET: Request/Create
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Create()
        {
            var model = new t_Servicedesk_Contact();
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
            model.RequestConfirmed = "No";
            model.DateFirstRequested = DateTime.Now;
            model.RequestorsName = User.Identity.Name;

            ViewBag.RequestorsLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", model.RequestorsLocation);
            return View(model);
        }

        // POST: Request/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Create([Bind(Include = "ID,DateFirstRequested,RequestorsName,RequestorsTelephoneNumber,RequestorsLocation,Requirements,RequestConfirmed,RequestDate")] t_Servicedesk_Contact t_Servicedesk_Contact)
        {
            if (ModelState.IsValid)
            {
                db.t_Servicedesk_Contact.Add(t_Servicedesk_Contact);
                db.SaveChanges();
                string StrLocation = "";

                IQueryable<t_Employee_Location> requestorlocations = db.t_Employee_Location
                            .Where(x => x.LocID == t_Servicedesk_Contact.RequestorsLocation);
                requestorlocations = requestorlocations.Take(1);
                foreach (var locs in requestorlocations)
                {
                    StrLocation = locs.LocationDescription;
                }

                //Build up email info to send
                string Subject = "New Request from : " + t_Servicedesk_Contact.RequestorsName;
                var sb = new StringBuilder();
                sb.AppendLine("Requested By : " + t_Servicedesk_Contact.RequestorsName);
                sb.AppendLine("Tel No : " + t_Servicedesk_Contact.RequestorsTelephoneNumber);
                sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("Details : " + t_Servicedesk_Contact.Requirements);

                string FromSetting = "RequestFromAddress";
                string ToSetting = "RequestToAddress";
                return RedirectToAction("SendNewMail", "CodeHelper", new { ReturnController = "Request", ReturnAction = "Index", FromSetting = FromSetting, ToSetting = ToSetting, EmailBody = sb.ToString(), EmailSubject = Subject });

            }
            ViewBag.RequestorsLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", t_Servicedesk_Contact.RequestorsLocation);
            return View(t_Servicedesk_Contact);
        }

        // GET: Request/Edit/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Servicedesk_Contact t_Servicedesk_Contact = db.t_Servicedesk_Contact.Find(id);
            if (t_Servicedesk_Contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequestorsLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", t_Servicedesk_Contact.RequestorsLocation);
            return View(t_Servicedesk_Contact);
        }

        // GET: Starter/Close/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Close(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Servicedesk_Contact t_Servicedesk_Contact = db.t_Servicedesk_Contact.Find(id);
            if (t_Servicedesk_Contact == null)
            {
                return HttpNotFound();
            }
            t_Servicedesk_Contact.RequestConfirmed = "Yes";
            //t_Servicedesk_Contact.CreationConfirmedBy = User.Identity.Name;
            t_Servicedesk_Contact.RequestDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(t_Servicedesk_Contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RequestorsLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", t_Servicedesk_Contact.RequestorsLocation);
            return View(t_Servicedesk_Contact);
        }


        // POST: Request/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Edit([Bind(Include = "ID,DateFirstRequested,RequestorsName,RequestorsTelephoneNumber,RequestorsLocation,Requirements,RequestConfirmed,RequestDate")] t_Servicedesk_Contact t_Servicedesk_Contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_Servicedesk_Contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RequestorsLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", t_Servicedesk_Contact.RequestorsLocation);
            return View(t_Servicedesk_Contact);
        }

        // GET: Request/Delete/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_Servicedesk_Contact t_Servicedesk_Contact = db.t_Servicedesk_Contact.Find(id);
            if (t_Servicedesk_Contact == null)
            {
                return HttpNotFound();
            }
            return View(t_Servicedesk_Contact);
        }

        // POST: Request/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult DeleteConfirmed(long id)
        {
            t_Servicedesk_Contact t_Servicedesk_Contact = db.t_Servicedesk_Contact.Find(id);
            db.t_Servicedesk_Contact.Remove(t_Servicedesk_Contact);
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
