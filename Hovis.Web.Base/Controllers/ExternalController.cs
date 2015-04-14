using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hovis.Web.Base.Models;
using System.Text;
using PagedList;
using System.Net.Mail;
using System.Net;

namespace Hovis.Web.Base.Controllers
{
    public class ExternalController : Controller
    {

        private StarterAndLeaverEntities db = new StarterAndLeaverEntities();

        // GET: External
        public ActionResult Index()
        {
            return View();
        }

        // GET: Starter/Create
        public ActionResult CreateStarter(string status)
        {

            if (status == "Success")
            {
                ViewBag.Status = "Success";
            }
            else
            {
                ViewBag.Status = "";
            }
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
        public ActionResult CreateStarter([Bind(Include = "ID,DateFirstRequested,RequestorsName,RaisedBy,RequestorsTelephoneNumber,RequestorsLocation,NewStartersName,NewStartersRole,EmploymentType,NewStarterDept,NewStarterLocation,NewStarterDate,ManagersName,DeptCostCentre,GmailRequired,LaptopRequired,MobileRequired,RemoteAccessVPN,CreationConfirmed,CreationConfirmedBy,CreationDate")] t_New_Starter_Details t_New_Starter_Details)
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
                return RedirectToAction("SendNewMail", "CodeHelper", new { ReturnController = "External", ReturnAction = "CreateStarter", FromSetting = FromSetting, ToSetting = ToSetting, EmailBody = sb.ToString(), EmailSubject = Subject });

                //return RedirectToAction("Index");
            }
            ViewBag.NewStarterLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", t_New_Starter_Details.NewStarterLocation);
            ViewBag.RequestorsLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", t_New_Starter_Details.RequestorsLocation);
            ViewBag.NewStarterDept = new SelectList(db.t_Employee_Department, "DeptID", "DeptDescription", t_New_Starter_Details.NewStarterDept);
            ViewBag.EmploymentType = new SelectList(db.t_Employment_Type, "TypeID", "TypeDescription", t_New_Starter_Details.EmploymentType);
            return View(t_New_Starter_Details);
        }

        // GET: Request/Create
        public ActionResult CreateRequest(string status)
        {

            if (status == "Success")
            {
                ViewBag.Status = "Success";
            }
            else
            {
                ViewBag.Status = "";
            }
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
        public ActionResult CreateRequest([Bind(Include = "ID,DateFirstRequested,RequestorsName,RequestorsTelephoneNumber,RequestorsLocation,Requirements,RequestConfirmed,RequestDate")] t_Servicedesk_Contact t_Servicedesk_Contact)
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
                return RedirectToAction("SendNewMail", "CodeHelper", new { ReturnController = "External", ReturnAction = "CreateRequest", FromSetting = FromSetting, ToSetting = ToSetting, EmailBody = sb.ToString(), EmailSubject = Subject });

            }
            ViewBag.RequestorsLocation = new SelectList(db.t_Employee_Location, "LocID", "LocationDescription", t_Servicedesk_Contact.RequestorsLocation);
            return View(t_Servicedesk_Contact);
        }

        // GET: SearchDetails
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

        // GET: Starter/Close/5
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
            return RedirectToAction("SendNewMail", "CodeHelper", new { ReturnController = "External", ReturnAction = "IndexSearch", FromSetting = FromSetting, ToSetting = ToSetting, EmailBody = sb.ToString(), EmailSubject = Subject });

        }

    }
}