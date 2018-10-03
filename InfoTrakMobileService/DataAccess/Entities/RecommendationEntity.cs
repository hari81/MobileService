using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfoTrakMobileService.DataAccess.Classes
{
    public class RecommendationEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ValidComponentTypeId { get; set; }
    }
}