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
    
    public partial class Mbl_Track_Inspection_Detail
    {
        public int inspection_detail_auto { get; set; }
        public int inspection_auto { get; set; }
        public long track_unit_auto { get; set; }
        public Nullable<int> tool_auto { get; set; }
        public decimal reading { get; set; }
        public decimal worn_percentage { get; set; }
        public string eval_code { get; set; }
        public Nullable<int> hours_on_surface { get; set; }
        public Nullable<int> projected_hours { get; set; }
        public Nullable<int> ext_projected_hours { get; set; }
        public Nullable<int> remaining_hours { get; set; }
        public Nullable<int> ext_remaining_hours { get; set; }
        public string comments { get; set; }
        public decimal worn_percentage_120 { get; set; }
    }
}
