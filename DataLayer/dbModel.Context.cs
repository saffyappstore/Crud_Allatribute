﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UserEntities : DbContext
    {
        public UserEntities()
            : base("name=UserEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tbl_city> tbl_city { get; set; }
        public virtual DbSet<tbl_user> tbl_user { get; set; }
        public virtual DbSet<ColumnSetting> ColumnSettings { get; set; }
        public virtual DbSet<ColumnUsersSetting> ColumnUsersSettings { get; set; }
        public virtual DbSet<cmatrix_contact_form_template_details> cmatrix_contact_form_template_details { get; set; }
        public virtual DbSet<cmatrix_contact> cmatrix_contact { get; set; }
        public virtual DbSet<cmatrix_contact_form_data> cmatrix_contact_form_data { get; set; }
        public virtual DbSet<cmatrix_student_enrollment_section> cmatrix_student_enrollment_section { get; set; }
        public virtual DbSet<cmatrix_student_form_data> cmatrix_student_form_data { get; set; }
        public virtual DbSet<cmatrix_student_form_template_details> cmatrix_student_form_template_details { get; set; }
    }
}
