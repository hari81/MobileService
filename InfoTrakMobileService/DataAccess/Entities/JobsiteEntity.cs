using System.Runtime.Serialization;

namespace InfoTrakMobileService.DataAccess.Entities
{
    [DataContract]
    public class JobsiteEntity
    {
        [DataMember]
        public long JobsiteId { get; set; }

        [DataMember]
        public string JobsiteName { get; set; }
    }
}