using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hovis.Web.Base.Models
{
    public class StarterMetaData
    {
        [Display(Name = "Ref No")]
        public int ID { get; set; }

        [Display(Name = "Date First Requested")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateFirstRequested { get; set; }

        [Display(Name = "Raised By")]
        public string RaisedBy { get; set; }

        [Display(Name = "Telephone No")]
        public string RequestorsTelephoneNumber { get; set; }

        [Display(Name = "Requestors Location")]
        public string RequestorsLocation { get; set; }

        [Required]
        [Display(Name = "New Starters Name")]
        public string NewStartersName { get; set; }

        [Display(Name = "New Starters Role")]
        public string NewStartersRole { get; set; }

        [Display(Name = "Employment Type")]
        public int EmploymentType { get; set; }

        [Display(Name = "New Starter Department")]
        public int NewStarterDept { get; set; }

        [Display(Name = "New starter Location")]
        public int NewStarterLocation { get; set; }

        [Display(Name = "New Starter Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> NewStarterDate { get; set; }

        [Required]
        [Display(Name = "Managers Name")]
        public string ManagersName { get; set; }

        [Display(Name = "Department Cost Center")]
        public int DeptCostCentre { get; set; }

        [Display(Name = "Email Required")]
        public string GmailRequired { get; set; }

        [Display(Name = "Laptop Required")]
        public string LaptopRequired { get; set; }

        [Display(Name = "Mobile Required")]
        public string MobileRequired { get; set; }

        [Display(Name = "Remote Access (VPN)")]
        public string RemoteAccessVPN { get; set; }

        [Display(Name = "Confirmed Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> CreationDate { get; set; }

        [Display(Name = "Confirmed")]
        public string CreationConfirmed { get; set; }

        [Display(Name = "Confirmed By")]
        public string CreationConfirmedBy { get; set; }
    }

    public class RequestMetaData
    {
        [Display(Name = "Ref No")]
        public int ID { get; set; }

        [Display(Name = "Date First Requested")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateFirstRequested { get; set; }

        [Display(Name = "Requestors Name")]
        public string RequestorsName { get; set; }

        [Display(Name = "Telephone No")]
        public string RequestorsTelephoneNumber { get; set; }

        [Display(Name = "Location")]
        public int RequestorsLocation { get; set; }

        [Display(Name = "Requirements")]
        public string Requirements { get; set; }

        [Display(Name = "Confirmed")]
        public string RequestConfirmed { get; set; }

        [Display(Name = "Confirmed Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> RequestDate { get; set; }
    }

    public class LeaversMetaData
    {
        [Display(Name = "Ref No")]
        public int ID { get; set; }

        [Display(Name = "Date First Requested")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateFirstRequested { get; set; }

        [Display(Name = "Requestors Name")]
        public string RequestorsName { get; set; }

        [Display(Name = "Telephone No")]
        public string RequestorsTelephoneNumber { get; set; }

        [Display(Name = "Location")]
        public int RequestorsLocation { get; set; }

        [Display(Name = "Leavers Name")]
        public string LeaversName { get; set; }

        [Display(Name = "Confirmed")]
        public string DeleteConfirmed { get; set; }

        [Display(Name = "Confirmed Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DeleteDate { get; set; }
    }

        public class ActiveDirectoryMetaData
    {
        [Display(Name = "Email")]
        public int email { get; set; }

        [Display(Name = "Location")]
        public string physicaldeliveryoffice { get; set; }

        [Display(Name = "Telephone No")]
        public string telephonenumber { get; set; }

        [Display(Name = "Department")]
        public int department { get; set; }

        [Display(Name = "Leavers Name")]
        public string samaccountname { get; set; }

        [Display(Name = "Managers Email")]
        public string ManagerEmail { get; set; }

    }
}