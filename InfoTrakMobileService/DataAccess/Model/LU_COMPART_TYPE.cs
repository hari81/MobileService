//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InfoTrakMobileService.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class LU_COMPART_TYPE
    {
        public LU_COMPART_TYPE()
        {
            this.LU_COMPART = new HashSet<LU_COMPART>();
        }
    
        public int comparttype_auto { get; set; }
        public string comparttypeid { get; set; }
        public string comparttype { get; set; }
        public Nullable<int> sorder { get; set; }
        public bool @protected { get; set; }
        public Nullable<long> modified_user_auto { get; set; }
        public Nullable<System.DateTime> modified_date { get; set; }
        public Nullable<long> implement_auto { get; set; }
        public Nullable<bool> multiple { get; set; }
        public Nullable<int> max_no { get; set; }
        public Nullable<byte> progid { get; set; }
        public Nullable<int> fixedamount { get; set; }
        public Nullable<int> min_no { get; set; }
        public Nullable<bool> getmesurement { get; set; }
        public Nullable<short> system_auto { get; set; }
        public Nullable<int> cs_comparttype_auto { get; set; }
        public Nullable<long> standard_compart_type_auto { get; set; }
        public string comparttype_shortkey { get; set; }
    
        public virtual ICollection<LU_COMPART> LU_COMPART { get; set; }
        public virtual LU_SYSTEM LU_SYSTEM { get; set; }
        public virtual LU_IMPLEMENT LU_IMPLEMENT { get; set; }
    }
}