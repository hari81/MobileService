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
    
    public partial class TRACK_COMPART_WORN_CALC_METHOD
    {
        public TRACK_COMPART_WORN_CALC_METHOD()
        {
            this.TRACK_COMPART_EXT = new HashSet<TRACK_COMPART_EXT>();
        }
    
        public int track_compart_worn_calc_method_auto { get; set; }
        public string track_compart_worn_calc_method_name { get; set; }
    
        public virtual ICollection<TRACK_COMPART_EXT> TRACK_COMPART_EXT { get; set; }
    }
}
