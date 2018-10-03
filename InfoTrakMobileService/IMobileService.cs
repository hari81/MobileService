using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using InfoTrakMobileService.DataAccess.Classes;
using InfoTrakMobileService.DataAccess.Entities;
using System.IO;

namespace InfoTrakMobileService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMobileService" in both code and config file together.
    [ServiceContract]
    public interface IMobileService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetCustomerList?userName={userName}")]
        List<CustomerEntity> GetCustomerList(string userName);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetJobsitesByCustomer?customerAuto={customerAuto}&userName={userName}")]
        List<JobsiteEntity> GetJobsitesByCustomer(long customerAuto, string userName);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetModelsByJobsite?jobsiteAuto={jobsiteAuto}")]
        List<ModelEntity> GetModelsByJobsite(long jobsiteAuto);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetEquipmentByJobsiteAndModel?jobsiteAuto={jobsiteAuto}&modelAuto={modelAuto}")]
        List<EquipmentEntity> GetEquipmentByJobsiteAndModel(long jobsiteAuto, int modelAuto);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetEquipmentByJobsiteAndSystem?jobsiteAuto={jobsiteAuto}&system={system}")]
        List<BLL.Core.WSRE.Models.WSREChainEquipmentModel> GetEquipmentByJobsiteAndSystem(long jobsiteAuto, string system);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetRecommendationByCompartment?compartment={compartment}")]
        List<RecommendationEntity> GetRecommendationByCompartment(long compartment);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetLinksConditions")]
        List<DAL.WSREDipTestCondition> GetLinksConditions();

        [OperationContract]
        [WebInvoke(Method = "GET",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetSelectedEquipment?equipmentList={equipmentList}")]
        List<EquipmentEntity> GetSelectedEquipment(string equipmentList);

        [OperationContract]
        [WebInvoke(Method = "GET",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetSelectedComponents?equipmentList={equipmentList}")]
        List<ComponentEntity> GetSelectedComponents(string equipmentList);

        [OperationContract]
        [WebInvoke(Method = "GET",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetSelectedComponentsByModuleSubAuto?moduleSubAutoList={moduleSubAutoList}")]
        List<ComponentEntity> GetSelectedComponentsByModuleSubAuto(string moduleSubAutoList);

        [OperationContract]
        [WebInvoke(Method = "GET",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetTestPointImages?equipmentList={equipmentList}")]
        List<TestPointImageEntity> GetTestPointImages(string equipmentList);

        [OperationContract]
        [WebInvoke(Method = "GET",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetTestPointImagesByModuleSubAuto?moduleList={moduleList}")]
        List<TestPointImageEntity> GetTestPointImagesByModuleSubAuto(string moduleList);

        //[OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Wrapped,
        //    UriTemplate = "GetUCLimits/")]
        //List<LimitsEntity> GetUCLimits();
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetUCLimits?equipmentList={equipmentList}")]
        List<LimitsEntity> GetUCLimits(string equipmentList);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetUCLimitsByModuleSubAuto?moduleList={moduleList}")]
        List<LimitsEntity> GetUCLimitsByModuleSubAuto(string moduleList);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetUCLimitsByCompartIdAuto?compartIdAutoList={compartIdAutoList}")]
        List<LimitsEntity> GetUCLimitsByCompartIdAuto(string compartIdAutoList);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetDealershipLimits/")]
        List<DealershipLimitEntity> GetDealershipLimits();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
           UriTemplate = "SaveUcInspection")]
        bool SaveUcInspection(UndercarriageInspectionEntity inspection);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "AuthenticateUser?username={username}&password={password}")]
        bool AuthenticateUser(string username, string password);



        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "GetUserPreference?userId={userId}")]
        UserPreferenceEntity GetUserPreference(string userId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
           UriTemplate = "SaveEquipment")]
        bool SaveEquipment(NewEquipmentEntity equipment);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
           UriTemplate = "SaveInspectionForNewEquipment")]
        bool SaveInspectionForNewEquipment(UndercarriageInspectionEntity inspection);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetEquipmentIdBySerialAndUnit?serial={serial}&unit={unit}")]
        long GetEquipmentIdBySerialAndUnit(string serial, string unit);

        //PRN9599
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
           UriTemplate = "SaveEquipmentsInspectionsData")]
        List<BLL.Core.Domain.ResultMessageExtended> SaveEquipmentsInspectionsData(EquipmentInspectionListEntity EquipList);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
           UriTemplate = "Save_iOS_EquipmentsInspectionsData")]
        Stream Save_iOS_EquipmentsInspectionsData(EquipmentInspectionListEntity EquipList);

        //PRN9632
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "GetApplicationVersion")]
        string GetApplicationVersion();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "GetUpdatedApp")]
        string GetUpdatedApp();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "GetWebAppDomain")]
        string GetWebAppDomain();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "IsAlive")]
        string IsAlive();

        //PRN10234
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetApplicationLogo")]
        ImageEntity GetApplicationLogo();

        //PRN10234
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetDefaultSkin")]
        string GetDefaultSkin();

        //PRN10396
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetTitleBarLog")]
        ImageEntity GetTitleBarLog();

        // PRN11155 - PaulN start
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetImplementsByEquipment?equipmentAuto={equipmentAuto}&serial_no={serial_no}")]
        List<GETEntity> GetImplementsByEquipment(long equipmentAuto, string serial_no);
        // PRN11155 - PaulN end

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "PostWSREInspectionRecord")]
        long PostWSREInspectionRecord(BLL.Core.WSRE.Models.WRESSyncModel Equip);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "PostWSREEquipInfo")]
        bool PostWSREEquipInfo(BLL.Core.WSRE.Models.WRESSyncModel Equip);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetWSREEnableSetting")]
        string GetWSREEnableSetting();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "PostValidateMiningShovelEquipInfo")]
        Stream PostValidateMiningShovelEquipInfo(BLL.Core.MiningShovel.Models.SyncModel Equip);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "PostMiningShovelEquipInfo")]
        bool PostMiningShovelEquipInfo(BLL.Core.MiningShovel.Models.SyncModel Equip);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetAdditionalRecords?customerId={customerId}&modelId={modelId}&compartTypeId={compartTypeId}")]
        List<BLL.Core.MiningShovel.Models.AdditionalRecordModel> GetAdditionalRecords(long customerId, long modelId, long compartTypeId);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetMeasurementPointsByCompartId?compartId={compartId}")]
        List<BLL.Core.MiningShovel.Models.MeasurementPointModel> GetMeasurementPointsByCompartId(long compartId);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetMandatoryImageRecords?customerId={customerId}&modelId={modelId}&compartTypeId={compartTypeId}")]
        List<BLL.Core.MiningShovel.Models.MandatoryImageModel> GetMandatoryImageRecords(long customerId, long modelId, long compartTypeId);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetEquipmentImageRecords?customerId={customerId}&modelId={modelId}")]
        List<BLL.Core.MiningShovel.Models.EquipmentImageModel> GetEquipmentImageRecords(long customerId, long modelId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "PostImage")]
        bool PostImage(BLL.Core.MiningShovel.Models.SyncImage Image);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "PostWSREImage")]
        bool PostWSREImage(BLL.Core.WSRE.Models.SyncImage Image);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "DownloadMAKETable")]
        Stream DownloadMAKETable();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "DownloadMODELTable")]
        Stream DownloadMODELTable();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "DownloadLU_MMTATable")]
        Stream DownloadLU_MMTATable();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "DownloadLU_COMPART_TYPETable")]
        Stream DownloadLU_COMPART_TYPETable();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "DownloadLU_COMPARTTable")]
        Stream DownloadLU_COMPARTTable();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "DownloadTRACK_COMPART_EXTTable")]
        Stream DownloadTRACK_COMPART_EXTTable();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "DownloadTRACK_COMPART_WORN_CALC_METHODTable")]
        Stream DownloadTRACK_COMPART_WORN_CALC_METHODTable();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "DownloadSHOE_SIZETable")]
        Stream DownloadSHOE_SIZETable();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "DownloadTRACK_COMPART_MODEL_MAPPINGTable")]
        Stream DownloadTRACK_COMPART_MODEL_MAPPINGTable();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "DownloadTYPETable")]
        Stream DownloadTYPETable();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "DownloadTRACK_TOOLTable")]
        Stream DownloadTRACK_TOOLTable();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "DownloadTRACK_ACTION_TYPETable")]
        Stream DownloadTRACK_ACTION_TYPETable();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "createNewChain")]
        Stream createNewChain(BLL.Core.WSRE.Models.WSRENewChain newChain);
    }
}