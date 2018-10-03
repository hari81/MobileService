using System.Runtime.Serialization;

namespace InfoTrakMobileService.DataAccess.Entities
{
    [DataContract]
    public class CustomerEntity
    {
        [DataMember]
        public long CustomerId { get; set; }

        [DataMember]
        public string CustomerName { get; set; }
    }
}