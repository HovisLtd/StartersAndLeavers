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
    public class StarterController : Controller
    {
        private StarterAndLeaverEntities db = new StarterAndLeaverEntities();

        // GET: Starter
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Index(int? page)
        {
            var t_New_Starter_Details = db.t_New_Starter_Details.Include(t => t.t_Employee_Location).Include(t => t.t_Employee_Department).Include(t => t.t_Employment_Type);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(t_New_Starter_Details.OrderByDescending(c => c.ID).ToPagedList(pageNumber, pageSize));
        }

        // GET: Starter/Details/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_New_Starter_Details t_New_Starter_Details = db.t_New_Starter_Details.Find(id);
            if (t_New_Starter_Details == null)
            {
                return HttpNotFound();
            }
            return View(t_New_Starter_Details);
        }

        // GET: Starter/Create
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Create()
        {

            var model = new t_New_Starter_Details();
            model.RaisedBy = User.Identity.Name;
            //Lets try to look up some of the requestors details
            string StrLocation = "";
            string StrDepartment = "";
            string StrTelNo = "";
            int IntLocation = 1;
            int IntDepartment = 1;

            //Look up details from AD table 
            IQueryable<v_AD_Users_Lookup> requestordetails = db.v_AD_Users_Lookup
                .Where(x => x.email == model.RaisedBy);
            requestordetails = requestordetails.Take(1);
            foreach (var item in requestordetails)
            {
                StrLocation = Convert.ToString(item.physicaldeliveryoffice);
                StrDepartment = Convert.ToString(item.department);
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

            // Now find the Department number rather than text
            IQueryable<t_Employee_Department> requestordepartment = db.t_Employee_Department
                .Where(x => x.DeptDescription == StrDepartment);
            requestordepartment = requestordepartment.Take(1);
            foreach (var depts in requestordepartment)
            {
                IntDepartment = depts.DeptID;
            }

            // Pass some default values to the view
            model.RequestorsLocation = IntLocation;
            model.NewStarterLocation = IntLocation;
            model.NewStarterDept = IntDepartment;
            model.RequestorsTelephoneNumber = StrTelNo;
            model.GmailRequired = "No";
            model.LaptopRequired = "No";
            model.MobileRequired = "No";
            model.RemoteAccessVPN = "No";
            model.CreationConfirmed = "No";
            model.DateFirstRequested = DateTime.Now;
            model.RequestorsName = User.Identity.Name;

            ViewBag.NewStarterLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", model.NewStarterLocation);
            ViewBag.RequestorsLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", model.RequestorsLocation);
            ViewBag.NewStarterDept = new SelectList(db.t_Employee_Department, "DeptID", "DeptDescription", model.NewStarterDept);
            ViewBag.EmploymentType = new SelectList(db.t_Employment_Type, "TypeID", "TypeDescription", model.EmploymentType);
            return View(model);
        }

        // POST: Starter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Create([Bind(Include = "ID,DateFirstRequested,RequestorsName,RaisedBy,RequestorsTelephoneNumber,RequestorsLocation,NewStartersName,NewStartersRole,EmploymentType,NewStarterDept,NewStarterLocation,NewStarterDate,ManagersName,DeptCostCentre,GmailRequired,LaptopRequired,MobileRequired,RemoteAccessVPN,CreationConfirmed,CreationConfirmedBy,CreationDate")] t_New_Starter_Details t_New_Starter_Details)
        {
            if (ModelState.IsValid)
            {
                if (t_New_Starter_Details.GmailRequired == null)
                {
                    t_New_Starter_Details.GmailRequired = "No";
                }
                if (t_New_Starter_Details.LaptopRequired == null)
                {
                    t_New_Starter_Details.LaptopRequired = "No";
                }
                if (t_New_Starter_Details.MobileRequired == null)
                {
                    t_New_Starter_Details.MobileRequired = "No";
                }
                if (t_New_Starter_Details.RemoteAccessVPN == null)
                {
                    t_New_Starter_Details.RemoteAccessVPN = "No";
                }
                if (t_New_Starter_Details.CreationConfirmed == null)
                {
                    t_New_Starter_Details.CreationConfirmed = "No";
                }
                db.t_New_Starter_Details.Add(t_New_Starter_Details);
                db.SaveChanges();

                string StrLocation = "";
                string StrDepartment = "";
                string StrEmptype = "";

                // Now find the location number rather than text
                IQueryable<t_Employee_Location> requestorlocations = db.t_Employee_Location
                    .Where(x => x.LocID == t_New_Starter_Details.NewStarterLocation);
                requestorlocations = requestorlocations.Take(1);
                foreach (var locs in requestorlocations)
                {
                    StrLocation = locs.LocationDescription;
                }

                // Now find the Department number rather than text
                IQueryable<t_Employee_Department> requestordepartment = db.t_Employee_Department
                    .Where(x => x.DeptID == t_New_Starter_Details.NewStarterDept);
                requestordepartment = requestordepartment.Take(1);
                foreach (var depts in requestordepartment)
                {
                    StrDepartment = depts.DeptDescription;
                }

                // Now find the Department number rather than text
                IQueryable<t_Employment_Type> employmenttype = db.t_Employment_Type
                    .Where(x => x.TypeID == t_New_Starter_Details.EmploymentType);
                employmenttype = employmenttype.Take(1);
                foreach (var emps in employmenttype)
                {
                    StrEmptype = emps.TypeDescription;
                }

                //Build up email info to send
                string Subject = "New Starter Request : " + t_New_Starter_Details.NewStartersName;
                var sb = new StringBuilder();
                sb.AppendLine("Please create this new starter : " + t_New_Starter_Details.NewStartersName);
                sb.AppendLine("Start Date : " + String.Format("{0:dd/MM/yyy}", t_New_Starter_Details.NewStarterDate));
                sb.AppendLine("New starters role : " + t_New_Starter_Details.NewStartersRole);
                sb.AppendLine("Department : " + StrDepartment);
                sb.AppendLine("Location : " + StrLocation);
                sb.AppendLine("Employment type : " + StrEmptype);
                sb.AppendLine("Managers Name : " + t_New_Starter_Details.ManagersName);
                sb.AppendLine("Cost Center : " + t_New_Starter_Details.DeptCostCentre);
                sb.AppendLine("");
                sb.AppendLine("Requested By : " + t_New_Starter_Details.RequestorsName);
                sb.AppendLine("Tel No : " + t_New_Starter_Details.RequestorsTelephoneNumber);
                sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("Email Required? : " + t_New_Starter_Details.GmailRequired);
                sb.AppendLine("Laptop Required? : " + t_New_Starter_Details.LaptopRequired);
                sb.AppendLine("Mobile Required? : " + t_New_Starter_Details.MobileRequired);
                sb.AppendLine("Remote Access (VPN)? : " + t_New_Starter_Details.RemoteAccessVPN);


                string FromSetting = "StarterFromAddress";
                string ToSetting = "starterToAddress";
                return RedirectToAction("SendNewMail", "CodeHelper", new { ReturnController = "Starter", ReturnAction = "Index", FromSetting = FromSetting, ToSetting = ToSetting, EmailBody = sb.ToString(), EmailSubject = Subject });

                //return RedirectToAction("Index");
            }

            return View(t_New_Starter_Details);
        }



        // GET: Starter/Edit/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_New_Starter_Details t_New_Starter_Details = db.t_New_Starter_Details.Find(id);
            if (t_New_Starter_Details == null)
            {
                return HttpNotFound();
            }
            ViewBag.NewStarterLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", t_New_Starter_Details.NewStarterLocation);
            ViewBag.RequestorsLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", t_New_Starter_Details.RequestorsLocation);
            ViewBag.NewStarterDept = new SelectList(db.t_Employee_Department, "DeptID", "DeptDescription", t_New_Starter_Details.NewStarterDept);
            ViewBag.EmploymentType = new SelectList(db.t_Employment_Type, "TypeID", "TypeDescription", t_New_Starter_Details.EmploymentType);
            return View(t_New_Starter_Details);
        }

        // GET: Starter/Close/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Close(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_New_Starter_Details t_New_Starter_Details = db.t_New_Starter_Details.Find(id);
            if (t_New_Starter_Details == null)
            {
                return HttpNotFound();
            }
            t_New_Starter_Details.CreationConfirmed = "Yes";
            t_New_Starter_Details.CreationConfirmedBy = User.Identity.Name;
            t_New_Starter_Details.CreationDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(t_New_Starter_Details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(t_New_Starter_Details);
        }

        // POST: Starter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Edit([Bind(Include = "ID,DateFirstRequested,RequestorsName,RaisedBy,RequestorsTelephoneNumber,RequestorsLocation,NewStartersName,NewStartersRole,EmploymentType,NewStarterDept,NewStarterLocation,NewStarterDate,ManagersName,DeptCostCentre,GmailRequired,LaptopRequired,MobileRequired,RemoteAccessVPN,CreationConfirmed,CreationConfirmedBy,CreationDate")] t_New_Starter_Details t_New_Starter_Details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_New_Starter_Details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(t_New_Starter_Details);
        }



        // GET: Starter/Delete/5
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_New_Starter_Details t_New_Starter_Details = db.t_New_Starter_Details.Find(id);
            if (t_New_Starter_Details == null)
            {
                return HttpNotFound();
            }
            return View(t_New_Starter_Details);
        }

        // POST: Starter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,STRcanEdit")]
        public ActionResult DeleteConfirmed(long id)
        {
            t_New_Starter_Details t_New_Starter_Details = db.t_New_Starter_Details.Find(id);
            db.t_New_Starter_Details.Remove(t_New_Starter_Details);
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
