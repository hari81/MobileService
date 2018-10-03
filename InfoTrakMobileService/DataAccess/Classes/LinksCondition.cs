using DAL;
using InfoTrakMobileService.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InfoTrakMobileService.DataAccess.Classes
{
    public class LinksCondition
    {
        private static readonly LinksCondition InstanceLinksCondition = new LinksCondition();

        private LinksCondition()
        {
        }

        public static LinksCondition Instance
        {
            get { return InstanceLinksCondition; }
        }

        public List<DAL.WSREDipTestCondition> GetLinksConditions()
        {
            using(var context = new UndercarriageContext())
            {
                return context.WSREDipTestCondition.ToList();
            }
        }

    }
}