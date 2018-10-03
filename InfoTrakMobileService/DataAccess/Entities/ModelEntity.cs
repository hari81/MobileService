using System.Runtime.Serialization;

namespace InfoTrakMobileService.DataAccess.Entities
{
    [DataContract]
    public class ModelEntity
    {
        [DataMember]
        public long ModelId { get; set; }

        [DataMember]
        public string ModelName { get; set; }
    }
}