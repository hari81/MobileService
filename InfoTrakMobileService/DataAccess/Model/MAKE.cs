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
    
    public partial class MAKE
    {
        public MAKE()
        {
            this.EQUIPMENTs = new HashSet<EQUIPMENT>();
            this.LU_MMTA = new HashSet<LU_MMTA>();
            this.TRACK_COMPART_EXT = new HashSet<TRACK_COMPART_EXT>();
        }
    
        public int make_auto { get; set; }
        public string makeid { get; set; }
        public string makedesc { get; set; }
        public string dbs_id { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public string created_user { get; set; }
        public Nullable<System.DateTime> modified_date { get; set; }
        public string modified_user { get; set; }
        public Nullable<int> cs_make_auto { get; set; }
        public bool cat { get; set; }
        public Nullable<bool> Oil { get; set; }
        public Nullable<bool> Components { get; set; }
        public Nullable<bool> Undercarriage { get; set; }
        public Nullable<bool> Tyre { get; set; }
        public Nullable<bool> Rim { get; set; }
        public Nullable<bool> Hydraulic { get; set; }
        public Nullable<bool> Body { get; set; }
    
        public virtual ICollection<EQUIPMENT> EQUIPMENTs { get; set; }
        public virtual ICollection<LU_MMTA> LU_MMTA { get; set; }
        public virtual ICollection<TRACK_COMPART_EXT> TRACK_COMPART_EXT { get; set; }
    }
}
