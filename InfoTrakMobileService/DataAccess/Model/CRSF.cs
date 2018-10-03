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
    
    public partial class CRSF
    {
        public long crsf_auto { get; set; }
        public long customer_auto { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public string created_user { get; set; }
        public Nullable<System.DateTime> modified_date { get; set; }
        public string modified_user { get; set; }
        public string site_name { get; set; }
        public string site_street { get; set; }
        public string site_street2 { get; set; }
        public string site_suburb { get; set; }
        public string site_postcode { get; set; }
        public string site_state { get; set; }
        public string site_country { get; set; }
        public Nullable<bool> priority { get; set; }
        public Nullable<int> branch_auto { get; set; }
        public Nullable<long> cs_jobsite_auto { get; set; }
        public Nullable<int> site_country_auto { get; set; }
        public Nullable<bool> hold_report_until_paid { get; set; }
        public Nullable<short> hold_invoice_days { get; set; }
        public Nullable<bool> schedule_sample_kit { get; set; }
        public Nullable<int> type_auto { get; set; }
        public int DealerId { get; set; }
        public Nullable<long> CreatedByUserId { get; set; }
        public string FullAddress { get; set; }
    
        public virtual CUSTOMER CUSTOMER { get; set; }
        public virtual USER_TABLE USER_TABLE { get; set; }
    }
}
