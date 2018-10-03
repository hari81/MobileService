using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace InfoTrakMobileService.DataAccess.Entities
{
    [DataContract]
    public class ImageEntity
    {
        [DataMember]
        public string _logoName { get; set; }

        [DataMember]
        public string _logo { get; set; }
    }
    
}