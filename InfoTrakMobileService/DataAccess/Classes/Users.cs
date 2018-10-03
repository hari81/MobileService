using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfoTrakMobileService.DataAccess.Model;
using InfoTrakMobileService.DataAccess.Entities;
using BLL.Extensions;
namespace InfoTrakMobileService.DataAccess.Classes
{
    public class Users
    {
        private static readonly Users InstanceComponent = new Users();

        private Users()
        {
        }

        public static Users Instance
        {
            get { return InstanceComponent; }
        }

        /// <summary>
        /// Authenticate user for the Mobile App.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Returns true or false depending on the authentication process.</returns>
        public bool Authenticate(String username, String password)
        {
            if (username.Contains('*'))
                username = username.Replace('*', ' ');

            bool result;
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var user = from users in dataEntities.USER_TABLE
                    where users.userid == username && users.passwd == password
                    select users;

                result = user.Any();
            }
            return result;
        }

        public int getUserId(string userName)
        {
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var users = dataEntities.USER_TABLE.Where(m => m.userid == userName);
                if (users.Count() > 0)
                {
                    return users.First().user_auto.LongNullableToInt();
                }
            }
            return 0;
        }

        public UserPreferenceEntity GetUserPreferenceById(string userId)
        {
            if (userId.Contains('*'))
                userId = userId.Replace('*', ' ');

            UserPreferenceEntity result = new UserPreferenceEntity();


            result.UndercarriagUOM = "INCH"; 

            using (var dataEntities = new InfoTrakDataEntities())
            {
                var userall = from users in dataEntities.USER_TABLE
                           where string.Compare(users.userid,userId) == 0 
                           select users;

                var user = userall.FirstOrDefault();

                if (user != null)
                {
                    result.UndercarriagUOM =  string.IsNullOrEmpty(user.track_uom) ? "INCH" : user.track_uom.ToUpper()   ;
                }
                
            }

            return result; 
        }

        public string GetApplicationVersion()
        {
            string strVersion = string.Empty;
            using (var dataEntities = new InfoTrakDataEntities())
            {
                System.Data.Entity.Core.Objects.ObjectResult<InfoTrakMobileService.DataAccess.Model.spGetMobileAppVersion_Result> r = dataEntities.spGetMobileAppVersion();
                //strVersion = r. .value_key;
                strVersion = r.FirstOrDefault().value_key;
            }

            return strVersion;
        }
    }
}