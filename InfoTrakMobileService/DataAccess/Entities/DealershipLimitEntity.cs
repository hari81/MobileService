using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace InfoTrakMobileService.DataAccess.Entities
{
    [DataContract]
    public class DealershipLimitEntity
    {
        [DataMember]
        public int? ALimit { get; set; }

        [DataMember]
        public int? BLimit { get; set; }

        [DataMember]
        public int? CLimit { get; set; }
    }
}