//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class cmatrix_student_enrollment_section
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cmatrix_student_enrollment_section()
        {
            this.cmatrix_student_form_template_details = new HashSet<cmatrix_student_form_template_details>();
        }
    
        public int cses_key { get; set; }
        public string cses_name { get; set; }
        public Nullable<int> cses_cmp_key { get; set; }
        public Nullable<bool> cses_isdeleted { get; set; }
        public Nullable<int> cses_order { get; set; }
        public Nullable<int> cses_enum { get; set; }
        public Nullable<int> cses_cab_type { get; set; }
        public Nullable<bool> cses_is_active { get; set; }
        public Nullable<bool> cses_is_editable { get; set; }
        public Nullable<bool> cses_is_allow_add_controls { get; set; }
        public Nullable<bool> cses_is_show_enable_icon { get; set; }
        public Nullable<bool> cses_is_deleteable { get; set; }
        public Nullable<int> cses_ceef_key { get; set; }
        public Nullable<int> cses_app_sync_key { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cmatrix_student_form_template_details> cmatrix_student_form_template_details { get; set; }
    }
}
