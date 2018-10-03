using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using InfoTrakMobileService.DataAccess.Entities;
using InfoTrakMobileService.DataAccess.Model;
using System.Collections.Generic;

namespace InfoTrakMobileService.DataAccess.Classes
{
    public class UndercarriageInspection
    {
         private static readonly UndercarriageInspection InstanceUcInspection = new UndercarriageInspection();

         private UndercarriageInspection()
        {
        }

         public static UndercarriageInspection Instance
        {
            get { return InstanceUcInspection; }
        }

        /*
         *         [DataMember]
        [DataMember]
        public long  { get; set; }

        [DataMember]
        public long  { get; set; }

        [DataMember]
        public String  { get; set; }

        [DataMember]
        public int  { get; set; }

        [DataMember]
        public String  { get; set; }

        [DataMember]
        public string  { get; set; }

        [DataMember]
        public String  { get; set; }

        [DataMember]
        public int  { get; set; }

        [DataMember]
        public string  { get; set; }

        [DataMember]
        public string  { get; set; }
         */
        private short StringToShort(string number)
        {
            short n = 0;
            short.TryParse(number, out n);
            return n;
        }
        private BLL.Core.Domain.InsertInspectionParams getInsertInspectionParams(UndercarriageInspectionEntity inspection)
        {
            BLL.Core.Domain.InsertInspectionParams inspectionParams = new BLL.Core.Domain.InsertInspectionParams();
            int smu = 0;
            Int32.TryParse(inspection.SMU, out smu);
            
            inspectionParams.EquipmentInspection = new DAL.TRACK_INSPECTION
            {
                abrasive = StringToShort(inspection.Abrasive.ToString()),
                equipmentid_auto = inspection.EquipmentIdAuto,
                examiner = inspection.Examiner,
                inspection_date = DateTime.ParseExact(inspection.InspectionDate, "dd MM yyyy", null),
                smu = smu,
                impact = StringToShort(inspection.Impact.ToString()),
                moisture = StringToShort(inspection.Moisture.ToString()),
                packing = StringToShort(inspection.Packing.ToString()),
                track_sag_left = inspection.TrackSagLeft,
                track_sag_right = inspection.TrackSagRight,
                dry_joints_left = inspection.DryJointsLeft,
                dry_joints_right = inspection.DryJointsRight,
                ext_cannon_left = inspection.ExtCannonLeft,
                ext_cannon_right = inspection.ExtCannonRight,
                Jobsite_Comms = inspection.JobsiteComments,
                inspection_comments = inspection.InspectorComments,
                LeftTrackSagComment = inspection.leftTrackSagComment,
                RightTrackSagComment = inspection.rightTrackSagComment,
                LeftCannonExtensionComment = inspection.leftCannonExtComment,
                RightCannonExtensionComment = inspection.rightCannonExtComment,
                LeftDryJointComments = inspection.leftDryJointsComment,
                RightDryJointComments = inspection.rightDryJointsComment,
                LeftScallopComments = inspection.leftScallopComment,
                RightScallopComments = inspection.rightScallopComment,

                TravelledKms = inspection.travelledByKms,
                ForwardTravelHours = inspection.travelForward,
                ReverseTravelHours = inspection.travelReverse,
                ForwardTravelKm = inspection.travelForwardKm,
                ReverseTravelKm = inspection.travelReverseKm,
                LeftScallopMeasurement = inspection.leftScallop,
                RightScallopMeasurement = inspection.rightScallop
            };
            try { inspectionParams.EquipmentInspection.LeftTrackSagImage = Convert.FromBase64String(inspection.leftTrackSagImage); } catch { }
            try { inspectionParams.EquipmentInspection.RightTrackSagImage = Convert.FromBase64String(inspection.rightTrackSagImage); } catch { }
            try { inspectionParams.EquipmentInspection.LeftCannonExtensionImage = Convert.FromBase64String(inspection.leftCannonExtImage); } catch { }
            try { inspectionParams.EquipmentInspection.RightCannonExtensionImage = Convert.FromBase64String(inspection.rightCannonExtImage); } catch { }
            try { inspectionParams.EquipmentInspection.DryJointsLeftImage = Convert.FromBase64String(inspection.leftDryJointsImage); } catch { }
            try { inspectionParams.EquipmentInspection.DryJointsRightImage = Convert.FromBase64String(inspection.rightDryJointsImage); } catch { }
            try { inspectionParams.EquipmentInspection.LeftScallopImage = Convert.FromBase64String(inspection.leftCannonExtImage); } catch { }
            try { inspectionParams.EquipmentInspection.RightScallopImage = Convert.FromBase64String(inspection.rightCannonExtImage); } catch { }

            System.Collections.Generic.List<BLL.Core.Domain.InspectionDetailWithSide> TidList = new System.Collections.Generic.List<BLL.Core.Domain.InspectionDetailWithSide>();
            foreach(var k in inspection.Details)
            {
                decimal read = 0;
                decimal.TryParse(k.Reading, out read);
                int toolId = -1;
                switch (k.ToolUsed)
                {
                    case "R":
                        toolId = 1;
                        break;
                    case "DG":
                        toolId = 2;
                        break;
                    case "UT":
                        toolId = 3;
                        break;
                    case "C":
                        toolId = 4;
                        break;
                }
                List<DAL.TRACK_INSPECTION_IMAGES> ImageList = new List<DAL.TRACK_INSPECTION_IMAGES>();
                List<DAL.COMPART_ATTACH_FILESTREAM> ImageStreamList = new List<DAL.COMPART_ATTACH_FILESTREAM>();
                try
                {
                    byte[] imgData = Convert.FromBase64String(k.Image);
                    ImageList.Add(new DAL.TRACK_INSPECTION_IMAGES
                    {
                        GUID = Guid.NewGuid(),
                        image_comment = k.Comments,
                        image_data = imgData
                    });
                    ImageStreamList.Add(new DAL.COMPART_ATTACH_FILESTREAM
                    {
                        attachment = imgData,
                        attachment_name = "EquipmentSerial-CompartId-Side-Position",
                        comment = k.Comments,
                        entry_date = DateTime.Now,
                        guid = Guid.NewGuid()
                });
                }
                catch(Exception e1)
                {
                    string Message = e1.Message;
                }

                // If % Worn is between -5% and 0%, set it to 0%. TT-76.
                k.PercentageWorn = k.PercentageWorn < 0 && k.PercentageWorn > -5 ? 0 : k.PercentageWorn;

                DAL.TRACK_INSPECTION_DETAIL tid = new DAL.TRACK_INSPECTION_DETAIL
                {
                    track_unit_auto = k.TrackUnitAuto,
                    reading = read,
                    worn_percentage = k.PercentageWorn,
                    tool_auto = toolId,
                    comments = k.Comments,
                    Images = ImageList
                };
                int compId = (tid.track_unit_auto > Int32.MaxValue || tid.track_unit_auto < Int32.MinValue) ? 0 : Convert.ToInt32(tid.track_unit_auto);
                int componentSide = (int)new BLL.Core.Domain.Component(new DAL.UndercarriageContext(), compId).GetComponentSide();
                TidList.Add(
                    new BLL.Core.Domain.InspectionDetailWithSide {
                        ComponentInspectionDetail = tid,
                        CompartAttachFileStreamImage = ImageStreamList,
                        side = componentSide
                    });
            }
            inspectionParams.ComponentsInspection = TidList;
            return inspectionParams;
        }
        private BLL.Interfaces.IUser GetUserIdByExaminer(string examiner)
        {
            DAL.USER_TABLE user = null;
            using (var ucdb = new DAL.UndercarriageContext())
            {
                var k = ucdb.USER_TABLE.Where(m => m.userid == examiner || m.username == examiner);
                if (k.Count() > 0)
                    user = k.First();
                else
                {
                    k = ucdb.USER_TABLE.Where(m => m.user_auto == 1);
                    if (k.Count() > 0)
                        user = k.First();
                }
            }
            return new BLL.Core.Domain.User { Id = (int)user.user_auto, userName = user.username, userStrId = user.userid };
        }

        private string GetUniqueDocketNo()
        {
            return "UC"+ DateTime.Now.Millisecond.ToString()+ DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.DayOfYear.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() ;
        }

        private void UpdateEquipmentImageFromInspection(long EquipmentIdAuto, String EquipmentImage)
        {
            DAL.UndercarriageContext _ucContext = new DAL.UndercarriageContext();
            _ucContext.EQUIPMENTs.Find(EquipmentIdAuto).EquipmentPhoto = Convert.FromBase64String(EquipmentImage);
            _ucContext.SaveChanges();
        }

        public BLL.Core.Domain.ResultMessageExtended SaveUcInspection(UndercarriageInspectionEntity inspection)
        {
            var rm = new BLL.Core.Domain.ResultMessageExtended
            {
                OperationSucceed = false,
                ActionLog = " ",
                LastMessage = " ",
                Id = 0,
            };
            BLL.Interfaces.IUser user = GetUserIdByExaminer(inspection.Examiner);
            if(user == null)
            {
                rm.LastMessage = "User Not Found!";
                return rm;
            }
            inspection.Examiner = user.userName;
            BLL.Core.Domain.InsertInspectionParams Params = getInsertInspectionParams(inspection);
            if (Params.EquipmentInspection.docket_no == null || Params.EquipmentInspection.docket_no.Length < 2)
                Params.EquipmentInspection.docket_no = GetUniqueDocketNo();
            BLL.Interfaces.IEquipmentActionRecord EquipmentAction = new BLL.Core.Domain.EquipmentActionRecord
            {
                ActionDate = Params.EquipmentInspection.inspection_date,
                ActionUser = user,
                EquipmentId = Params.EquipmentInspection.equipmentid_auto > int.MaxValue ? int.MaxValue : (int)Params.EquipmentInspection.equipmentid_auto,
                Comment = Params.EquipmentInspection.inspection_comments,
                ReadSmuNumber = Params.EquipmentInspection.smu == null ? 0 : (int)Params.EquipmentInspection.smu,
                TypeOfAction = BLL.Core.Domain.ActionType.InsertInspection,
                Cost = 0
            };

            using (BLL.Core.Domain.Action UCAction = new BLL.Core.Domain.Action(new DAL.UndercarriageContext(), EquipmentAction, Params))
            {
                rm.PreValidation = UCAction.PreValidate(EquipmentAction);
                if (!rm.PreValidation.IsValid)
                {
                    rm.LastMessage = "Validation Failed!";
                    rm.ActionLog = "PreValidation Failed";
                    rm.OperationSucceed = false;
                    return rm;
                }
                UCAction.Operation.Start();
                if (UCAction.Operation.Status == BLL.Core.Domain.ActionStatus.Close)
                {
                    rm.OperationSucceed = false;
                    //rm.ActionLog = UCAction.Operation.ActionLog;
                    rm.LastMessage = UCAction.Operation.Message;
                }
                if (UCAction.Operation.Status == BLL.Core.Domain.ActionStatus.Started)
                    UCAction.Operation.Validate();

                if (UCAction.Operation.Status == BLL.Core.Domain.ActionStatus.Invalid)
                {
                    rm.OperationSucceed = false;
                    //rm.ActionLog = UCAction.Operation.ActionLog;
                    rm.LastMessage = UCAction.Operation.Message;
                }
                if (UCAction.Operation.Status == BLL.Core.Domain.ActionStatus.Valid)
                    UCAction.Operation.Commit();
                if (UCAction.Operation.Status == BLL.Core.Domain.ActionStatus.Failed)
                {
                    rm.OperationSucceed = false;
                    //rm.ActionLog = UCAction.Operation.ActionLog;
                    rm.LastMessage = UCAction.Operation.Message;
                }
                if (UCAction.Operation.Status == BLL.Core.Domain.ActionStatus.Succeed)
                {
                    rm.OperationSucceed = true;
                    //rm.ActionLog = UCAction.Operation.ActionLog;
                    rm.LastMessage = UCAction.Operation.Message;

                    // TT-49
                    if (inspection.EquipmentImage != null)
                    {
                        UpdateEquipmentImageFromInspection(inspection.EquipmentIdAuto, inspection.EquipmentImage);
                    }
                }
                rm.Id = UCAction.Operation.UniqueId;
            }

            try
            {
                BLL.Core.Domain.Equipment LogicalEquipment = new BLL.Core.Domain.Equipment(new DAL.UndercarriageContext(), EquipmentAction.EquipmentId);
                if (LogicalEquipment.Id == 0 || LogicalEquipment.GetEquipmentFamily() != BLL.Core.Domain.EquipmentFamily.MEX_Mining_Shovel)
                    return rm;
                LogicalEquipment.UpdateMiningShovelInspectionParentsFromChildren(rm.Id);
                return rm;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return rm;
            }
        }
        public bool SaveUcInspectionOldVersionNotUsedAnyMore(UndercarriageInspectionEntity inspection)
        {
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var inspectionAutoObjectParameter = new ObjectParameter("inspection_auto", typeof(long)) {Value = 0};
                var ltd = dataEntities.GetEquipmentLTD(inspection.EquipmentIdAuto,
                    long.Parse(inspection.SMU), DateTime.ParseExact(inspection.InspectionDate,"dd MM yyyy",null)).FirstOrDefault() ??
                          long.Parse(inspection.SMU);
                var allowableWear = (inspection.Impact == 2) ? 2 : 1;
                var docket = "UC" + DateTime.Now.ToString("ddMMyyhhmm");
                var userAuto = GetUserAuto(inspection.Examiner);

                dataEntities.StoreTrackInspection(inspectionAutoObjectParameter,
                    inspection.EquipmentIdAuto, inspection.Examiner, DateTime.ParseExact(inspection.InspectionDate, "dd MM yyyy", null),
                    int.Parse(inspection.SMU), (int)ltd, null, inspection.TrackSagLeft,
                    inspection.TrackSagRight, null, null, inspection.DryJointsLeft,
                    inspection.DryJointsRight, inspection.ExtCannonLeft, inspection.ExtCannonRight, 0, 0, null, null,
                    (short) inspection.Impact, (short) inspection.Abrasive, (short) inspection.Moisture,
                    (short)inspection.Packing, DateTime.Now.Date, inspection.Examiner, null, null, null, null, null, null, null, null, null,
                    null, docket, null, (byte)allowableWear,
                    inspection.InspectorComments, inspection.JobsiteComments);

                var inspectionAuto = (int)inspectionAutoObjectParameter.Value;

                if (inspectionAuto > 0)
                {
                    foreach (var componentDetails in inspection.Details)
                    {
                        var detailAutoObjectParameter = new ObjectParameter("inspection_detail_auto", typeof(long)) {Value = 0};
                        var projectedHours = 0;
                        var extProjectedHours = 0;
                        var remainingHours = 0;
                        var extRemainingHours = 0;

                        if (componentDetails.TrackUnitAuto <= 0 || String.IsNullOrEmpty(componentDetails.Reading) ||
                            String.IsNullOrEmpty(componentDetails.PercentageWorn.ToString())) continue;

                        var componentInstance =
                            dataEntities.GetComponentById(componentDetails.TrackUnitAuto).FirstOrDefault();


                        if (componentInstance == null || componentInstance.equnit_auto <= 0) continue;

                        var ltdDifference = (int) (ltd - componentInstance.eq_ltd_at_install);
                        //var hoursOnSurface = (int) (ltdDifference >= 0 ? (ltdDifference + componentInstance.smu_at_install) : 0);
                        //PRN9229
                        var hoursOnSurface = (int)(ltdDifference >= 0 ? (ltdDifference + componentInstance.cmu) : 0);


                        if (hoursOnSurface > 0)
                        {
                            if (componentDetails.PercentageWorn > 0M)
                            {
                                projectedHours = componentDetails.PercentageWorn <= 100M
                                    ? Convert.ToInt32(Convert.ToDecimal(hoursOnSurface)*100/
                                                      componentDetails.PercentageWorn)
                                    : Convert.ToInt32(hoursOnSurface);

                                extProjectedHours =
                                    Convert.ToInt32(Convert.ToInt32(projectedHours)*1.2);

                                remainingHours = Convert.ToInt32(projectedHours) >=
                                                 Convert.ToInt32(hoursOnSurface)
                                    ? Convert.ToInt32(projectedHours) -
                                      Convert.ToInt32(hoursOnSurface)
                                    : 0;

                                extRemainingHours = Convert.ToInt32(extProjectedHours) >=
                                                    Convert.ToInt32(hoursOnSurface)
                                    ? Convert.ToInt32(extProjectedHours) -
                                      Convert.ToInt32(hoursOnSurface)
                                    : 0;
                            }
                            else
                            {
                                if (componentInstance.track_budget_life != null)
                                {
                                    projectedHours = (int) componentInstance.track_budget_life;
                                    remainingHours = (int) componentInstance.track_budget_life;
                                }

                                extProjectedHours = (int) (projectedHours*1.2);
                                extRemainingHours = (int) (remainingHours*1.2);
                            }
                        }
                        else if (hoursOnSurface == 0)
                        {
                            if (componentInstance.track_budget_life != null)
                            {
                                projectedHours = (int) componentInstance.track_budget_life;
                                remainingHours = (int) componentInstance.track_budget_life;
                            }

                            extProjectedHours = (int) (projectedHours*1.2);
                            extRemainingHours = (int) (remainingHours*1.2);
                        }

                        dataEntities.StoreTrackInspectionDetail(detailAutoObjectParameter,
                            inspectionAuto, componentDetails.TrackUnitAuto,
                            GetToolAuto(componentDetails.ToolUsed),
                            decimal.Parse(componentDetails.Reading),
                            componentDetails.PercentageWorn, null, null, hoursOnSurface, projectedHours, extProjectedHours, remainingHours, 
                            extRemainingHours, componentDetails.Comments);

                        var detailAuto = (int) detailAutoObjectParameter.Value;
                        var position = Components.Instance.GetPosition(componentDetails.TrackUnitAuto);

                        if (String.IsNullOrEmpty(componentDetails.Image)) continue;

                        var input = Convert.FromBase64String(componentDetails.Image);


                        var k_eq = dataEntities.EQUIPMENTs.Find(inspection.EquipmentIdAuto);

                        var k_compart_list = dataEntities.LU_COMPART.Where(m => m.compartid_auto == componentDetails.CompartIdAuto);

                        var k_component = dataEntities.GENERAL_EQ_UNIT.Find(componentDetails.TrackUnitAuto);

                        string imageFileName_PRN11089 = "";
                        if (k_eq != null)
                            imageFileName_PRN11089 = k_eq.serialno;
                        if (k_compart_list != null && k_compart_list.Count() > 0)
                            imageFileName_PRN11089 += "_" + k_compart_list.FirstOrDefault().compart;
                        if (k_component != null)
                            imageFileName_PRN11089 += "_" + k_component.side + "_" + k_component.pos;
                        if (imageFileName_PRN11089.Length == 0)
                            imageFileName_PRN11089 = "ImageFromMobile";
                        imageFileName_PRN11089 += ".jpg";


                        var result = new ObjectParameter("result", typeof(int)) { Value = 0 };
                        dataEntities.StoreCompartAttachment(componentDetails.CompartIdAuto,
                            componentDetails.AttachmentType, input,
                            imageFileName_PRN11089, DateTime.Now, userAuto, "", inspectionAuto,
                            null, null, position, null, result);
                    }

                    
                    dataEntities.UpdateTrackEval(inspectionAutoObjectParameter, userAuto);
                    return true;
                }

            }
            return false;
        }

        private static int? GetUserAuto(string examiner)
        {
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var userAuto =
                    (from users in dataEntities.USER_TABLE
                        where users.userid == examiner
                        select users.user_auto).FirstOrDefault();

                return (int)userAuto;

            }
        }

        private static int? GetToolAuto(string toolUsed)
        {
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var toolAuto = (from tools in dataEntities.TRACK_TOOL
                    where tools.tool_code == toolUsed
                    select tools.tool_auto).FirstOrDefault();

                return toolAuto;
            }
        }

        public bool SaveInspectionForNewEquipment(UndercarriageInspectionEntity inspection)
        {
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var inspectionAutoObjectParameter = new ObjectParameter("inspection_auto", typeof (long)) {Value = 0};

                var userAuto = GetUserAuto(inspection.Examiner);
                int SMU = 0;

                if (!inspection.SMU.Equals(string.Empty))
                    SMU = int.Parse(inspection.SMU);

                var examinerUser = dataEntities.USER_TABLE.Where(m => m.userid == inspection.Examiner || m.username == inspection.Examiner);
                if (examinerUser.Count() > 0)
                    inspection.Examiner = examinerUser.First().username;
                if(inspection.Examiner == null)
                {
                    var examinerInfotrak = dataEntities.USER_TABLE.Find(1);
                    if (examinerInfotrak == null)
                        inspection.Examiner = "Unknown";
                    else inspection.Examiner = examinerInfotrak.username;
                }
                dataEntities.SaveInspectionForNewEquipment(inspectionAutoObjectParameter,
                    inspection.EquipmentIdAuto, SMU, inspection.TrackSagLeft,
                    inspection.TrackSagRight, inspection.DryJointsLeft,
                    inspection.DryJointsRight, inspection.ExtCannonLeft, inspection.ExtCannonRight,
                    (short) inspection.Impact, (short) inspection.Abrasive, (short) inspection.Moisture,
                    (short) inspection.Packing, inspection.InspectorComments, inspection.JobsiteComments,
                    DateTime.ParseExact(inspection.InspectionDate, "dd MM yyyy", null), inspection.Examiner,
                    inspection.leftTrackSagComment, inspection.rightTrackSagComment, inspection.leftCannonExtComment,
        inspection.rightCannonExtComment, inspection.leftTrackSagImage, inspection.rightTrackSagImage, inspection.leftCannonExtImage,
        inspection.rightCannonExtImage, inspection.travelForward, inspection.travelReverse, inspection.leftScallop, inspection.rightScallop, inspection.travelledByKms);

                var inspectionAuto = (int) inspectionAutoObjectParameter.Value;

                if (inspectionAuto > 0)
                {
                    decimal reading = 0;
                    foreach (var componentDetails in inspection.Details)
                    {
                        //var detailAutoObjectParameter = new ObjectParameter("inspection_detail_auto", typeof(long)) {Value = 0};

                       // if (componentDetails.TrackUnitAuto <= 0 || String.IsNullOrEmpty(componentDetails.Reading))
                            //continue;

                        //var componentInstance =
                        //    dataEntities.GetComponentById(componentDetails.TrackUnitAuto).FirstOrDefault();

                        //if (componentInstance == null || componentInstance.equnit_auto <= 0) continue;

                        if (!String.IsNullOrEmpty(componentDetails.Reading))
                            reading = decimal.Parse(componentDetails.Reading);

                        dataEntities.SaveInspectionDetailsForNewEquipment(inspectionAuto,
                            (int) componentDetails.TrackUnitAuto,
                            reading,
                            GetToolAuto(componentDetails.ToolUsed),
                            componentDetails.Comments);

                        reading = 0;
                        //var detailAuto = (int) detailAutoObjectParameter.Value;
                        var position = Components.Instance.GetPositionforNewEquip(componentDetails.TrackUnitAuto);

                        if (String.IsNullOrEmpty(componentDetails.Image)) continue;
                        
                        var input = Convert.FromBase64String(componentDetails.Image);

                        var result = new ObjectParameter("result", typeof(int)) { Value = 0 };
                        dataEntities.StoreCompartAttachmentForNewEquip(componentDetails.CompartIdAuto,
                        componentDetails.AttachmentType, input,
                        "ImageFromMobile.jpg", DateTime.Now, userAuto, "", inspectionAuto,
                        null, null, position, result);
                    }




                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Rope shovel
        /// </summary>
        /// <param name="inspection"></param>
        /// <returns></returns>
        public BLL.Core.Domain.ResultMessageExtended SaveUcRopeShovelInspection(UndercarriageInspectionEntity inspection)
        {
            var rm = new BLL.Core.Domain.ResultMessageExtended
            {
                OperationSucceed = false,
                ActionLog = " ",
                LastMessage = " ",
                Id = 0,
            };
            BLL.Interfaces.IUser user = GetUserIdByExaminer(inspection.Examiner);
            if (user == null)
            {
                rm.LastMessage = "User Not Found!";
                return rm;
            }
            inspection.Examiner = user.userName;
            BLL.Core.Domain.InsertInspectionParams Params = getInsertInspectionParams(inspection);
            if (Params.EquipmentInspection.docket_no == null || Params.EquipmentInspection.docket_no.Length < 2)
                Params.EquipmentInspection.docket_no = GetUniqueDocketNo();


            BLL.Interfaces.IEquipmentActionRecord EquipmentAction = new BLL.Core.Domain.EquipmentActionRecord
            {
                ActionDate = Params.EquipmentInspection.inspection_date,
                ActionUser = user,
                EquipmentId = Params.EquipmentInspection.equipmentid_auto > int.MaxValue ? int.MaxValue : (int)Params.EquipmentInspection.equipmentid_auto,
                Comment = Params.EquipmentInspection.inspection_comments,
                ReadSmuNumber = Params.EquipmentInspection.smu == null ? 0 : (int)Params.EquipmentInspection.smu,
                TypeOfAction = BLL.Core.Domain.ActionType.InsertInspection,
                Cost = 0
            };

            /////////////////
            // Rope Shovel
            BLL.Interfaces.IGeneralInspectionModel GeneralInspection = new BLL.Core.ViewModel.GeneralInspectionViewModel
            {
                Date = Params.EquipmentInspection.inspection_date,
                SMU = (int)Params.EquipmentInspection.smu,
                TrammingHours = inspection.TrammingHours,
                CustomerContact = inspection.CustomerContact,
                InspectionNotes = inspection.InspectorComments,
                DocketNo = Params.EquipmentInspection.docket_no,
                Impact = inspection.Impact,
                Abrasive = inspection.Abrasive,
                Moisture = inspection.Moisture,
                Packing = inspection.Packing,
                JobSiteNotes = inspection.JobsiteComments
            };

            /////////////////
            // Rope Shovel
            //using (BLL.Core.Domain.Action UCAction = new BLL.Core.Domain.Action(new DAL.UndercarriageContext(), EquipmentAction, Params))
            using (BLL.Core.Domain.Action UCAction = new BLL.Core.Domain.Action(new DAL.UndercarriageContext(), EquipmentAction, GeneralInspection))
            {
                rm.PreValidation = UCAction.PreValidate(EquipmentAction);
                if (!rm.PreValidation.IsValid)
                {
                    rm.LastMessage = "Validation Failed!";
                    rm.ActionLog = "PreValidation Failed";
                    rm.OperationSucceed = false;
                    return rm;
                }

                UCAction.Operation.Start();
                if (UCAction.Operation.Status == BLL.Core.Domain.ActionStatus.Close)
                {
                    rm.OperationSucceed = false;
                    //rm.ActionLog = UCAction.Operation.ActionLog;
                    rm.LastMessage = UCAction.Operation.Message;
                }
                if (UCAction.Operation.Status == BLL.Core.Domain.ActionStatus.Started)
                    UCAction.Operation.Validate();

                if (UCAction.Operation.Status == BLL.Core.Domain.ActionStatus.Invalid)
                {
                    rm.OperationSucceed = false;
                    //rm.ActionLog = UCAction.Operation.ActionLog;
                    rm.LastMessage = UCAction.Operation.Message;
                }
                if (UCAction.Operation.Status == BLL.Core.Domain.ActionStatus.Valid)
                    UCAction.Operation.Commit();
                if (UCAction.Operation.Status == BLL.Core.Domain.ActionStatus.Failed)
                {
                    rm.OperationSucceed = false;
                    //rm.ActionLog = UCAction.Operation.ActionLog;
                    rm.LastMessage = UCAction.Operation.Message;
                }
                if (UCAction.Operation.Status == BLL.Core.Domain.ActionStatus.Succeed)
                {
                    rm.OperationSucceed = true;
                    //rm.ActionLog = UCAction.Operation.ActionLog;
                    rm.LastMessage = UCAction.Operation.Message;
                }
                rm.Id = UCAction.Operation.UniqueId;
            }

            try
            {
                BLL.Core.Domain.Equipment LogicalEquipment = new BLL.Core.Domain.Equipment(new DAL.UndercarriageContext(), EquipmentAction.EquipmentId);
                //if (LogicalEquipment.Id == 0 || LogicalEquipment.GetEquipmentFamily() != BLL.Core.Domain.EquipmentFamily.MEX_Mining_Shovel)
                //    return rm;
                //LogicalEquipment.UpdateMiningShovelInspectionParentsFromChildren(rm.Id);
                return rm;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return rm;
            }
        }
    }
}
 