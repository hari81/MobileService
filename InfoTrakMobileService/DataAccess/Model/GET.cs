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
    
    public partial class GET
    {
        public int get_auto { get; set; }
        public string impserial { get; set; }
        public Nullable<long> implement_auto { get; set; }
        public long equipmentid_auto { get; set; }
        public Nullable<bool> isinuse { get; set; }
        public Nullable<int> make_auto { get; set; }
        public string makeid { get; set; }
        public Nullable<long> installsmu { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public string created_user { get; set; }
        public Nullable<System.DateTime> modified_date { get; set; }
        public string modified_user { get; set; }
        public Nullable<short> get_linkage_system_auto { get; set; }
        public Nullable<decimal> bucket_width { get; set; }
        public Nullable<decimal> base_edge_thickness { get; set; }
        public Nullable<decimal> eb_bottom_thickness { get; set; }
        public Nullable<decimal> eb_side_thickness { get; set; }
        public Nullable<decimal> cutting_edge_thickness { get; set; }
        public Nullable<decimal> mb_bottom_thickness { get; set; }
        public Nullable<decimal> mb_rear_thickness { get; set; }
        public string bucket_width_uom { get; set; }
        public string base_edge_thickness_uom { get; set; }
        public Nullable<int> num { get; set; }
        public Nullable<decimal> segment_length { get; set; }
        public Nullable<int> plates_width { get; set; }
        public Nullable<int> plates_length { get; set; }
        public Nullable<int> plates_thickness { get; set; }
        public Nullable<int> strips_width { get; set; }
        public Nullable<int> strips_length { get; set; }
        public Nullable<int> strips_thickness { get; set; }
        public Nullable<long> feet_type_auto { get; set; }
        public string num_feet { get; set; }
        public Nullable<decimal> start_height { get; set; }
        public Nullable<decimal> end_height { get; set; }
        public Nullable<System.Guid> image_guid { get; set; }
        public Nullable<long> impsetup_hours { get; set; }
        public bool on_equipment { get; set; }
    
        public virtual EQUIPMENT EQUIPMENT { get; set; }
        public virtual LU_IMPLEMENT LU_IMPLEMENT { get; set; }
    }
}
