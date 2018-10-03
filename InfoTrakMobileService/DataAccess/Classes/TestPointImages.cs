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
    public class TestPointImages
    {
        private static readonly TestPointImages InstanceTestPointImages = new TestPointImages();

        private TestPointImages()
        {
        }

        public static TestPointImages Instance
        {
            get { return InstanceTestPointImages; }
        }

        /// <summary>
        /// Get all Test Point Images. This is used in the Equipment Search screen for the Mobile App.
        /// </summary>
        /// <returns>List of TestPointImageEntity</returns>
        public List<TestPointImageEntity> GetTestPointImages()
        {
            var result = new List<TestPointImageEntity>();
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var testPointImages = from images in dataEntities.COMPART_ATTACH_FILESTREAM
                                      join testPointType in dataEntities.COMPART_ATTACH_TYPE on images.compart_attach_type_auto equals testPointType.compart_attach_type_auto
                                 where testPointType.compart_attach_type_name == "compart_testing_point_image"
                                 select
                        new
                        {
                            images.comparttype_auto,
                            images.tool_auto,
                            images.attachment
                            
                        };


                foreach (var testPointImage in testPointImages)
                {
                    var newTpi = new TestPointImageEntity
                    {
                        CompartType = (long) testPointImage.comparttype_auto,
                        Tool = GetToolCode(testPointImage.tool_auto),
                        TestPointImage = testPointImage.attachment
                        //TestPointImage = null
                    };

                    result.Add(newTpi);
                }
            }
            return result;
        }

        public static string GetToolCode(int? toolAuto)
        {
            if (toolAuto == null) return "R";
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var toolCode = from tool in dataEntities.TRACK_TOOL
                    where tool.tool_auto == (int) toolAuto
                    select tool.tool_code;

                return toolCode.Any() ? toolCode.First() : "R"; // R by default
            }
        }
    }
}