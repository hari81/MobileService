using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfoTrakMobileService.DataAccess.Entities;
using InfoTrakMobileService.DataAccess.Model;

namespace InfoTrakMobileService.DataAccess.Classes
{
    public class DealershipLimits
    {
         private static readonly DealershipLimits InstanceComponent = new DealershipLimits();

         private DealershipLimits()
        {
        }

         public static DealershipLimits Instance
        {
            get { return InstanceComponent; }
        }

        /// <summary>
        /// Get Dealership Limits for the Mobile App.
        /// </summary>
       
        /// <returns>List of Dealership Limits</returns>
        public List<DealershipLimitEntity> GetDealershipLimits()
        {
            var result = new List<DealershipLimitEntity>();
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var limitQuery = from limits in dataEntities.TRACK_DEALERSHIP_LIMITS
                    select limits;

                result.AddRange(limitQuery.Select(trackDealershipLimits => new DealershipLimitEntity
                {
                    ALimit = trackDealershipLimits.a_limit, BLimit = trackDealershipLimits.b_limit, CLimit = trackDealershipLimits.c_limit
                }));
            }
            return result;
        }
    }
}