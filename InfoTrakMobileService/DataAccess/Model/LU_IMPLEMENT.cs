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
    
    public partial class LU_IMPLEMENT
    {
        public LU_IMPLEMENT()
        {
            this.GETs = new HashSet<GET>();
            this.LU_COMPART_TYPE = new HashSet<LU_COMPART_TYPE>();
        }
    
        public long implement_auto { get; set; }
        public string implementdescription { get; set; }
        public Nullable<long> parentID { get; set; }
        public string schematic_auto_multiple { get; set; }
        public int implement_category_auto { get; set; }
        public Nullable<long> CustomerId { get; set; }
    
        public virtual ICollection<GET> GETs { get; set; }
        public virtual ICollection<LU_COMPART_TYPE> LU_COMPART_TYPE { get; set; }
        public virtual CUSTOMER CUSTOMER { get; set; }
    }
}
