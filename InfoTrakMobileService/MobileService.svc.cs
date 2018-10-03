using System.Collections.Generic;
using InfoTrakMobileService.DataAccess.Classes;
using InfoTrakMobileService.DataAccess.Entities;
using System.Configuration;
using System.Web.Configuration;
using System.IO;
using System;
using System.Web;
using System.ServiceModel.Web;
using System.Linq;
using DAL;
using Newtonsoft.Json;
using BLL.Extensions;

namespace InfoTrakMobileService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MobileService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MobileService.svc or MobileService.svc.cs at the Solution Explorer and start debugging.
    public class MobileService : IMobileService
    {

        public List<CustomerEntity> GetCustomerList(string userName)
        {
            //return Customers.Instance.List;
            return new BLL.GETCore.Classes.CustomerManagement().getListOfCustomersForLoggedInUser(Users.Instance.getUserId(userName)).Select(m => new CustomerEntity { CustomerId = m.customerId, CustomerName = m.customerName }).ToList();
        }

        public List<JobsiteEntity> GetJobsitesByCustomer(long customerAuto, string userName)
        {
            return new BLL.Core.Domain.UserAccess(new SharedContext(), Users.Instance.getUserId(userName)).getAccessibleJobsites().Where(m => m.customer_auto == customerAuto).Select(m => new JobsiteEntity { JobsiteId = m.crsf_auto, JobsiteName = m.site_name }).ToList();
            //return Jobsites.Instance.GetJobsitesByCustomer(customerAuto);
        }


        public List<ModelEntity> GetModelsByJobsite(long jobsiteAuto)
        {
            return Models.Instance.GetModelsByJobsite(jobsiteAuto);
        }

        public List<EquipmentEntity> GetEquipmentByJobsiteAndModel(long jobsiteAuto, int modelAuto)
        {
            return Equipments.Instance.GetEquipmentByJobsiteAndModel(jobsiteAuto, modelAuto);
        }

        public List<BLL.Core.WSRE.Models.WSREChainEquipmentModel> GetEquipmentByJobsiteAndSystem(long jobsiteAuto, string system)
        {
            return Equipments.Instance.GetEquipmentByJobsiteAndSystem(jobsiteAuto, system);
        }

        public List<RecommendationEntity> GetRecommendationByCompartment(long compartment)
        {
            return Recommendations.Instance.GetRecommendationByCompartment(compartment);
        }

        public List<DAL.WSREDipTestCondition> GetLinksConditions()
        {
            return LinksCondition.Instance.GetLinksConditions();
        }

        public List<EquipmentEntity> GetSelectedEquipment(string equipmentList)
        {
            return Equipments.Instance.GetSelectedEquipment(equipmentList);
        }

        public List<ComponentEntity> GetSelectedComponents(string equipmentList)
        {
            return Components.Instance.GetSelectedComponents(equipmentList);
        }

        public List<ComponentEntity> GetSelectedComponentsByModuleSubAuto(string moduleSubAutoList)
        {
            return Components.Instance.GetSelectedComponentsByModuleSubAuto(moduleSubAutoList);
        }

        public List<TestPointImageEntity> GetTestPointImages(string equipmentList)
        {
            List<TestPointImageEntity> resultList = new List<TestPointImageEntity>();
            var equipmentArray = equipmentList.Split(',');
            List<DAL.COMPART_TOOL_IMAGE> resultListRaw = new List<DAL.COMPART_TOOL_IMAGE>();
            foreach (var s in equipmentArray)
            {
                int equipmentId = 0;
                Int32.TryParse(s, out equipmentId);
                if (equipmentId == 0)
                    continue;
                BLL.Core.Domain.Equipment LogicalEquipment = new BLL.Core.Domain.Equipment(new DAL.UndercarriageContext(), equipmentId);
                var k = LogicalEquipment.GetEquipmentCompartToolImageList();
                resultListRaw.AddRange(k);
            }
            resultListRaw = resultListRaw.GroupBy(m => m.Id).Select(m => m.First()).ToList();
            foreach (var img in resultListRaw)
            {
                TestPointImageEntity ent = new TestPointImageEntity();
                ent.CompartType = img.CompartId;
                ent.TestPointImage = img.ImageData;
                ent.Tool = img.Tool.tool_code;
                resultList.Add(ent);
            }
            return resultList;
        }

        public List<TestPointImageEntity> GetTestPointImagesByModuleSubAuto(string moduleList)
        {
            List<TestPointImageEntity> resultList = new List<TestPointImageEntity>();
            var moduleArray = moduleList.Split(',');
            List<DAL.COMPART_TOOL_IMAGE> resultListRaw = new List<DAL.COMPART_TOOL_IMAGE>();
            foreach (var s in moduleArray)
            {
                int moduleId = 0;
                Int32.TryParse(s, out moduleId);
                if (moduleId == 0)
                    continue;
                BLL.Core.Domain.LuModuleSub LogicalEquipment = new BLL.Core.Domain.LuModuleSub(new DAL.UndercarriageContext());
                var k = LogicalEquipment.GetEquipmentCompartToolImageListByModuleSubAuto(moduleId);
                resultListRaw.AddRange(k);
            }
            resultListRaw = resultListRaw.GroupBy(m => m.Id).Select(m => m.First()).ToList();
            foreach (var img in resultListRaw)
            {
                TestPointImageEntity ent = new TestPointImageEntity();
                ent.CompartType = img.CompartId;
                ent.TestPointImage = img.ImageData;
                ent.Tool = img.Tool.tool_code;
                resultList.Add(ent);
            }
            return resultList;
        }

        //public List<LimitsEntity> GetUCLimits()
        //{
        //    return Limits.Instance.GetUCLimits();
        //}

        public List<LimitsEntity> GetUCLimits(string equipmentList)
        {
            var k = Limits.Instance.GetEquipmentLimits(equipmentList);
            return k;
        }

        public List<LimitsEntity> GetUCLimitsByModuleSubAuto(string moduleList)
        {
            var k = Limits.Instance.GetEquipmentLimitsBymoduleSubAuto(moduleList);
            return k;
        }

        public List<LimitsEntity> GetUCLimitsByCompartIdAuto(string compartIdAutoList)
        {
            var k = Limits.Instance.GetEquipmentLimitsByCompartIdAuto(compartIdAutoList);
            return k;
        }

        public bool SaveUcInspection(UndercarriageInspectionEntity inspection)
        {
            var rm = UndercarriageInspection.Instance.SaveUcInspection(inspection);
            return rm.OperationSucceed;

        }

        public bool AuthenticateUser(string username, string password)
        {
            return Users.Instance.Authenticate(username, password);
        }

        public List<DealershipLimitEntity> GetDealershipLimits()
        {
            return DealershipLimits.Instance.GetDealershipLimits();
        }


        public UserPreferenceEntity GetUserPreference(string userId)
        {
            return Users.Instance.GetUserPreferenceById(userId);
        }

        public bool SaveEquipment(NewEquipmentEntity equipment)
        {
            return Equipments.Instance.SaveEquipment(equipment);
        }

        public bool SaveInspectionForNewEquipment(UndercarriageInspectionEntity inspection)
        {
            return UndercarriageInspection.Instance.SaveInspectionForNewEquipment(inspection);
        }

        public long GetEquipmentIdBySerialAndUnit(string serial, string unit)
        {
            return Equipments.Instance.GetEquipmentIdBySerialAndUnit(serial, unit);
        }
        public List<BLL.Core.Domain.ResultMessageExtended> SaveEquipmentsInspectionsData(EquipmentInspectionListEntity EquipList)
        {
            List<BLL.Core.Domain.ResultMessageExtended> rmList = new List<BLL.Core.Domain.ResultMessageExtended>();
            try
            {
                rmList = EquipmentInspectionList.Instance.SaveEquipmentInspections(EquipList);
            }
            catch (Exception ex)
            {
                BLL.Core.Domain.ResultMessageExtended rmException = new BLL.Core.Domain.ResultMessageExtended();
                rmException.ActionLog = "In Service " + ex.Message;
                if (ex.InnerException != null)
                    rmException.ActionLog += ex.InnerException.Message;
                rmException.OperationSucceed = false;
                rmException.Id = -1;
                rmException.LastMessage = ex.Message;
                rmList.Add(rmException);
            }
            return rmList;
        }

        public Stream Save_iOS_EquipmentsInspectionsData(EquipmentInspectionListEntity EquipList)
        {
            List<BLL.Core.Domain.ResultMessageExtended> rmList = new List<BLL.Core.Domain.ResultMessageExtended>();
            try
            {
                rmList = EquipmentInspectionList.Instance.SaveEquipmentInspections(EquipList);
            }
            catch (Exception ex)
            {
                BLL.Core.Domain.ResultMessageExtended rmException = new BLL.Core.Domain.ResultMessageExtended();
                rmException.ActionLog = "In Service " + ex.Message;
                if (ex.InnerException != null)
                    rmException.ActionLog += ex.InnerException.Message;
                rmException.OperationSucceed = false;
                rmException.Id = -1;
                rmException.LastMessage = ex.Message;
                rmList.Add(rmException);
            }

            //return rmList.First().Id;
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(rmList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public List<BLL.Core.Domain.ResultMessageExtended> SaveReactEquipmentsInspectionsData(EquipmentInspectionListEntity EquipList)
        {
            List<BLL.Core.Domain.ResultMessageExtended> rmList = new List<BLL.Core.Domain.ResultMessageExtended>();
            return rmList;
        }

        //PRN9632
        public string GetApplicationVersion()
        {
            return Users.Instance.GetApplicationVersion();
        }

        public string GetUpdatedApp()
        {
            var branding = new BLL.Core.Domain.Dealership(new DAL.UndercarriageContext()).getDealershipFirstBranding();
            if (branding == null)
                return "";
            var fileAddress = new BLL.Core.Domain.SharedDomain(new DAL.SharedContext()).getApkFileAddressFromMenu();
            return "http://" + branding.UCHost + "/" + fileAddress;
            //return "http://" + "192.168.1.116/TTdev" + "/" + fileAddress;
        }

        public string GetWebAppDomain()
        {
            var branding = new BLL.Core.Domain.Dealership(new DAL.UndercarriageContext()).getDealershipFirstBranding();
            if (branding == null)
                return "";
            return "http://" + branding.UCHost;
        }

        public string IsAlive()
        {
            return "1";
        }

        //PRN10234
        public ImageEntity GetApplicationLogo()
        {
            //string strReturn = string.Empty;
            List<ImageEntity> list = new List<ImageEntity>();
            ImageEntity obj = new ImageEntity();

            string Val1 = ConfigurationSettings.AppSettings.Get("Logo");

            string path = HttpContext.Current.Server.MapPath("~/Images/" + Val1);
            byte[] logo = File.ReadAllBytes(path);
            obj._logoName = Val1;
            obj._logo = Convert.ToBase64String(logo);
            //strReturn = JsonConvert.SerializeObject(obj);
            list.Add(obj);

            return obj;
        }

        //PRN10234
        public string GetDefaultSkin()
        {
            return ConfigurationSettings.AppSettings.Get("DefaultSkin");
        }

        //PRN10396
        public ImageEntity GetTitleBarLog()
        {
            //string strReturn = string.Empty;
            List<ImageEntity> list = new List<ImageEntity>();
            ImageEntity obj = new ImageEntity();

            string Val1 = ConfigurationSettings.AppSettings.Get("Titlebar_Logo");

            string path = HttpContext.Current.Server.MapPath("~/Images/" + Val1);
            byte[] logo = File.ReadAllBytes(path);
            obj._logoName = Val1;
            obj._logo = Convert.ToBase64String(logo);
            //strReturn = JsonConvert.SerializeObject(obj);
            list.Add(obj);

            return obj;
        }

        // PRN11155 - PaulN start
        public List<GETEntity> GetImplementsByEquipment(long equipmentAuto, string serial_no)
        {
            return GETs.Instance.GetImplementsByEquipment(equipmentAuto, serial_no);
        }
        // PRN11155 - PaulN end

        public long PostWSREInspectionRecord(BLL.Core.WSRE.Models.WRESSyncModel Equip)
        {

            long newWSRERecord = 0;
            try
            {
                BLL.Core.Domain.WSREMobileManager manager = new BLL.Core.Domain.WSREMobileManager(new UndercarriageContext());
                newWSRERecord = manager.InsertWSREInspectionRecord(Equip);
            }
            catch (Exception e)
            {
                return 0;
            }

            return newWSRERecord;
        }

        public Boolean PostWSREEquipInfo(BLL.Core.WSRE.Models.WRESSyncModel Equip)
        {
            Boolean result = false;
            try
            {
                BLL.Core.Domain.WSREMobileManager manager = new BLL.Core.Domain.WSREMobileManager(new UndercarriageContext());
                result = manager.SaveWSREInfo(Equip);
            }
            catch (Exception e)
            {
                return false;
            }

            return result;
        }

        public String GetWSREEnableSetting()
        {
            String returnVal = "0";
            try
            {
                BLL.Core.Domain.WSREMobileManager manager =
                    new BLL.Core.Domain.WSREMobileManager(new UndercarriageContext());
                returnVal = manager.GetWSREEnableSetting();
            }
            catch (Exception e)
            {
                return "0";
            }

            return returnVal;
        }

        /// <summary>
        /// Mining Shovel APIs
        /// </summary>
        /// <param name="Equip"></param>
        /// <returns></returns>
        public Stream PostValidateMiningShovelEquipInfo(BLL.Core.MiningShovel.Models.SyncModel Equip)
        {

            List<BLL.Core.Domain.ResultMessageExtended> rmList = new List<BLL.Core.Domain.ResultMessageExtended>();
            try
            {
                ///////////////////////////////////////////////////////////
                // Update TRACK_INSPECTION and TRACK_INSPECTION_DETAIL
                EquipmentInspectionListEntity EquipList = new EquipmentInspectionListEntity();

                ///////////////////
                // Equipment count
                EquipList.EquipmentsCount = 1;

                ///////////////////
                // New equipment
                List<NewEquipmentEntity> newEquip = new List<NewEquipmentEntity>();
                EquipList.NewEquipmentsInspectionsList = newEquip;

                ///////////////////
                // Equipment list
                List<UndercarriageInspectionEntity> EquipmentsInspectionsList = new List<UndercarriageInspectionEntity>();
                UndercarriageInspectionEntity equipInspection = new UndercarriageInspectionEntity();
                equipInspection.Abrasive = Equip.abrasive;
                equipInspection.Impact = Equip.impact;
                equipInspection.Moisture = Equip.moisture;
                equipInspection.Packing = Equip.packing;
                equipInspection.SMU = Equip.smu.ToString();
                equipInspection.DryJointsLeft = 0;
                equipInspection.DryJointsRight = 0;
                equipInspection.EquipmentIdAuto = Equip.equipmentid_auto;
                equipInspection.Examiner = Equip.examiner;
                equipInspection.ExtCannonLeft = 0;
                equipInspection.ExtCannonRight = 0;
                equipInspection.InspectionDate = Equip.currentDateandTime;
                equipInspection.InspectorComments = Equip.notes;
                equipInspection.JobsiteComments = Equip.Jobsite_Comms;
                equipInspection.leftCannonExtComment = "";
                equipInspection.leftCannonExtImage = "";
                equipInspection.leftScallop = 0;
                equipInspection.leftTrackSagComment = "";
                equipInspection.leftTrackSagImage = "";
                equipInspection.rightCannonExtComment = "";
                equipInspection.rightCannonExtImage = "";
                equipInspection.rightScallop = 0;
                equipInspection.rightTrackSagComment = "";
                equipInspection.rightTrackSagImage = "";
                equipInspection.TrackSagLeft = 0;
                equipInspection.TrackSagRight = 0;
                equipInspection.travelForward = 0;
                equipInspection.travelReverse = 0;
                equipInspection.TrammingHours = Equip.TrammingHours;
                equipInspection.CustomerContact = Equip.CustomerContact;

                // InspectionDetails
                //equipInspection.Details = ???
                List<InspectionDetails> inspectionDetailList = new List<InspectionDetails>();
                foreach (var item in Equip.InspectionDetails)
                {
                    InspectionDetails inspectionDetail = new InspectionDetails();
                    inspectionDetail.AttachmentType = 0;
                    inspectionDetail.Comments = "";
                    inspectionDetail.TrackUnitAuto = item.EqunitAuto;

                    DAL.LU_COMPART compart = new LU_COMPART();
                    BLL.Core.Domain.Component component = new BLL.Core.Domain.Component(new UndercarriageContext());
                    compart = component.getCompart(item.EqunitAuto);

                    inspectionDetail.CompartIdAuto = compart.compartid_auto;
                    inspectionDetail.FlangeType = "";
                    inspectionDetail.Image = "";
                    inspectionDetail.InspectionImage = "";
                    inspectionDetail.PercentageWorn = 0;
                    inspectionDetail.Reading = "";
                    inspectionDetail.ToolUsed = "";

                    inspectionDetailList.Add(inspectionDetail);
                }

                equipInspection.Details = inspectionDetailList;

                EquipmentsInspectionsList.Add(equipInspection);

                EquipList.EquipmentsInspectionsList = EquipmentsInspectionsList;

                /////////////
                // Validate
                //List<BLL.Core.Domain.ResultMessageExtended> returnList = SaveEquipmentsInspectionsData(EquipList);
                try
                {
                    rmList = EquipmentInspectionList.Instance.SaveRopeShovelEquipmentInspections(EquipList);
                }
                catch (Exception ex)
                {
                    BLL.Core.Domain.ResultMessageExtended rmException = new BLL.Core.Domain.ResultMessageExtended();
                    rmException.ActionLog = "In Service " + ex.Message;
                    if (ex.InnerException != null)
                        rmException.ActionLog += ex.InnerException.Message;
                    rmException.OperationSucceed = false;
                    rmException.Id = -1;
                    rmException.LastMessage = ex.Message;
                    rmList.Add(rmException);
                }
            }
            catch (Exception ex)
            {
                BLL.Core.Domain.ResultMessageExtended rmException = new BLL.Core.Domain.ResultMessageExtended();
                rmException.ActionLog = "In Service " + ex.Message;
                if (ex.InnerException != null)
                    rmException.ActionLog += ex.InnerException.Message;
                rmException.OperationSucceed = false;
                rmException.Id = -1;
                rmException.LastMessage = ex.Message;
                rmList.Add(rmException);
            }

            //return rmList.First().Id;
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(rmList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Mining Shovel APIs
        /// </summary>
        /// <param name="Equip"></param>
        /// <returns></returns>
        public Boolean PostMiningShovelEquipInfo(BLL.Core.MiningShovel.Models.SyncModel Equip)
        {
            Boolean returnVal = true;
            try
            {
                BLL.Core.Domain.MiningShovelMobileSyncManager manager =
                    new BLL.Core.Domain.MiningShovelMobileSyncManager(new UndercarriageContext());
                returnVal = manager.SaveMiningShovelInfo(Equip);
            }
            catch (Exception e)
            {
                return false;
            }

            return returnVal;
        }

        /// <summary>
        /// Get additional records
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="modelId"></param>
        /// <param name="compartTypeId"></param>
        /// <returns></returns>
        public List<BLL.Core.MiningShovel.Models.AdditionalRecordModel> GetAdditionalRecords(
            long customerId, long modelId, long compartTypeId)
        {
            List<BLL.Core.MiningShovel.Models.AdditionalRecordModel> additionalRecords = new List<BLL.Core.MiningShovel.Models.AdditionalRecordModel>();
            try
            {
                BLL.Core.Domain.MiningShovelMobileManager manager =
                    new BLL.Core.Domain.MiningShovelMobileManager(new UndercarriageContext());
                additionalRecords = manager.GetAdditionalRecords(customerId, modelId, compartTypeId);
            }
            catch (Exception e)
            {
                return null;
            }

            return additionalRecords;
        }

        /// <summary>
        /// Get measurement point list
        /// </summary>
        /// <param name="compartId"></param>
        /// <returns></returns>
        public List<BLL.Core.MiningShovel.Models.MeasurementPointModel> GetMeasurementPointsByCompartId(long compartId)
        {
            List<BLL.Core.MiningShovel.Models.MeasurementPointModel> records = 
                new List<BLL.Core.MiningShovel.Models.MeasurementPointModel>();
            try
            {
                BLL.Core.Domain.MiningShovelMobileManager manager =
                    new BLL.Core.Domain.MiningShovelMobileManager(new UndercarriageContext());
                records = manager.GetMeasurementPointsByCompartId(compartId);
            }
            catch (Exception e)
            {
                return null;
            }

            return records;
        }

        /// <summary>
        /// Get mandatory images
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="modelId"></param>
        /// <param name="compartTypeId"></param>
        /// <returns></returns>
        public List<BLL.Core.MiningShovel.Models.MandatoryImageModel> GetMandatoryImageRecords(
            long customerId, long modelId, long compartTypeId)
        {
            List<BLL.Core.MiningShovel.Models.MandatoryImageModel> mandatoryImageRecords = new List<BLL.Core.MiningShovel.Models.MandatoryImageModel>();
            try
            {
                BLL.Core.Domain.MiningShovelMobileManager manager =
                    new BLL.Core.Domain.MiningShovelMobileManager(new UndercarriageContext());
                mandatoryImageRecords = manager.GetMandatoryImageRecords(customerId, modelId, compartTypeId);
            }
            catch (Exception e)
            {
                return null;
            }

            return mandatoryImageRecords;
        }

        /// <summary>
        /// Get mandatory images
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="modelId"></param>
        /// <param name="compartTypeId"></param>
        /// <returns></returns>
        public List<BLL.Core.MiningShovel.Models.EquipmentImageModel> GetEquipmentImageRecords(
            long customerId, long modelId)
        {
            List<BLL.Core.MiningShovel.Models.EquipmentImageModel> equipmentImageRecords = new List<BLL.Core.MiningShovel.Models.EquipmentImageModel>();
            try
            {
                BLL.Core.Domain.MiningShovelMobileManager manager =
                    new BLL.Core.Domain.MiningShovelMobileManager(new UndercarriageContext());
                equipmentImageRecords = manager.GetEquipmentImageRecords(customerId, modelId);
            }
            catch (Exception e)
            {
                return null;
            }

            return equipmentImageRecords;
        }

        /// <summary>
        /// Post images to server
        /// </summary>
        /// <param name="Equip"></param>
        /// <returns></returns>
        public Boolean PostImage(BLL.Core.MiningShovel.Models.SyncImage Image)
        {
            try
            {
                BLL.Core.Domain.MiningShovelMobileSyncManager manager = 
                    new BLL.Core.Domain.MiningShovelMobileSyncManager(new UndercarriageContext());
                manager.SaveImage(Image);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Post images to server
        /// </summary>
        /// <param name="Equip"></param>
        /// <returns></returns>
        public Boolean PostWSREImage(BLL.Core.WSRE.Models.SyncImage Image)
        {
            try
            {
                BLL.Core.Domain.WSREMobileManager manager =
                    new BLL.Core.Domain.WSREMobileManager(new UndercarriageContext());
                manager.SaveImage(Image);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Download MAKE table
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream DownloadMAKETable()
        {
            List<BLL.Core.WSRE.Models.DownloadMake> recordList = new List<BLL.Core.WSRE.Models.DownloadMake>();
            try
            {
                BLL.Core.Domain.WSREMobileDownloadDB manager =
                    new BLL.Core.Domain.WSREMobileDownloadDB(new UndercarriageContext());
                recordList = manager.GetMAKERecords();
            }
            catch (Exception e)
            {
                return null;
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(recordList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Download MODEL table
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream DownloadMODELTable()
        {
            List<BLL.Core.WSRE.Models.DownloadModel> recordList = new List<BLL.Core.WSRE.Models.DownloadModel>();
            try
            {
                BLL.Core.Domain.WSREMobileDownloadDB manager =
                    new BLL.Core.Domain.WSREMobileDownloadDB(new UndercarriageContext());
                recordList = manager.GetMODELRecords();
            }
            catch (Exception e)
            {
                return null;
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(recordList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Download LU_MMTA table
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream DownloadLU_MMTATable()
        {
            List<BLL.Core.WSRE.Models.DownloadLU_MMTA> recordList = new List<BLL.Core.WSRE.Models.DownloadLU_MMTA>();
            try
            {
                BLL.Core.Domain.WSREMobileDownloadDB manager =
                    new BLL.Core.Domain.WSREMobileDownloadDB(new UndercarriageContext());
                recordList = manager.GetLU_MMTARecords();
            }
            catch (Exception e)
            {
                return null;
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(recordList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Download LU_COMPART_TYPE table
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream DownloadLU_COMPART_TYPETable()
        {
            List<BLL.Core.WSRE.Models.DownloadLU_COMPART_TYPE> recordList = new List<BLL.Core.WSRE.Models.DownloadLU_COMPART_TYPE>();
            try
            {
                BLL.Core.Domain.WSREMobileDownloadDB manager =
                    new BLL.Core.Domain.WSREMobileDownloadDB(new UndercarriageContext());
                recordList = manager.GetLU_COMPART_TYPERecords();
            }
            catch (Exception e)
            {
                return null;
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(recordList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Download LU_COMPART table
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream DownloadLU_COMPARTTable()
        {
            List<BLL.Core.WSRE.Models.DownloadLU_COMPART> recordList = new List<BLL.Core.WSRE.Models.DownloadLU_COMPART>();
            try
            {
                BLL.Core.Domain.WSREMobileDownloadDB manager =
                    new BLL.Core.Domain.WSREMobileDownloadDB(new UndercarriageContext());
                recordList = manager.GetLU_COMPARTRecords();
            }
            catch (Exception e)
            {
                return null;
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(recordList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Download TRACK_COMPART_EXT table
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream DownloadTRACK_COMPART_EXTTable()
        {
            List<BLL.Core.WSRE.Models.DownloadTRACK_COMPART_EXT> recordList = new List<BLL.Core.WSRE.Models.DownloadTRACK_COMPART_EXT>();
            try
            {
                BLL.Core.Domain.WSREMobileDownloadDB manager =
                    new BLL.Core.Domain.WSREMobileDownloadDB(new UndercarriageContext());
                recordList = manager.GetTRACK_COMPART_EXTRecords();
            }
            catch (Exception e)
            {
                return null;
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(recordList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Download TRACK_COMPART_WORN_CALC_METHOD table
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream DownloadTRACK_COMPART_WORN_CALC_METHODTable()
        {
            List<BLL.Core.WSRE.Models.DownloadTRACK_COMPART_WORN_CALC_METHOD> recordList = new List<BLL.Core.WSRE.Models.DownloadTRACK_COMPART_WORN_CALC_METHOD>();
            try
            {
                BLL.Core.Domain.WSREMobileDownloadDB manager =
                    new BLL.Core.Domain.WSREMobileDownloadDB(new UndercarriageContext());
                recordList = manager.GetTRACK_COMPART_WORN_CALC_METHODRecords();
            }
            catch (Exception e)
            {
                return null;
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(recordList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Download SHOE_SIZE table
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream DownloadSHOE_SIZETable()
        {
            List<BLL.Core.WSRE.Models.DownloadSHOE_SIZE> recordList = new List<BLL.Core.WSRE.Models.DownloadSHOE_SIZE>();
            try
            {
                BLL.Core.Domain.WSREMobileDownloadDB manager =
                    new BLL.Core.Domain.WSREMobileDownloadDB(new UndercarriageContext());
                recordList = manager.GetSHOE_SIZERecords();
            }
            catch (Exception e)
            {
                return null;
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(recordList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Download SHOE_SIZE table
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream DownloadTRACK_COMPART_MODEL_MAPPINGTable()
        {
            List<BLL.Core.WSRE.Models.DownloadTRACK_COMPART_MODEL_MAPPING> recordList = 
                new List<BLL.Core.WSRE.Models.DownloadTRACK_COMPART_MODEL_MAPPING>();
            try
            {
                BLL.Core.Domain.WSREMobileDownloadDB manager =
                    new BLL.Core.Domain.WSREMobileDownloadDB(new UndercarriageContext());
                recordList = manager.GetTRACK_COMPART_MODEL_MAPPINGRecords();
            }
            catch (Exception e)
            {
                return null;
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(recordList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        /// <summary>
        /// Download TYPE table
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream DownloadTYPETable()
        {
            List<BLL.Core.WSRE.Models.DownloadTYPE> recordList =
                new List<BLL.Core.WSRE.Models.DownloadTYPE>();
            try
            {
                BLL.Core.Domain.WSREMobileDownloadDB manager =
                    new BLL.Core.Domain.WSREMobileDownloadDB(new UndercarriageContext());
                recordList = manager.GetTYPERecords();
            }
            catch (Exception e)
            {
                return null;
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(recordList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Download TRACK_TOOL table
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream DownloadTRACK_TOOLTable()
        {
            List<BLL.Core.WSRE.Models.DownloadTRACK_TOOL> recordList =
                new List<BLL.Core.WSRE.Models.DownloadTRACK_TOOL>();
            try
            {
                BLL.Core.Domain.WSREMobileDownloadDB manager =
                    new BLL.Core.Domain.WSREMobileDownloadDB(new UndercarriageContext());
                recordList = manager.GetTRACK_TOOLRecords();
            }
            catch (Exception e)
            {
                return null;
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(recordList));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Create New Chain
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream createNewChain(BLL.Core.WSRE.Models.WSRENewChain newChain)
        {
            BLL.Core.ViewModel.SetupViewModel system = new BLL.Core.ViewModel.SetupViewModel();
            try
            {
                BLL.Core.Domain.WSREMobileCreateNewChain manager =
                    new BLL.Core.Domain.WSREMobileCreateNewChain(new UndercarriageContext());
                system = manager.createNewChain(newChain);
            }
            catch (Exception e)
            {
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(system));
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Download TRACK_ACTION_TYPE table
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public Stream DownloadTRACK_ACTION_TYPETable()
        {
            List<BLL.Core.WSRE.Models.DownloadTRACK_ACTION_TYPE> recordList = new List<BLL.Core.WSRE.Models.DownloadTRACK_ACTION_TYPE>();
            try
            {
                BLL.Core.Domain.WSREMobileDownloadDB manager =
                    new BLL.Core.Domain.WSREMobileDownloadDB(new UndercarriageContext());
                recordList = manager.GetTRACK_ACTION_TYPERecords();
            }
            catch (Exception e)
            {
                return null;
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("{DownloadTRACK_ACTION_TYPETableResult:" + JsonConvert.SerializeObject(recordList) + "}");
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}