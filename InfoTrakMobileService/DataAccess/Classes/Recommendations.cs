using DAL;
using InfoTrakMobileService.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InfoTrakMobileService.DataAccess.Classes
{
    public class Recommendations
    {
        private static readonly Recommendations InstanceRecommendation = new Recommendations();

        private Recommendations()
        {
        }

        public static Recommendations Instance
        {
            get { return InstanceRecommendation; }
        }

        public List<RecommendationEntity> GetRecommendationByCompartment(long compartment)
        {
            //using (var context = new UndercarriageContext())
            //{
            //    return context.WSREComponentRecommendation.Select(c => new RecommendationEntity()
            //    {
            //        Description = c.Description,
            //        Id = c.Id,
            //        ValidComponentId = c.ValidComponentTypeId
            //    }).ToList();
            //}
            var result = new List<RecommendationEntity>();
            using (var dataEntities = new UndercarriageContext())
            {
                var items = dataEntities.Database.SqlQuery<RecommendationEntity>(
                    "select * from WSREComponentRecommendations"
                    + " where ValidComponentTypeId = @ValidComponentTypeId"
                    , new SqlParameter("@ValidComponentTypeId", compartment)
                ).ToList();

                foreach (var item in items)
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}