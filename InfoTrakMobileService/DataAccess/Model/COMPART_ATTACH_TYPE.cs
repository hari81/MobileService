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
    
    public partial class COMPART_ATTACH_TYPE
    {
        public COMPART_ATTACH_TYPE()
        {
            this.COMPART_ATTACH_FILESTREAM = new HashSet<COMPART_ATTACH_FILESTREAM>();
        }
    
        public int compart_attach_type_auto { get; set; }
        public string compart_attach_type_name { get; set; }
    
        public virtual ICollection<COMPART_ATTACH_FILESTREAM> COMPART_ATTACH_FILESTREAM { get; set; }
    }
}
