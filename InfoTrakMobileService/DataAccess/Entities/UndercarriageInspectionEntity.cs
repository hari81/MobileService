using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace InfoTrakMobileService.DataAccess.Entities
{
    [DataContract]
    public class UndercarriageInspectionEntity
    {
        [DataMember]
        public long EquipmentIdAuto { get; set; }

        [DataMember]
        public String Examiner { get; set; }

        [DataMember]
        public String InspectionDate { get; set; }

        [DataMember]
        public String SMU { get; set; }

        [DataMember]
        public int Impact { get; set; }

        [DataMember]
        public int Abrasive { get; set; }

        [DataMember]
        public int Moisture { get; set; }

        [DataMember]
        public int Packing { get; set; }

        [DataMember]
        public decimal TrackSagLeft { get; set; }

        [DataMember]
        public decimal TrackSagRight { get; set; }

        [DataMember]
        public int DryJointsLeft { get; set; }

        [DataMember]
        public int DryJointsRight { get; set; }

        [DataMember]
        public decimal ExtCannonLeft { get; set; }

        [DataMember]
        public decimal ExtCannonRight { get; set; }

        [DataMember]
        public String JobsiteComments { get; set; }

        [DataMember]
        public String InspectorComments { get; set; }

        [DataMember]
        public List<InspectionDetails> Details { get; set; }
        [DataMember]
        public String leftTrackSagComment { get; set; }
        [DataMember]
        public String rightTrackSagComment { get; set; }
        [DataMember]
        public String leftCannonExtComment { get; set; }
        [DataMember]
        public String rightCannonExtComment { get; set; }
        [DataMember]
        public String leftDryJointsComment { get; set; }
        [DataMember]
        public String rightDryJointsComment { get; set; }
        [DataMember]
        public String leftScallopComment { get; set; }
        [DataMember]
        public String rightScallopComment { get; set; }

        [DataMember]
        public String leftTrackSagImage { get; set; }
        [DataMember]
        public String rightTrackSagImage { get; set; }
        [DataMember]
        public String leftCannonExtImage { get; set; }
        [DataMember]
        public String rightCannonExtImage { get; set; }
        [DataMember]
        public String leftDryJointsImage { get; set; }
        [DataMember]
        public String rightDryJointsImage { get; set; }
        [DataMember]
        public String leftScallopImage { get; set; }
        [DataMember]
        public String rightScallopImage { get; set; }
        [DataMember]
        public bool travelledByKms { get; set; }
        [DataMember]
        public int travelForward { get; set; }  // hours
        [DataMember]
        public int travelReverse { get; set; }  // hours
        [DataMember]
        public int travelForwardKm { get; set; }
        [DataMember]
        public int travelReverseKm { get; set; }
        [DataMember]
        public decimal leftScallop { get; set; }
        [DataMember]
        public decimal rightScallop { get; set; }
        [DataMember]
        public int TrammingHours { get; set; }
        [DataMember]
        public String CustomerContact { get; set; }
        [DataMember]
        public String EquipmentImage { get; set; }
    }

    [DataContract]
    public class InspectionDetails
    {
        [DataMember]
        public long CompartIdAuto { get; set; }

        [DataMember]
        public long TrackUnitAuto { get; set; }

        [DataMember]
        public String Reading { get; set; }

        [DataMember]
        public int PercentageWorn { get; set; }

        [DataMember]
        public String ToolUsed { get; set; }

        [DataMember]
        public string Comments { get; set; }

        [DataMember]
        public String Image { get; set; }

        [DataMember]
        public int AttachmentType { get; set; }

        [DataMember]
        public string FlangeType { get; set; }

        [DataMember]
        public string InspectionImage { get; set; }
    }
}