using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfoTrakMobileService.DataAccess.Entities;
using InfoTrakMobileService.DataAccess.Model;

namespace InfoTrakMobileService.DataAccess.Classes
{
    public class GETs
    {
        private static readonly GETs InstanceGETs = new GETs();

        private GETs()
        {
        }

        public static GETs Instance
        {
            get { return InstanceGETs; }
        }

        public List<GETEntity> GetImplementsByEquipment(long equipmentAuto, string serial_no)
        {
            var result = new List<GETEntity>();
            using (var dataEntities = new InfoTrakDataEntities())
            {
                // This SQL is converted from procedure: [return_get_equip_implement_with_no_implementcategory]
                var mmtaid_auto = (from equipment in dataEntities.EQUIPMENTs
                                  where equipment.equipmentid_auto == equipmentAuto
                                  select new { equipment.mmtaid_auto }).Single();
                int selectedMmtaid = mmtaid_auto.mmtaid_auto;

                var model_auto = (from lu_mmta in dataEntities.LU_MMTA
                                  where lu_mmta.mmtaid_auto == selectedMmtaid
                                  select new { lu_mmta.model_auto }).Single();
                int selectedModel = model_auto.model_auto;

                var implement_auto = from get_implement_make_model in dataEntities.GET_IMPLEMENT_MAKE_MODEL
                                      where get_implement_make_model.model_auto == selectedModel
                                     select get_implement_make_model.implement_auto;
                long[] selectedImplementAuto = implement_auto.ToArray();

                var implements = from get in dataEntities.GETs
                                     join lu_implement in dataEntities.LU_IMPLEMENT
                                        on get.implement_auto equals lu_implement.implement_auto
                                     join equipment in dataEntities.EQUIPMENTs on get.equipmentid_auto equals equipment.equipmentid_auto
                                     join make in dataEntities.MAKEs on get.make_auto equals make.make_auto
                                 where (
                                    get.equipmentid_auto == equipmentAuto
                                    && selectedImplementAuto.Contains(lu_implement.implement_auto)
                                 )
                                 orderby get.impserial
                                 select get;

                result.AddRange(
                    implements.Select(
                        get => new GETEntity { 
                            get_auto = get.get_auto, 
                            impserial = get.impserial, 
                            implement_auto = get.implement_auto, 
                            equipmentid_auto = get.equipmentid_auto,
                            serial_no = serial_no
                        }));
            }
            return result;
        }
    }
}