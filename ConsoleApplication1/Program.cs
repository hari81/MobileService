using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.ServiceReference1;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            EquipmentInspectionListEntity obj = new EquipmentInspectionListEntity();

            List<InspectionDetails> insList = new List<InspectionDetails>();
            InspectionDetails insObj = new InspectionDetails();
            insObj.Comments = "Comments";
            insObj.CompartIdAuto = 1234;
            insObj.Reading = "100";

            insList.Add(insObj);

            UndercarriageInspectionEntity obj1 = new UndercarriageInspectionEntity();
            List<UndercarriageInspectionEntity> obj1List = new List<UndercarriageInspectionEntity>();

            obj1.Abrasive = 0;
            obj1.Examiner = "Riya";
            obj1.Details = insList.ToArray();

            obj1List.Add(obj1);
            obj.EquipmentsCount = 1;
            obj.EquipmentsInspectionsList = obj1List.ToArray();


            MobileServiceClient _c = new MobileServiceClient("test", "http://localhost:53781/MobileService.svc");
            
            _c.SaveEquipmentsInspectionsData(obj);
        }
    }
}
