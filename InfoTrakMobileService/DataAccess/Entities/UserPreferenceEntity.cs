using System.Runtime.Serialization;

namespace InfoTrakMobileService.DataAccess.Entities
{

     [DataContract]
    public class UserPreferenceEntity
    {

         [DataMember]
         public string UndercarriagUOM { get; set; } 

    }
}