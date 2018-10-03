using System.Runtime.Serialization;

namespace InfoTrakMobileService.DataAccess.Entities
{
    [DataContract]
    public class TestPointImageEntity
    {
        [DataMember]
        public long CompartType { get; set; }

        [DataMember]
        public string Tool { get; set; }

        [DataMember]
        public byte[] TestPointImage { get; set; }
    }
}