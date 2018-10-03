using System.Collections.Generic;
using System.Linq;
using InfoTrakMobileService.DataAccess.Entities;
using InfoTrakMobileService.DataAccess.Model;

namespace InfoTrakMobileService.DataAccess.Classes
{
    public class Models
    {
        private static readonly Models InstanceModels = new Models();

        private Models()
        {
        }

        public static Models Instance
        {
            get { return InstanceModels; }
        }

        /// <summary>
        /// Returns the list with all the Models in the system.
        /// </summary>
        public List<ModelEntity> List
        {
            get { return GetModels(); }
        }

        private static List<ModelEntity> GetModels()
        {
            var result = new List<ModelEntity>();

            using (var dataEntities = new InfoTrakDataEntities())
            {
                IQueryable<MODEL> models = from model in dataEntities.MODELs
                    select model;

                result.AddRange(
                    models.Select(model => new ModelEntity {ModelId = model.model_auto, ModelName = model.modelid}));
            }

            return result;
        }

        /// <summary>
        /// Get Models for a given Jobsite. This is used in the Equipment Search Screen for the Mobile App
        /// </summary>
        /// <param name="jobsiteAuto">Jobsite auto number</param>
        /// <returns>Returns a List of ModelEntity with the results.</returns>
        public List<ModelEntity> GetModelsByJobsite(long jobsiteAuto)
        {
            var result = new List<ModelEntity>();

            using (var dataEntities = new InfoTrakDataEntities())
            {
                var models = from model in dataEntities.MODELs
                    join mmta in dataEntities.LU_MMTA on model.model_auto equals mmta.model_auto
                    join equipment in dataEntities.EQUIPMENTs on mmta.mmtaid_auto equals equipment.mmtaid_auto
                    where equipment.crsf_auto == jobsiteAuto
                    select model;

                result.AddRange(
                    models.Select(model => new ModelEntity {ModelId = model.model_auto, ModelName = model.modelid}).Distinct());
            }

            return result;
        }
    }
}