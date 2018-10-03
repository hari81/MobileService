using System;
using System.Runtime.Serialization;

namespace InfoTrakMobileService.DataAccess.Entities
{
    [DataContract]
    public class ComponentEntity
    {
        [DataMember]
        public long ComponentId { get; set; }

        [DataMember]
        public long ComponentType { get; set; }

        [DataMember]
        public long EquipmentId { get; set; }

        [DataMember]
        public string PartNo { get; set; }

        [DataMember]
        public string ComponentName { get; set; }

        [DataMember]
        public string ComponentSide { get; set; }

        [DataMember]
        public int? ComponentPosition { get; set; }

        [DataMember]
        public byte[] ComponentImage { get; set; }

        [DataMember]
        public String DefaultTool { get; set; }

        [DataMember]
        public String ComponentMethod { get; set; }

        [DataMember]
        public long ComponentIdAuto { get; set; }

        [DataMember]
        public double LastReading { get; set; }
        
		[DataMember]
        public int LastWornPercentage { get; set; }
        
		[DataMember]
        public int ToolId { get; set; }
		
        [DataMember]
        public string ToolSymbol { get; set; }

        [DataMember]
        public long ModuleSubAuto { get; set; }
    }
}