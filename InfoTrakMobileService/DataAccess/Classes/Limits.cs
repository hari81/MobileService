using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Compilation;
using System.Web.Hosting;
using InfoTrakMobileService.DataAccess.Entities;
using InfoTrakMobileService.DataAccess.Model;

namespace InfoTrakMobileService.DataAccess.Classes
{
    public class Limits
    {
        private static readonly Limits InstanceLimits = new Limits();

        private Limits()
        {
        }

        public static Limits Instance
        {
            get { return InstanceLimits; }
        }

        /// <summary>
        /// Get all limits. This is used in the Equipment Search screen for the Mobile App.
        /// </summary>
        /// <returns>List of LimitsEntity for the selected equipment</returns>
        public List<LimitsEntity> GetUCLimits()
        {
            var result = new List<LimitsEntity>();
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var limitsForEquipment = dataEntities.GetLimitsForEquipment(0, 0, 0, 0, 0, 0, 0);

                foreach (var limit in limitsForEquipment)
                {
                    var newLimit = new LimitsEntity
                    {

                        CompartIdAuto = limit.compartid_auto,
                        Method = limit.method,
                        ITMTool = GetToolCode(limit.itm_tool),
                        StartDepthNew = limit.start_depth_new,
                        WearDepth10Percent = limit.wear_depth_10_percent,
                        WearDepth20Percent = limit.wear_depth_20_percent,
                        WearDepth30Percent = limit.wear_depth_30_percent,
                        WearDepth40Percent = limit.wear_depth_40_percent,
                        WearDepth50Percent = limit.wear_depth_50_percent,
                        WearDepth60Percent = limit.wear_depth_60_percent,
                        WearDepth70Percent = limit.wear_depth_70_percent,
                        WearDepth80Percent = limit.wear_depth_80_percent,
                        WearDepth90Percent = limit.wear_depth_90_percent,
                        WearDepth100Percent = limit.wear_depth_100_percent,
                        WearDepth110Percent = limit.wear_depth_110_percent,
                        WearDepth120Percent = limit.wear_depth_120_percent,
                        CATTool = GetToolCode(limit.cat_tool),
                        Slope = limit.slope,
                        AdjToBase = limit.adjust_base,
                        HiInflectionPoint = limit.hi_inflectionPoint,
                        HiSlope1 = limit.hi_slope1,
                        HiIntercept1 = limit.hi_intercept1,
                        HiSlope2 = limit.hi_slope2,
                        HiIntercept2 = limit.hi_intercept2,
                        MiInflectionPoint = limit.mi_inflectionPoint,
                        MiSlope1 = limit.mi_slope1,
                        MiIntercept1 = limit.mi_intercept1,
                        MiSlope2 = limit.mi_slope2,
                        MiIntercept2 = limit.mi_intercept2,
                        
                        //PRN9826
                        KomatsuTool = limit.komatsu_tool,
                        ImpactSlope = limit.impact_slope,
                        ImpactIntercept = limit.impact_intercept,
                        NormalSlope = limit.normal_slope,
                        NormalIntercept = limit.normal_intercept,
                        
                        HitachiTool = limit.Hitachi_tool,
                        ImpactSlopeHit=limit.hit_impact_slope,
                        ImpactInterceptHit = limit.hit_impact_intercept,
                        NormalSlopeHit = limit.hit_normal_slope,
                        NormalInterceptHit = limit.hit_normal_intercept,

                        LiebherrTool = limit.Liebherr_tool,
                        ImpactSlopeLie = limit.lie_impact_slope,
                        ImpactInterceptLie = limit.lie_impact_intercept,
                        NormalSlopeLie = limit.lie_normal_slope,
                        NormalInterceptLie = limit.lie_normal_intercept

                    };
                    result.Add(newLimit);
                }        
            }
            return result;
        }

        public List<LimitsEntity> GetEquipmentLimits(string equipmentList)
        {
            List<LimitsEntity> resultList = new List<LimitsEntity>();
            var equipmentArray = equipmentList.Split(',');

            List<BLL.Core.Domain.CompartWornExtViewModel> modelList = new List<BLL.Core.Domain.CompartWornExtViewModel>();
            foreach (var s in equipmentArray)
            {
                int equipmentId = 0;
                Int32.TryParse(s, out equipmentId);
                if (equipmentId == 0)
                    continue;
                BLL.Core.Domain.Equipment LogicalEquipment = new BLL.Core.Domain.Equipment(new DAL.UndercarriageContext(), equipmentId);
                modelList.AddRange(LogicalEquipment.getWornLimitList());
            }
            foreach (var model in modelList)
            {
                int max = 0;
                if (model.ITMExtList != null && model.ITMExtList.Count > max) max = model.ITMExtList.Count;
                if (model.CATExtList != null && model.CATExtList.Count > max) max = model.CATExtList.Count;
                if (model.KomatsuExtList != null && model.KomatsuExtList.Count > max) max = model.KomatsuExtList.Count;
                if (model.HitachiExtList != null && model.HitachiExtList.Count > max) max = model.HitachiExtList.Count;
                if (model.LiebherrExtList != null && model.LiebherrExtList.Count > max) max = model.LiebherrExtList.Count;

                for (int k = 0; k < max; k++)
                {
                    bool ITM = model.ITMExtList != null && model.ITMExtList.Count > k ? true : false;
                    bool CAT = model.CATExtList != null && model.CATExtList.Count > k ? true : false;
                    bool Komatsu = model.KomatsuExtList != null && model.KomatsuExtList.Count > k ? true : false;
                    bool Hitachi = model.HitachiExtList != null && model.HitachiExtList.Count > k ? true : false;
                    bool Liebherr = model.LiebherrExtList != null && model.LiebherrExtList.Count > k ? true : false;

                    var res = new LimitsEntity
                    {
                        CompartIdAuto = model.Id,
                        Method = getMethodById((int)model.method)
                    };
                    if (ITM)
                    {
                        res.CompartMeasurePointId = model.ITMExtList.ElementAt(k).MeasurePointId ?? 0;
                        res.ITMTool = GetToolCode(model.ITMExtList.ElementAt(k).track_tools_auto);
                        res.StartDepthNew = model.ITMExtList.ElementAt(k).start_depth_new;
                        res.WearDepth10Percent = model.ITMExtList.ElementAt(k).wear_depth_10_percent;
                        res.WearDepth20Percent = model.ITMExtList.ElementAt(k).wear_depth_20_percent;
                        res.WearDepth30Percent = model.ITMExtList.ElementAt(k).wear_depth_30_percent;
                        res.WearDepth40Percent = model.ITMExtList.ElementAt(k).wear_depth_40_percent;
                        res.WearDepth50Percent = model.ITMExtList.ElementAt(k).wear_depth_50_percent;
                        res.WearDepth60Percent = model.ITMExtList.ElementAt(k).wear_depth_60_percent;
                        res.WearDepth70Percent = model.ITMExtList.ElementAt(k).wear_depth_70_percent;
                        res.WearDepth80Percent = model.ITMExtList.ElementAt(k).wear_depth_80_percent;
                        res.WearDepth90Percent = model.ITMExtList.ElementAt(k).wear_depth_90_percent;
                        res.WearDepth100Percent = model.ITMExtList.ElementAt(k).wear_depth_100_percent;
                        res.WearDepth110Percent = model.ITMExtList.ElementAt(k).wear_depth_110_percent;
                        res.WearDepth120Percent = model.ITMExtList.ElementAt(k).wear_depth_120_percent;
                    }
                    if (CAT)
                    {
                        res.CompartMeasurePointId = model.CATExtList.ElementAt(k).MeasurePointId ?? 0;
                        res.CATTool = GetToolCode(model.CATExtList.ElementAt(k).track_tools_auto);
                        res.Slope = model.CATExtList.ElementAt(k).slope;
                        res.AdjToBase = model.CATExtList.ElementAt(k).adjust_base;
                        res.HiInflectionPoint = model.CATExtList.ElementAt(k).hi_inflectionPoint;
                        res.HiSlope1 = model.CATExtList.ElementAt(k).hi_slope1;
                        res.HiIntercept1 = model.CATExtList.ElementAt(k).hi_intercept1;
                        res.HiSlope2 = model.CATExtList.ElementAt(k).hi_slope2;
                        res.HiIntercept2 = model.CATExtList.ElementAt(k).hi_intercept2;
                        res.MiInflectionPoint = model.CATExtList.ElementAt(k).mi_inflectionPoint;
                        res.MiSlope1 = model.CATExtList.ElementAt(k).mi_slope1;
                        res.MiIntercept1 = model.CATExtList.ElementAt(k).mi_intercept1;
                        res.MiSlope2 = model.CATExtList.ElementAt(k).mi_slope2;
                        res.MiIntercept2 = model.CATExtList.ElementAt(k).mi_intercept2;
                    }
                    if (Komatsu)
                    {
                        res.CompartMeasurePointId = model.KomatsuExtList.ElementAt(k).MeasurePointId ?? 0;
                        res.KomatsuTool = GetToolCode(model.KomatsuExtList.ElementAt(k).track_tools_auto);
                        res.ImpactSecondOrder = model.KomatsuExtList.ElementAt(k).impact_secondorder;
                        res.NormalSecondOrder = model.KomatsuExtList.ElementAt(k).normal_secondorder;
                        res.ImpactSlope = model.KomatsuExtList.ElementAt(k).impact_slope;
                        res.ImpactIntercept = model.KomatsuExtList.ElementAt(k).impact_intercept;
                        res.NormalSlope = model.KomatsuExtList.ElementAt(k).normal_slope;
                        res.NormalIntercept = model.KomatsuExtList.ElementAt(k).normal_intercept;
                    }

                    if (Hitachi)
                    {
                        res.CompartMeasurePointId = model.HitachiExtList.ElementAt(k).MeasurePointId ?? 0;
                        res.HitachiTool = GetToolCode(model.HitachiExtList.ElementAt(k).track_tools_auto);
                        res.ImpactSlopeHit = model.HitachiExtList.ElementAt(k).impact_slope;
                        res.ImpactInterceptHit = model.HitachiExtList.ElementAt(k).impact_intercept;
                        res.NormalSlopeHit = model.HitachiExtList.ElementAt(k).normal_slope;
                        res.NormalInterceptHit = model.HitachiExtList.ElementAt(k).normal_intercept;
                    }
                    if (Liebherr)
                    {
                        res.CompartMeasurePointId = model.LiebherrExtList.ElementAt(k).MeasurePointId ?? 0;
                        res.LiebherrTool = GetToolCode(model.LiebherrExtList.ElementAt(k).track_tools_auto);
                        res.ImpactSlopeLie = model.LiebherrExtList.ElementAt(k).impact_slope;
                        res.ImpactInterceptLie = model.LiebherrExtList.ElementAt(k).impact_intercept;
                        res.NormalSlopeLie = model.LiebherrExtList.ElementAt(k).normal_slope;
                        res.NormalInterceptLie = model.LiebherrExtList.ElementAt(k).normal_intercept;
                    }
                    resultList.Add(res);
                }
            }
            return resultList;
        }

        public List<LimitsEntity> GetEquipmentLimitsBymoduleSubAuto(string moduleList)
        {
            List<LimitsEntity> resultList = new List<LimitsEntity>();
            var moduleArray = moduleList.Split(',');

            List<BLL.Core.Domain.CompartWornExtViewModel> modelList = new List<BLL.Core.Domain.CompartWornExtViewModel>();
            foreach (var s in moduleArray)
            {
                int moduleId = 0;
                Int32.TryParse(s, out moduleId);
                if (moduleId == 0)
                    continue;
                BLL.Core.Domain.LuModuleSub LogicalEquipment = new BLL.Core.Domain.LuModuleSub(new DAL.UndercarriageContext());
                modelList.AddRange(LogicalEquipment.getWornLimitListBySubModuleId(moduleId));
            }
            foreach (var model in modelList)
            {
                int max = 0;
                if (model.ITMExtList != null && model.ITMExtList.Count > max) max = model.ITMExtList.Count;
                if (model.CATExtList != null && model.CATExtList.Count > max) max = model.CATExtList.Count;
                if (model.KomatsuExtList != null && model.KomatsuExtList.Count > max) max = model.KomatsuExtList.Count;
                if (model.HitachiExtList != null && model.HitachiExtList.Count > max) max = model.HitachiExtList.Count;
                if (model.LiebherrExtList != null && model.LiebherrExtList.Count > max) max = model.LiebherrExtList.Count;

                for (int k = 0; k < max; k++)
                {
                    bool ITM = model.ITMExtList != null && model.ITMExtList.Count > k ? true : false;
                    bool CAT = model.CATExtList != null && model.CATExtList.Count > k ? true : false;
                    bool Komatsu = model.KomatsuExtList != null && model.KomatsuExtList.Count > k ? true : false;
                    bool Hitachi = model.HitachiExtList != null && model.HitachiExtList.Count > k ? true : false;
                    bool Liebherr = model.LiebherrExtList != null && model.LiebherrExtList.Count > k ? true : false;

                    var res = new LimitsEntity
                    {
                        CompartIdAuto = model.Id,
                        Method = getMethodById((int)model.method)
                    };
                    if (ITM)
                    {
                        res.ITMTool = GetToolCode(model.ITMExtList.ElementAt(k).track_tools_auto);
                        res.StartDepthNew = model.ITMExtList.ElementAt(k).start_depth_new;
                        res.WearDepth10Percent = model.ITMExtList.ElementAt(k).wear_depth_10_percent;
                        res.WearDepth20Percent = model.ITMExtList.ElementAt(k).wear_depth_20_percent;
                        res.WearDepth30Percent = model.ITMExtList.ElementAt(k).wear_depth_30_percent;
                        res.WearDepth40Percent = model.ITMExtList.ElementAt(k).wear_depth_40_percent;
                        res.WearDepth50Percent = model.ITMExtList.ElementAt(k).wear_depth_50_percent;
                        res.WearDepth60Percent = model.ITMExtList.ElementAt(k).wear_depth_60_percent;
                        res.WearDepth70Percent = model.ITMExtList.ElementAt(k).wear_depth_70_percent;
                        res.WearDepth80Percent = model.ITMExtList.ElementAt(k).wear_depth_80_percent;
                        res.WearDepth90Percent = model.ITMExtList.ElementAt(k).wear_depth_90_percent;
                        res.WearDepth100Percent = model.ITMExtList.ElementAt(k).wear_depth_100_percent;
                        res.WearDepth110Percent = model.ITMExtList.ElementAt(k).wear_depth_110_percent;
                        res.WearDepth120Percent = model.ITMExtList.ElementAt(k).wear_depth_120_percent;
                    }
                    if (CAT)
                    {
                        res.CATTool = GetToolCode(model.CATExtList.ElementAt(k).track_tools_auto);
                        res.Slope = model.CATExtList.ElementAt(k).slope;
                        res.AdjToBase = model.CATExtList.ElementAt(k).adjust_base;
                        res.HiInflectionPoint = model.CATExtList.ElementAt(k).hi_inflectionPoint;
                        res.HiSlope1 = model.CATExtList.ElementAt(k).hi_slope1;
                        res.HiIntercept1 = model.CATExtList.ElementAt(k).hi_intercept1;
                        res.HiSlope2 = model.CATExtList.ElementAt(k).hi_slope2;
                        res.HiIntercept2 = model.CATExtList.ElementAt(k).hi_intercept2;
                        res.MiInflectionPoint = model.CATExtList.ElementAt(k).mi_inflectionPoint;
                        res.MiSlope1 = model.CATExtList.ElementAt(k).mi_slope1;
                        res.MiIntercept1 = model.CATExtList.ElementAt(k).mi_intercept1;
                        res.MiSlope2 = model.CATExtList.ElementAt(k).mi_slope2;
                        res.MiIntercept2 = model.CATExtList.ElementAt(k).mi_intercept2;
                    }
                    if (Komatsu)
                    {
                        res.KomatsuTool = GetToolCode(model.KomatsuExtList.ElementAt(k).track_tools_auto);
                        res.ImpactSecondOrder = model.KomatsuExtList.ElementAt(k).impact_secondorder;
                        res.NormalSecondOrder = model.KomatsuExtList.ElementAt(k).normal_secondorder;
                        res.ImpactSlope = model.KomatsuExtList.ElementAt(k).impact_slope;
                        res.ImpactIntercept = model.KomatsuExtList.ElementAt(k).impact_intercept;
                        res.NormalSlope = model.KomatsuExtList.ElementAt(k).normal_slope;
                        res.NormalIntercept = model.KomatsuExtList.ElementAt(k).normal_intercept;
                    }

                    if (Hitachi)
                    {
                        res.HitachiTool = GetToolCode(model.HitachiExtList.ElementAt(k).track_tools_auto);
                        res.ImpactSlopeHit = model.HitachiExtList.ElementAt(k).impact_slope;
                        res.ImpactInterceptHit = model.HitachiExtList.ElementAt(k).impact_intercept;
                        res.NormalSlopeHit = model.HitachiExtList.ElementAt(k).normal_slope;
                        res.NormalInterceptHit = model.HitachiExtList.ElementAt(k).normal_intercept;
                    }
                    if (Liebherr)
                    {
                        res.LiebherrTool = GetToolCode(model.LiebherrExtList.ElementAt(k).track_tools_auto);
                        res.ImpactSlopeLie = model.LiebherrExtList.ElementAt(k).impact_slope;
                        res.ImpactInterceptLie = model.LiebherrExtList.ElementAt(k).impact_intercept;
                        res.NormalSlopeLie = model.LiebherrExtList.ElementAt(k).normal_slope;
                        res.NormalInterceptLie = model.LiebherrExtList.ElementAt(k).normal_intercept;
                    }
                    resultList.Add(res);
                }
            }
            return resultList;
        }

        public List<LimitsEntity> GetEquipmentLimitsByCompartIdAuto(string compartIdAutoList)
        {
            List<LimitsEntity> resultList = new List<LimitsEntity>();
            var compartIdAutoArray = compartIdAutoList.Split(',');

            List<BLL.Core.Domain.CompartWornExtViewModel> modelList = new List<BLL.Core.Domain.CompartWornExtViewModel>();
            foreach (var compartIdAuto in compartIdAutoArray)
            {
                int compartAuto = 0;
                Int32.TryParse(compartIdAuto, out compartAuto);
                if (compartAuto == 0)
                    continue;
                BLL.Core.Domain.LuModuleSub LogicalEquipment = new BLL.Core.Domain.LuModuleSub(new DAL.UndercarriageContext());
                modelList.AddRange(LogicalEquipment.getWornLimitListByCompartIdAuto(compartAuto));
            }
            foreach (var model in modelList)
            {
                int max = 0;
                if (model.ITMExtList != null && model.ITMExtList.Count > max) max = model.ITMExtList.Count;
                if (model.CATExtList != null && model.CATExtList.Count > max) max = model.CATExtList.Count;
                if (model.KomatsuExtList != null && model.KomatsuExtList.Count > max) max = model.KomatsuExtList.Count;
                if (model.HitachiExtList != null && model.HitachiExtList.Count > max) max = model.HitachiExtList.Count;
                if (model.LiebherrExtList != null && model.LiebherrExtList.Count > max) max = model.LiebherrExtList.Count;

                for (int k = 0; k < max; k++)
                {
                    bool ITM = model.ITMExtList != null && model.ITMExtList.Count > k ? true : false;
                    bool CAT = model.CATExtList != null && model.CATExtList.Count > k ? true : false;
                    bool Komatsu = model.KomatsuExtList != null && model.KomatsuExtList.Count > k ? true : false;
                    bool Hitachi = model.HitachiExtList != null && model.HitachiExtList.Count > k ? true : false;
                    bool Liebherr = model.LiebherrExtList != null && model.LiebherrExtList.Count > k ? true : false;

                    var res = new LimitsEntity
                    {
                        CompartIdAuto = model.Id,
                        Method = getMethodById((int)model.method)
                    };
                    if (ITM)
                    {
                        res.ITMTool = GetToolCode(model.ITMExtList.ElementAt(k).track_tools_auto);
                        res.StartDepthNew = model.ITMExtList.ElementAt(k).start_depth_new;
                        res.WearDepth10Percent = model.ITMExtList.ElementAt(k).wear_depth_10_percent;
                        res.WearDepth20Percent = model.ITMExtList.ElementAt(k).wear_depth_20_percent;
                        res.WearDepth30Percent = model.ITMExtList.ElementAt(k).wear_depth_30_percent;
                        res.WearDepth40Percent = model.ITMExtList.ElementAt(k).wear_depth_40_percent;
                        res.WearDepth50Percent = model.ITMExtList.ElementAt(k).wear_depth_50_percent;
                        res.WearDepth60Percent = model.ITMExtList.ElementAt(k).wear_depth_60_percent;
                        res.WearDepth70Percent = model.ITMExtList.ElementAt(k).wear_depth_70_percent;
                        res.WearDepth80Percent = model.ITMExtList.ElementAt(k).wear_depth_80_percent;
                        res.WearDepth90Percent = model.ITMExtList.ElementAt(k).wear_depth_90_percent;
                        res.WearDepth100Percent = model.ITMExtList.ElementAt(k).wear_depth_100_percent;
                        res.WearDepth110Percent = model.ITMExtList.ElementAt(k).wear_depth_110_percent;
                        res.WearDepth120Percent = model.ITMExtList.ElementAt(k).wear_depth_120_percent;
                    }
                    if (CAT)
                    {
                        res.CATTool = GetToolCode(model.CATExtList.ElementAt(k).track_tools_auto);
                        res.Slope = model.CATExtList.ElementAt(k).slope;
                        res.AdjToBase = model.CATExtList.ElementAt(k).adjust_base;
                        res.HiInflectionPoint = model.CATExtList.ElementAt(k).hi_inflectionPoint;
                        res.HiSlope1 = model.CATExtList.ElementAt(k).hi_slope1;
                        res.HiIntercept1 = model.CATExtList.ElementAt(k).hi_intercept1;
                        res.HiSlope2 = model.CATExtList.ElementAt(k).hi_slope2;
                        res.HiIntercept2 = model.CATExtList.ElementAt(k).hi_intercept2;
                        res.MiInflectionPoint = model.CATExtList.ElementAt(k).mi_inflectionPoint;
                        res.MiSlope1 = model.CATExtList.ElementAt(k).mi_slope1;
                        res.MiIntercept1 = model.CATExtList.ElementAt(k).mi_intercept1;
                        res.MiSlope2 = model.CATExtList.ElementAt(k).mi_slope2;
                        res.MiIntercept2 = model.CATExtList.ElementAt(k).mi_intercept2;
                    }
                    if (Komatsu)
                    {
                        res.KomatsuTool = GetToolCode(model.KomatsuExtList.ElementAt(k).track_tools_auto);
                        res.ImpactSecondOrder = model.KomatsuExtList.ElementAt(k).impact_secondorder;
                        res.NormalSecondOrder = model.KomatsuExtList.ElementAt(k).normal_secondorder;
                        res.ImpactSlope = model.KomatsuExtList.ElementAt(k).impact_slope;
                        res.ImpactIntercept = model.KomatsuExtList.ElementAt(k).impact_intercept;
                        res.NormalSlope = model.KomatsuExtList.ElementAt(k).normal_slope;
                        res.NormalIntercept = model.KomatsuExtList.ElementAt(k).normal_intercept;
                    }

                    if (Hitachi)
                    {
                        res.HitachiTool = GetToolCode(model.HitachiExtList.ElementAt(k).track_tools_auto);
                        res.ImpactSlopeHit = model.HitachiExtList.ElementAt(k).impact_slope;
                        res.ImpactInterceptHit = model.HitachiExtList.ElementAt(k).impact_intercept;
                        res.NormalSlopeHit = model.HitachiExtList.ElementAt(k).normal_slope;
                        res.NormalInterceptHit = model.HitachiExtList.ElementAt(k).normal_intercept;
                    }
                    if (Liebherr)
                    {
                        res.LiebherrTool = GetToolCode(model.LiebherrExtList.ElementAt(k).track_tools_auto);
                        res.ImpactSlopeLie = model.LiebherrExtList.ElementAt(k).impact_slope;
                        res.ImpactInterceptLie = model.LiebherrExtList.ElementAt(k).impact_intercept;
                        res.NormalSlopeLie = model.LiebherrExtList.ElementAt(k).normal_slope;
                        res.NormalInterceptLie = model.LiebherrExtList.ElementAt(k).normal_intercept;
                    }
                    resultList.Add(res);
                }
            }
            return resultList;
        }

        private string getMethodById(int Id)
        {
            BLL.Core.Domain.WornCalculationMethod method;
            try { method = (BLL.Core.Domain.WornCalculationMethod)Id; } catch { method = BLL.Core.Domain.WornCalculationMethod.None; }
            string result = "None";
            switch (method)
            {
                case BLL.Core.Domain.WornCalculationMethod.ITM:
                    result = "ITM";
                    break;
                case BLL.Core.Domain.WornCalculationMethod.CAT:
                    result = "CAT";
                    break;
                case BLL.Core.Domain.WornCalculationMethod.Komatsu:
                    result = "Komatsu";
                    break;
                case BLL.Core.Domain.WornCalculationMethod.Hitachi:
                    result = "Hitachi";
                    break;
                case BLL.Core.Domain.WornCalculationMethod.Liebherr:
                    result = "Liebherr";
                    break;
            }
            return result;
        }
        private string GetToolCode(int Id)
        {
            if (Id == 1) return "R";
            if (Id == 2) return "DG";
            if (Id == 3) return "UT";
            if (Id == 4) return "C";
            return "R";
        }

        private string GetToolCode(string tool)
        {
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var result = from tools in dataEntities.TRACK_TOOL
                    where tools.tool_name == tool
                    select tools.tool_code;

                return result.Any() ? result.First() : "R";
            }
        }
    }
}