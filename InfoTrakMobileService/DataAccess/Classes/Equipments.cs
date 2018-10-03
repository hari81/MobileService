using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Hosting;
using System.Data.Entity.Core.Objects;
using InfoTrakMobileService.DataAccess.Entities;
using InfoTrakMobileService.DataAccess.Model;
using System.Transactions;
using BLL.Extensions;
using System.Data.SqlClient;

namespace InfoTrakMobileService.DataAccess.Classes
{
    public class Equipments
    {
        private static readonly Equipments InstanceEquipment = new Equipments();

        private Equipments()
        {
        }

        public static Equipments Instance
        {
            get { return InstanceEquipment; }
        }

        /// <summary>
        /// Gets a list of equipment for the given Jobsite and Model. This is used in the Equipment Search screen for the Mobile App.
        /// </summary>
        /// <param name="jobsiteAuto">Selected Jobsite auto number</param>
        /// <param name="modelAuto">Selected Model auto number</param>
        /// <returns>List of EquipmentEntity with the results</returns>
        public List<EquipmentEntity> GetEquipmentByJobsiteAndModel(long jobsiteAuto, int modelAuto)
        {
            var result = new List<EquipmentEntity>();
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var equipments = from equipment in dataEntities.EQUIPMENTs
                                 join mmta in dataEntities.LU_MMTA on equipment.mmtaid_auto equals mmta.mmtaid_auto
                                 join type in dataEntities.TYPEs on mmta.type_auto equals type.type_auto
                                 where equipment.crsf_auto == jobsiteAuto
                                 select new { equipment.equipmentid_auto, equipment.serialno, equipment.unitno, mmta.model_auto, type.typeid };

                if (modelAuto > 0)
                    equipments = equipments.Where(item => item.model_auto == modelAuto);

                result.AddRange(
                    equipments.Select(
                        equipment =>
                            new EquipmentEntity
                            {
                                EquipmentId = equipment.equipmentid_auto,
                                EquipmentSerialNo = equipment.serialno,
                                EquipmentUnitNo = equipment.unitno,
                                EquipmentFamily = equipment.typeid
                            }));
            }
            return result;
        }


        public List<BLL.Core.WSRE.Models.WSREChainEquipmentModel> GetEquipmentByJobsiteAndSystem(long jobsiteAuto, string system)
        {

            var result = new List<BLL.Core.WSRE.Models.WSREChainEquipmentModel>();

            var systemTypeEnumIndex = 1;
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var equipmentData = dataEntities.Database.SqlQuery<BLL.Core.WSRE.Models.WSREChainEquipmentModel>(
                    "select * from LU_MODULE_SUB module"
                        + " inner join MODEL model on model.model_auto = module.model_auto"
                        + " where module.crsf_auto = @jobsiteAuto"
                        + " and module.systemTypeEnumIndex = " + systemTypeEnumIndex
                    , new SqlParameter("@jobsiteAuto", jobsiteAuto)
                ).ToList();

                foreach (var item in equipmentData)
                {
                    var equipment = new BLL.Core.WSRE.Models.WSREChainEquipmentModel();
                    equipment.equipmentid_auto = item.equipmentid_auto;
                    equipment.Serialno = item.Serialno;
                    equipment.make_auto = item.make_auto;
                    equipment.systemTypeEnumIndex = item.systemTypeEnumIndex;
                    equipment.crsf_auto = jobsiteAuto;
                    equipment.model_auto = item.model_auto;
                    equipment.LinksInChain = item.LinksInChain;
                    equipment.Module_sub_auto = item.Module_sub_auto;
                    result.Add(equipment);
                }
            }

            return result;
        }

        /// <summary>
        /// Get all equipment details for the selected equipment. This is used in the Equipment Search screen for the Mobile App.
        /// </summary>
        /// <param name="equipmentList">Comma separated equipment auto list</param>
        /// <returns>List of EquipmentEntity for the selected equipment</returns>
        public List<EquipmentEntity> GetSelectedEquipment(String equipmentList)
        {
            var result = new List<EquipmentEntity>();
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var equipmentArray = equipmentList.Split(',');
                var equipments = from equipment in dataEntities.EQUIPMENTs
                                 join crsf in dataEntities.CRSFs on equipment.crsf_auto equals crsf.crsf_auto
                                 join cust in dataEntities.CUSTOMERs on crsf.customer_auto equals cust.customer_auto
                                 join mmta in dataEntities.LU_MMTA on equipment.mmtaid_auto equals mmta.mmtaid_auto
                                 join family in dataEntities.TYPEs on mmta.type_auto equals family.type_auto
                                 join model in dataEntities.MODELs on mmta.model_auto equals model.model_auto
                                 where equipmentArray.Contains(equipment.equipmentid_auto.ToString())
                                 select
                                     new
                                     {
                                         equipment.equipmentid_auto,
                                         equipment.serialno,
                                         equipment.unitno,
                                         equipment.currentsmu,
                                         crsf.site_name,
                                         cust.cust_name,
                                         family.typeid,
                                         model.modelid,
                                         model.model_auto,
                                         crsf.crsf_auto,
                                         crsf.site_street,
                                         crsf.site_suburb,
                                         crsf.site_state,
                                         crsf.site_postcode,
                                         crsf.site_country
                                     };

                //TODO: Get equipment image
                //byte[] bytesImage =
                //    File.ReadAllBytes(HostingEnvironment.MapPath("~/Images/DefaultEquipment.png"));

                // ReSharper disable once LoopCanBeConvertedToQuery
                // Cannot use LINQ because the GetEquipmentLocation function is not supported.
                foreach (var equipment in equipments)
                {
                    // TT-49 Load equipment photo
                    byte[] bytesImage = new DAL.UndercarriageContext().EQUIPMENTs.Find(equipment.equipmentid_auto).EquipmentPhoto;
                    if(bytesImage == null)
                    {
                        bytesImage = File.ReadAllBytes(HostingEnvironment.MapPath("~/Images/DefaultEquipment.png"));
                    }

                    var newEquipment = new EquipmentEntity
                    {
                        EquipmentId = equipment.equipmentid_auto,
                        EquipmentSerialNo = equipment.serialno,
                        EquipmentUnitNo = equipment.unitno,
                        EquipmentCustomer = equipment.cust_name,
                        EquipmentJobsite = equipment.site_name,
                        EquipmentJobsiteAuto = equipment.crsf_auto,
                        EquipmentFamily = equipment.typeid,
                        EquipmentModel = equipment.modelid,
                        EquipmentModelAuto = equipment.model_auto,
                        EquipmentSMU = equipment.currentsmu.ToString(),
                        EquipmentImage = bytesImage,
                        EquipmentLocation =
                            GetEquipmentLocation(equipment.site_street, equipment.site_suburb, equipment.site_state,
                                equipment.site_country)
                    };

                    result.Add(newEquipment);
                }

                var isUCSubSystemVisible = (from settings in dataEntities.APPLICATION_LU_CONFIG
                                            where settings.variable_key == "UCSubSysSerialShow"
                                            select settings.value_key).FirstOrDefault();

                if (isUCSubSystemVisible.Equals("1"))
                {
                    foreach (var equip in result)
                    {
                        var UCSubSerial = string.Empty;
                        var query = from sub in dataEntities.LU_Module_Sub
                                    join geu in dataEntities.GENERAL_EQ_UNIT on sub.Module_sub_auto equals geu.module_ucsub_auto
                                    join lc in dataEntities.LU_COMPART on geu.compartid_auto equals lc.compartid_auto
                                    join lct in dataEntities.LU_COMPART_TYPE on lc.comparttype_auto equals lct.comparttype_auto
                                    where lct.comparttype == "Link" && geu.side == 1 && geu.equipmentid_auto == equip.EquipmentId
                                    select new { sub.Serialno };

                        if (query.FirstOrDefault() != null)
                            UCSubSerial = query.FirstOrDefault().Serialno;



                        equip.UCSerialLeft = UCSubSerial.ToString();
                        UCSubSerial = string.Empty;
                        query = null;

                        query = (from sub in dataEntities.LU_Module_Sub
                                 join geu in dataEntities.GENERAL_EQ_UNIT on sub.Module_sub_auto equals geu.module_ucsub_auto
                                 join lc in dataEntities.LU_COMPART on geu.compartid_auto equals lc.compartid_auto
                                 join lct in dataEntities.LU_COMPART_TYPE on lc.comparttype_auto equals lct.comparttype_auto
                                 where lct.comparttype == "Link" && geu.side == 2 && geu.equipmentid_auto == equip.EquipmentId
                                 select new { sub.Serialno });

                        if (query.FirstOrDefault() != null)
                            UCSubSerial = query.FirstOrDefault().Serialno;

                        equip.UCSerialRight = UCSubSerial.ToString();
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// Get byte array with the Image to show the equipment location in the Mobile App.
        /// </summary>
        /// <param name="siteStreet">Street of the location</param>
        /// <param name="siteSuburb">Suburb of the location</param>
        /// <param name="siteState">State of the location</param>
        /// <param name="siteCountry">Country of the location</param>
        /// <returns>Returns the byte array of the image with the map or default map</returns>
        private static byte[] GetEquipmentLocation(string siteStreet, string siteSuburb, string siteState, string siteCountry)
        {
            string address = !string.IsNullOrEmpty(siteStreet)
                ? siteStreet + ", " + siteSuburb + " " + siteState + " " + siteCountry
                : "None";

            //PRN9622 -- Temporarily commented that code it's not working due to some issue on the ITM live server. need to investigate it.
            //if (address != "None")
            //{
            //    string urlString =
            //        "https://maps.googleapis.com/maps/api/staticmap?zoom=13&size=400x400&markers=%7Clabel:A%7C" +
            //        address +
            //        "&center=" + address;

            //    var webClient = new WebClient();
            //    byte[] imageBytes = webClient.DownloadData(urlString);

            //    return imageBytes;
            //}

            byte[] bytesImage =
                File.ReadAllBytes(HostingEnvironment.MapPath("~/Images/ActivityDetails.png"));

            return bytesImage;
        }

        public bool SaveEquipment(NewEquipmentEntity equipment)
        {
            bool result = false;
            using (var dataEntities = new InfoTrakDataEntities())
            {
                byte[] photoByteArr = null;
                try { photoByteArr = Convert.FromBase64String(equipment._base64Image); } catch {  }
                var Equipment_autoParameter = new ObjectParameter("equip_auto", typeof(long)) { Value = 0 };
                dataEntities.SaveEquipment(Equipment_autoParameter, equipment._serialno, equipment._unitno, equipment._jobsiteAuto, Convert.ToInt32(equipment._smu),
                    equipment._modelAuto, DateTime.ParseExact(equipment._creationDate, "d MM yyyy", null), equipment._examiner, equipment._customer, equipment._jobsite, equipment._model, photoByteArr);

                var smu = equipment._smu;
                var model = equipment._modelAuto;
                var EquipAuto = (int)Equipment_autoParameter.Value;

                if (EquipAuto > 0)
                {
                    foreach (var details in equipment._details)
                    {
                        int side = 0;
                        if (!details.Side.Equals(string.Empty) || details.Side != null)
                        {
                            if (details.Side.ToLower().Equals("left"))
                                side = 1;
                            else if (details.Side.ToLower().Equals("right"))
                                side = 2;
                        }

                        dataEntities.SaveComponentDetailsFromMobileService(EquipAuto, int.Parse(details.Compartid), Convert.ToInt32(smu), Convert.ToInt32(model),
                            (byte?)details.Pos, (byte?)(side), details.FlangeType, DateTime.ParseExact(equipment._creationDate, "d MM yyyy", null),
                            equipment._examiner, details.Compart, Convert.ToInt32(details.CompartIdAuto));
                    }

                    //PRN9455
                    equipment._equipmentInspection.EquipmentIdAuto = EquipAuto;

                    if (UndercarriageInspection.Instance.SaveInspectionForNewEquipment(equipment._equipmentInspection))
                        result = true;
                    
                }
            }

            return result;
        }

        public UndercarriageInspectionEntity SaveEquipmentReturnInspection(NewEquipmentEntity equipment)
        {
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var Equipment_autoParameter = new ObjectParameter("equip_auto", typeof(long)) { Value = 0 };
                dataEntities.SaveEquipment(Equipment_autoParameter, equipment._serialno, equipment._unitno, equipment._jobsiteAuto, Convert.ToInt32(equipment._smu),
                    equipment._modelAuto, DateTime.ParseExact(equipment._creationDate, "d MM yyyy", null), equipment._examiner, equipment._customer, equipment._jobsite, equipment._model);

                var smu = equipment._smu;
                var model = equipment._modelAuto;
                var EquipAuto = (int)Equipment_autoParameter.Value;

                if (EquipAuto > 0)
                {
                    foreach (var details in equipment._details)
                    {
                        int side = 0;
                        if (!details.Side.Equals(string.Empty) || details.Side != null)
                        {
                            if (details.Side.ToLower().Equals("left"))
                                side = 1;
                            else if (details.Side.ToLower().Equals("right"))
                                side = 2;
                        }

                        dataEntities.SaveComponentDetailsFromMobileService(EquipAuto, int.Parse(details.Compartid), Convert.ToInt32(smu), Convert.ToInt32(model),
                            (byte?)details.Pos, (byte?)(side), details.FlangeType, DateTime.ParseExact(equipment._creationDate, "d MM yyyy", null),
                            equipment._examiner, details.Compart, Convert.ToInt32(details.CompartIdAuto));
                    }
                    equipment._equipmentInspection.EquipmentIdAuto = EquipAuto;
                    return equipment._equipmentInspection;
                }
                return null;
            }
        }

        public long GetEquipmentIdBySerialAndUnit(string serial, string unit)
        {
            long result = 0;
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var equipments = from equipment in dataEntities.Mbl_NewEquipment
                                 where equipment.serialno == serial //&& equipment.unitno == unit.Trim()+"- NEW"
                                 select equipment;

                if (equipments.FirstOrDefault() != null)
                    result = equipments.FirstOrDefault().equipmentid_auto;
            }
            return result;
        }
    }

    //PRN9599
    public class EquipmentInspectionList
    {
        private static readonly EquipmentInspectionList _instance = new EquipmentInspectionList();

        public static EquipmentInspectionList Instance
        {
            get { return _instance; }
        }

        public List<BLL.Core.Domain.ResultMessageExtended> SaveEquipmentInspections(EquipmentInspectionListEntity inspectionsList)
        {
            List<BLL.Core.Domain.ResultMessageExtended> rmList = new List<BLL.Core.Domain.ResultMessageExtended>();
            //System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            //string strResult = js.Serialize(rmList);
            try
            {
                if (inspectionsList.EquipmentsInspectionsList != null)
                {
                    //List through existing equipments list
                    foreach (var obj in inspectionsList.EquipmentsInspectionsList)
                    {
                        BLL.Core.Domain.ResultMessageExtended rm =
                            Classes.UndercarriageInspection.Instance.SaveUcInspection(obj);
                        if (!rm.OperationSucceed)
                            rm.LastMessage = rm.LastMessage + "(SMU: " + obj.SMU + ")";
                        rmList.Add(rm);
                    }
                }
                using (TransactionScope scope = new TransactionScope())
                {
                    if (inspectionsList.NewEquipmentsInspectionsList != null)
                    {
                        foreach (var obj2 in inspectionsList.NewEquipmentsInspectionsList)
                        {
                            BLL.Core.Domain.ResultMessageExtended rm = new BLL.Core.Domain.ResultMessageExtended();
                            int smu = 0;
                            rm.PreValidation = new BLL.Core.Domain.ActionPreValidationResult
                            {
                                IsValid = true,
                                EquipmentId = obj2._equipmentId.LongNullableToInt(),
                                Id = 0,
                                ProvidedDate = DateTime.Now,
                                ProvidedSMU = Int32.TryParse(obj2._smu, out smu) ? smu : 0,
                                EarliestValidDateForProvidedSMU = DateTime.Now,
                                SmallestValidSmuForProvidedDate = 0,
                                Status = BLL.Core.Domain.ActionValidationStatus.Valid
                            };
                            rm.OperationSucceed = Classes.Equipments.Instance.SaveEquipment(obj2);
                            if (rm.OperationSucceed)
                                rm.LastMessage = "Data Successfully Synced";
                            else
                                rm.LastMessage = "Save Equipment Failed";
                            rmList.Add(rm);
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                BLL.Core.Domain.ResultMessageExtended rmException = new BLL.Core.Domain.ResultMessageExtended();

                rmException.ActionLog = "In Equipment " + ex.Message;
                if (ex.InnerException != null)
                    rmException.ActionLog += ex.InnerException.Message;
                rmException.OperationSucceed = false;
                rmException.Id = -1;
                rmException.LastMessage = ex.Message;
                rmList.Add(rmException);
            }
            return rmList;
        }

        public List<BLL.Core.Domain.ResultMessageExtended> SaveRopeShovelEquipmentInspections(EquipmentInspectionListEntity inspectionsList)
        {
            List<BLL.Core.Domain.ResultMessageExtended> rmList = new List<BLL.Core.Domain.ResultMessageExtended>();
            try
            {
                if (inspectionsList.EquipmentsInspectionsList != null)
                {
                    //List through existing equipments list
                    foreach (var obj in inspectionsList.EquipmentsInspectionsList)
                    {
                        BLL.Core.Domain.ResultMessageExtended rm =
                            Classes.UndercarriageInspection.Instance.SaveUcRopeShovelInspection(obj);
                        if (!rm.OperationSucceed)
                            rm.LastMessage = rm.LastMessage + "(SMU: " + obj.SMU + ")";
                        rmList.Add(rm);
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Core.Domain.ResultMessageExtended rmException = new BLL.Core.Domain.ResultMessageExtended();

                rmException.ActionLog = "In Equipment " + ex.Message;
                if (ex.InnerException != null)
                    rmException.ActionLog += ex.InnerException.Message;
                rmException.OperationSucceed = false;
                rmException.Id = -1;
                rmException.LastMessage = ex.Message;
                rmList.Add(rmException);
            }
            return rmList;
        }
    }
}