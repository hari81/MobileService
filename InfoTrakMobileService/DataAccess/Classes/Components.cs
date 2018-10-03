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
    public class Components
    {
        private static readonly Components InstanceComponent = new Components();

        private Components()
        {
        }

        public static Components Instance
        {
            get { return InstanceComponent; }
        }

        /// <summary>
        /// Get all component details for the selected equipment. This is used in the Equipment Search screen for the Mobile App.
        /// </summary>
        /// <param name="equipmentList">Comma separated equipment auto list</param>
        /// <returns>List of ComponentEntity for the selected equipment</returns>
        public List<ComponentEntity> GetSelectedComponents(String equipmentList)
        {

            var result = new List<ComponentEntity>();
            int temp = 0;
            var equipmentArrayInt = equipmentList.Split(',').Select(m=> int.TryParse(m,out temp) ? int.Parse(m):0);
            var lastInspections = new List<IEnumerable<BLL.Core.ViewModel.ComponentWornReadingViewModel>>();
            
            using (var dalDataEntities = new DAL.UndercarriageContext()) {
                lastInspections = dalDataEntities.TRACK_INSPECTION.Where(m => equipmentArrayInt.Any(k => k == m.equipmentid_auto)).GroupBy(m => m.equipmentid_auto).Select(group => group.OrderByDescending(m => m.inspection_date).FirstOrDefault())
                    .Select(m =>  m.TRACK_INSPECTION_DETAIL.Select(k=> new BLL.Core.ViewModel.ComponentWornReadingViewModel { Id = k.inspection_detail_auto, ComponentId = (int)k.track_unit_auto, Reading = k.reading, WornPercentage = k.worn_percentage, EquipmentId = (int)k.TRACK_INSPECTION.equipmentid_auto, ToolId = k.tool_auto ?? 0, ToolSymbol = (k.TRACK_TOOL != null ? k.TRACK_TOOL.tool_code : "UN")} )).ToList();
                
            }
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var equipmentArray = equipmentList.Split(',');
                var components = from generalComponents in dataEntities.GENERAL_EQ_UNIT
                                 join comp in dataEntities.LU_COMPART on generalComponents.compartid_auto equals comp.compartid_auto
                                 join equipment in dataEntities.EQUIPMENTs on generalComponents.equipmentid_auto equals equipment.equipmentid_auto
                                 join crsf in dataEntities.CRSFs on equipment.crsf_auto equals crsf.crsf_auto
                                 join cust in dataEntities.CUSTOMERs on crsf.customer_auto equals cust.customer_auto
                                 join mmta in dataEntities.LU_MMTA on equipment.mmtaid_auto equals mmta.mmtaid_auto
                                 join family in dataEntities.TYPEs on mmta.type_auto equals family.type_auto
                                 join model in dataEntities.MODELs on mmta.model_auto equals model.model_auto
                                 join compType in dataEntities.LU_COMPART_TYPE on comp.comparttype_auto equals compType.comparttype_auto
                                 join system in dataEntities.LU_SYSTEM on compType.system_auto equals system.system_auto
                                 where equipmentArray.Contains(equipment.equipmentid_auto.ToString()) && (system.system_desc == "undercarriage" || system.system_desc == "U/C")
                                 orderby generalComponents.side, compType.sorder, generalComponents.pos

                                 select
                        new
                        {
                            comp.compartid_auto,
                            generalComponents.equnit_auto,
                            comp.comparttype_auto,
                            comp.compartid,
                            comp.compart,
                            compType.comparttype,
                            generalComponents.side,
                            generalComponents.pos,
                            equipment.equipmentid_auto
                        };


                foreach (var component in components)
                {
                    var newComponent = new ComponentEntity
                    {
                        ComponentId = component.equnit_auto,
                        ComponentType = component.comparttype_auto,
                        ComponentIdAuto = component.compartid_auto,
                        PartNo = component.comparttype + ": " + component.compartid,
                        ComponentName = component.compart,
                        ComponentSide = (Convert.ToInt16(component.side) == 1) ? "Left" : "Right",
                        ComponentPosition = (Convert.ToInt32(component.pos)),
                        ComponentImage = GetComponentImage(component.comparttype_auto),
                        DefaultTool = GetComponentDefaultTool(component.compartid_auto),
                        ComponentMethod = GetComponentMethod(component.compartid_auto),
                        EquipmentId = component.equipmentid_auto,
                        LastReading = (lastInspections.Where(m => m.Any(k => k.EquipmentId == component.equipmentid_auto)).Count() > 0 && lastInspections.Where(m => m.Any(k => k.EquipmentId == component.equipmentid_auto)).FirstOrDefault().Where(k => k.ComponentId == component.equnit_auto).Count() > 0) ? (double)lastInspections.Where(m => m.Any(k => k.EquipmentId == component.equipmentid_auto)).FirstOrDefault().Where(k => k.ComponentId == component.equnit_auto).FirstOrDefault().Reading : 0,
                        LastWornPercentage = Decimal.ToInt32((lastInspections.Where(m => m.Any(k => k.EquipmentId == component.equipmentid_auto)).Count() > 0 && lastInspections.Where(m => m.Any(k => k.EquipmentId == component.equipmentid_auto)).FirstOrDefault().Where(k => k.ComponentId == component.equnit_auto).Count() > 0) ? lastInspections.Where(m => m.Any(k => k.EquipmentId == component.equipmentid_auto)).FirstOrDefault().Where(k => k.ComponentId == component.equnit_auto).FirstOrDefault().WornPercentage : Decimal.Zero),
                        ToolId = (lastInspections.Where(m => m.Any(k => k.EquipmentId == component.equipmentid_auto)).Count() > 0 && lastInspections.Where(m => m.Any(k => k.EquipmentId == component.equipmentid_auto)).FirstOrDefault().Where(k => k.ComponentId == component.equnit_auto).Count() > 0) ? lastInspections.Where(m => m.Any(k => k.EquipmentId == component.equipmentid_auto)).FirstOrDefault().Where(k => k.ComponentId == component.equnit_auto).FirstOrDefault().ToolId : 0,
                        ToolSymbol = (lastInspections.Where(m => m.Any(k => k.EquipmentId == component.equipmentid_auto)).Count() > 0 && lastInspections.Where(m => m.Any(k => k.EquipmentId == component.equipmentid_auto)).FirstOrDefault().Where(k => k.ComponentId == component.equnit_auto).Count() > 0) ? lastInspections.Where(m => m.Any(k => k.EquipmentId == component.equipmentid_auto)).FirstOrDefault().Where(k => k.ComponentId == component.equnit_auto).FirstOrDefault().ToolSymbol : "UN"
                    };

                    result.Add(newComponent);
                }
            }
            return result;
        }

        public List<ComponentEntity> GetSelectedComponentsByModuleSubAuto(String moduleSubAutoList)
        {
            var result = new List<ComponentEntity>();
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var moduleArray = moduleSubAutoList.Split(',');
                var components = from generalComponents in dataEntities.GENERAL_EQ_UNIT
                                 join comp in dataEntities.LU_COMPART on generalComponents.compartid_auto equals comp.compartid_auto
                                 //join equipment in dataEntities.EQUIPMENTs on generalComponents.equipmentid_auto equals equipment.equipmentid_auto
                                 join module in dataEntities.LU_Module_Sub on generalComponents.module_ucsub_auto equals module.Module_sub_auto
                                 join crsf in dataEntities.CRSFs on module.crsf_auto equals crsf.crsf_auto
                                 join cust in dataEntities.CUSTOMERs on crsf.customer_auto equals cust.customer_auto
                                 //join mmta in dataEntities.LU_MMTA on module.mmtaid_auto equals mmta.mmtaid_auto
                                 //join family in dataEntities.TYPEs on mmta.type_auto equals family.type_auto
                                 join model in dataEntities.MODELs on module.model_auto equals model.model_auto
                                 join compType in dataEntities.LU_COMPART_TYPE on comp.comparttype_auto equals compType.comparttype_auto
                                 join system in dataEntities.LU_SYSTEM on compType.system_auto equals system.system_auto
                                 where moduleArray.Contains(module.Module_sub_auto.ToString()) && (system.system_desc == "undercarriage" || system.system_desc == "U/C")
                                 orderby generalComponents.side, compType.sorder, generalComponents.pos

                                 select
                        new
                        {
                            comp.compartid_auto,
                            generalComponents.equnit_auto,
                            comp.comparttype_auto,
                            comp.compartid,
                            comp.compart,
                            compType.comparttype,
                            generalComponents.side,
                            generalComponents.pos,
                            module.Module_sub_auto
                        };


                foreach (var component in components)
                {
                    var newComponent = new ComponentEntity
                    {
                        ComponentId = component.equnit_auto,
                        ComponentType = component.comparttype_auto,
                        ComponentIdAuto = component.compartid_auto,
                        PartNo = component.comparttype + ": " + component.compartid,
                        ComponentName = component.compart,
                        ComponentSide = (Convert.ToInt16(component.side) == 1) ? "Left" : "Right",
                        ComponentPosition = (Convert.ToInt32(component.pos)),
                        ComponentImage = GetComponentImage(component.comparttype_auto),
                        DefaultTool = GetComponentDefaultTool(component.compartid_auto),
                        ComponentMethod = GetComponentMethod(component.compartid_auto),
                        ModuleSubAuto = component.Module_sub_auto,
                    };

                    result.Add(newComponent);
                }
            }
            return result;
        }

        private string GetComponentMethod(int compartidAuto)
        {
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var method = from methods in dataEntities.TRACK_COMPART_WORN_CALC_METHOD
                           join compEx in dataEntities.TRACK_COMPART_EXT on methods.track_compart_worn_calc_method_auto equals compEx.track_compart_worn_calc_method_auto
                           where compEx.compartid_auto == compartidAuto
                           select methods.track_compart_worn_calc_method_name;

                return method.Any() ? method.First() : "ITM";
            }
        }

        private static string GetComponentDefaultTool(int compartidAuto)
        {
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var tool = from tools in dataEntities.TRACK_COMPART_EXT
                    where tools.compartid_auto == compartidAuto
                    select tools.tools_auto;

                return tool.Any() ? TestPointImages.GetToolCode(tool.First()) : "R";
            }
        }

        private static byte[] GetComponentImage(int comparttypeAuto)
        {
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var result = from componentAttachment in dataEntities.COMPART_ATTACH_FILESTREAM
                    join componentAttachmentType in dataEntities.COMPART_ATTACH_TYPE on
                        componentAttachment.compart_attach_type_auto equals
                        componentAttachmentType.compart_attach_type_auto
                    where
                        componentAttachment.comparttype_auto == comparttypeAuto &&
                        componentAttachmentType.compart_attach_type_name == "comparttype_image"
                    select componentAttachment.attachment;

                if (result.Any())
                {
                    return result.First();
                }

            }

            return null;
        }

        public int? GetPosition(long trackUnitAuto)
        {
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var result = from component in dataEntities.GENERAL_EQ_UNIT
                    where component.equnit_auto == trackUnitAuto
                    select component.pos;

                if (result.Any())
                {
                    return result.First();
                }

            }

            return 1;
        }

        public int? GetPositionforNewEquip(long trackUnitAuto)
        {
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var result = from component in dataEntities.Mbl_NewGENERAL_EQ_UNIT
                             where component.compartid_auto == trackUnitAuto
                             select component.pos;

                if (result.Any())
                {
                    return result.First();
                }

            }

            return 1;
        }
    }
}