using System.Runtime.Serialization;

namespace InfoTrakMobileService.DataAccess.Entities
{
    [DataContract]
    public class EquipmentEntity
    {
        [DataMember]
        public long EquipmentId { get; set; }

        [DataMember]
        public string EquipmentSerialNo { get; set; }

        [DataMember]
        public string EquipmentUnitNo { get; set; }

        [DataMember]
        public string EquipmentCustomer { get; set; }

        [DataMember]
        public string EquipmentJobsite { get; set; }

        [DataMember]
        public long EquipmentJobsiteAuto { get; set; }

        [DataMember]
        public string EquipmentFamily { get; set; }

        [DataMember]
        public string EquipmentModel { get; set; }

        [DataMember]
        public string EquipmentSMU { get; set; }

        [DataMember]
        public byte[] EquipmentImage { get; set; }

        [DataMember]
        public byte[] EquipmentLocation { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public string CreatedDate { get; set; }

        [DataMember]
        public long EquipmentModelAuto { get; set; }

        [DataMember]
        public string UCSerialLeft { get; set; }

        [DataMember]
        public string UCSerialRight { get; set; }

        [DataMember]
        public long LinksInChain { get; set; }

        [DataMember]
        public long Module_sub_auto { get; set; }
    }
}