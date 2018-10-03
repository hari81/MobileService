using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace InfoTrakMobileService.DataAccess.Entities
{
    [DataContract]
    public class NewEquipmentEntity
    {
        [DataMember]
        public string _unitno { get; set; }

        [DataMember]
        public string _serialno { get; set; }

        [DataMember]
        public string _customer { get; set; }

        [DataMember]
        public string _jobsite { get; set; }

        [DataMember]
        public string _family { get; set; }

        [DataMember]
        public string _model { get; set; }

        [DataMember]
        public string _smu { get; set; }

        [DataMember]
        public byte[] _location { get; set; }

        [DataMember]
        public byte[] _image { get; set; }

        [DataMember]
        private int _imageRes { get; set; }

        [DataMember]
        public long _equipmentId { get; set; }

        [DataMember]
        public long _jobsiteAuto { get; set; }

        [DataMember]
        public string _status { get; set; }

        [DataMember]
        public int _isNew { get; set; }

        [DataMember]
        public long _customerAuto { get; set; }

        [DataMember]
        public long _modelAuto { get; set; }

        [DataMember]
        public string _examiner { get; set; }

        [DataMember]
        public string _creationDate { get; set; }

        [DataMember]
        public List<EquipmentDetails> _details { get; set; }

        //PRN9455 - This will only have data when adding a new equipment
        [DataMember]
        public UndercarriageInspectionEntity _equipmentInspection { get; set; }

        // TT-49
        [DataMember]
        public string _base64Image { get; set; }
    }

    [DataContract]
    public class EquipmentDetails
    {
        [DataMember]
        public long EqunitAuto { get; set; }

        [DataMember]
        public long CompType { get; set; }

        [DataMember]
        public long CompartIdAuto { get; set; }

        [DataMember]
        public string Compartid { get; set; }

        [DataMember]
        public string Compart { get; set; }

        [DataMember]
        public int Pos { get; set; }

        [DataMember]
        public byte[] Image { get; set; }

        [DataMember]
        public string Side { get; set; }

        [DataMember]
        public long EquipmentidAuto { get; set; }

        [DataMember]
        public string FlangeType { get; set; }

        [DataMember]
        public string Reading { get; set; }

        [DataMember]
        public string Tool { get; set; }

        [DataMember]
        public string Method { get; set; }

        [DataMember]
        public string Comments { get; set; }

        [DataMember]
        public String InspectionImage { get; set; }

    }
}