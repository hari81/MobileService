using System.Runtime.Serialization;

namespace InfoTrakMobileService.DataAccess.Entities
{
    [DataContract]
    public class LimitsEntity
    {
        [DataMember]
        public long CompartIdAuto { get; set; }
        [DataMember]
        public int CompartMeasurePointId { get; set; }
        [DataMember]
        public string Method { get; set; }

        #region ITM Method
        [DataMember]
        public string ITMTool { get; set; }
        [DataMember]
        public decimal? StartDepthNew { get; set; }
        [DataMember]
        public decimal? WearDepth10Percent { get; set; }
        [DataMember]
        public decimal? WearDepth20Percent { get; set; }
        [DataMember]
        public decimal? WearDepth30Percent { get; set; }
        [DataMember]
        public decimal? WearDepth40Percent { get; set; }
        [DataMember]
        public decimal? WearDepth50Percent { get; set; }
        [DataMember]
        public decimal? WearDepth60Percent { get; set; }
        [DataMember]
        public decimal? WearDepth70Percent { get; set; }
        [DataMember]
        public decimal? WearDepth80Percent { get; set; }
        [DataMember]
        public decimal? WearDepth90Percent { get; set; }
        [DataMember]
        public decimal? WearDepth100Percent { get; set; }
        [DataMember]
        public decimal? WearDepth110Percent { get; set; }
        [DataMember]
        public decimal? WearDepth120Percent { get; set; }
        #endregion

        #region CAT Method
        [DataMember]
        public string CATTool { get; set; }
        [DataMember]
        public int? Slope { get; set; }
        [DataMember]
        public decimal? AdjToBase { get; set; }
        [DataMember]
        public decimal? HiInflectionPoint { get; set; }
        [DataMember]
        public decimal? HiSlope1 { get; set; }
        [DataMember]
        public decimal? HiIntercept1 { get; set; }
        [DataMember]
        public decimal? HiSlope2 { get; set; }
        [DataMember]
        public decimal? HiIntercept2 { get; set; }
        [DataMember]
        public decimal? MiInflectionPoint { get; set; }
        [DataMember]
        public decimal? MiSlope1 { get; set; }
        [DataMember]
        public decimal? MiIntercept1 { get; set; }
        [DataMember]
        public decimal? MiSlope2 { get; set; }
        [DataMember]
        public decimal? MiIntercept2 { get; set; }
        #endregion

        #region Komatsu Method
        [DataMember]
        public string KomatsuTool { get; set; }
        [DataMember]
        public decimal? ImpactSecondOrder { get; set; }
        [DataMember]
        public decimal? NormalSecondOrder { get; set; }
        [DataMember]
        public decimal? ImpactSlope { get; set; }
        [DataMember]
        public decimal? NormalSlope { get; set; }
        [DataMember]
        public decimal? ImpactIntercept { get; set; }
        [DataMember]
        public decimal? NormalIntercept { get; set; }
        #endregion

        #region Hitachi Method
        [DataMember]
        public string HitachiTool { get; set; }
        [DataMember]
        public decimal? ImpactSlopeHit { get; set; }
        [DataMember]
        public decimal? NormalSlopeHit { get; set; }
        [DataMember]
        public decimal? ImpactInterceptHit { get; set; }
        [DataMember]
        public decimal? NormalInterceptHit { get; set; }
        #endregion

        #region Liebherr Method
        [DataMember]
        public string LiebherrTool { get; set; }
        [DataMember]
        public decimal? ImpactSlopeLie { get; set; }
        [DataMember]
        public decimal? NormalSlopeLie { get; set; }
        [DataMember]
        public decimal? ImpactInterceptLie { get; set; }
        [DataMember]
        public decimal? NormalInterceptLie { get; set; }
        #endregion
    }
}