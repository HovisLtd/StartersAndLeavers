﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hovis.Web.Base.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class StarterAndLeaverEntities : DbContext
    {
        public StarterAndLeaverEntities()
            : base("name=StarterAndLeaverEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<t_Employee_Department> t_Employee_Department { get; set; }
        public virtual DbSet<t_Employee_Department_Redundant> t_Employee_Department_Redundant { get; set; }
        public virtual DbSet<t_Employee_Location> t_Employee_Location { get; set; }
        public virtual DbSet<t_Employee_Location_Redundant> t_Employee_Location_Redundant { get; set; }
        public virtual DbSet<t_Employment_Type> t_Employment_Type { get; set; }
        public virtual DbSet<t_Leaver_Details> t_Leaver_Details { get; set; }
        public virtual DbSet<t_New_Starter_Details> t_New_Starter_Details { get; set; }
        public virtual DbSet<t_Servicedesk_Contact> t_Servicedesk_Contact { get; set; }
        public virtual DbSet<v_AD_Users_Lookup> v_AD_Users_Lookup { get; set; }
        public virtual DbSet<t_Application_Standard_Settings> t_Application_Standard_Settings { get; set; }
    }
}
