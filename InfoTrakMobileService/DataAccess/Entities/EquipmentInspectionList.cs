using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace InfoTrakMobileService.DataAccess.Entities
{
    [DataContract]
    public class EquipmentInspectionListEntity
    {
        
        [DataMember(Name="_equipmentsInspection")]
        public List<UndercarriageInspectionEntity> EquipmentsInspectionsList { get; set; } //For existing Equipments 

        [DataMember(Name="_newEquipments")]
        public List<NewEquipmentEntity> NewEquipmentsInspectionsList { get; set; } //For New Equipments 

        [DataMember(Name = "_totalEquipments")]
        public int EquipmentsCount { get; set; }
    }

   
}