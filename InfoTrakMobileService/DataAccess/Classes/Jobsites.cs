using System.Collections.Generic;
using System.Linq;
using InfoTrakMobileService.DataAccess.Entities;
using InfoTrakMobileService.DataAccess.Model;

namespace InfoTrakMobileService.DataAccess.Classes
{
    public class Jobsites
    {
        private static readonly Jobsites InstanceCustomers = new Jobsites();

        private Jobsites()
        {
        }

        public static Jobsites Instance
        {
            get { return InstanceCustomers; }
        }

        /// <summary>
        /// Returns the list with all the Jobsites in the system.
        /// </summary>
        public List<JobsiteEntity> List
        {
            get { return GetJobsites(); }
        }

        private static List<JobsiteEntity> GetJobsites()
        {
            var result = new List<JobsiteEntity>();

            using (var dataEntities = new InfoTrakDataEntities())
            {
                IQueryable<CRSF> jobsites = from jobsite in dataEntities.CRSFs
                    select jobsite;

                result.AddRange(
                    jobsites.Select(
                        jobsite => new JobsiteEntity {JobsiteId = jobsite.crsf_auto, JobsiteName = jobsite.site_name}));
            }

            return result;
        }

        /// <summary>
        /// Gets the Jobsite details given a jobsite auto number. This is used in the Equipment Search screen for the Mobile App.
        /// </summary>
        /// <param name="jobsiteAuto">Jobsite auto number</param>
        /// <returns>List of JobsiteEntity with the results.</returns>
        public List<JobsiteEntity> GetJobsiteById(long jobsiteAuto)
        {
            var result = new List<JobsiteEntity>();
            using (var dataEntities = new InfoTrakDataEntities())
            {
                IQueryable<CRSF> jobsites = from jobsite in dataEntities.CRSFs
                    where jobsite.crsf_auto == jobsiteAuto
                    select jobsite;

                result.AddRange(
                    jobsites.Select(
                        jobsite => new JobsiteEntity {JobsiteId = jobsite.crsf_auto, JobsiteName = jobsite.site_name}));
            }
            return result;
        }

        /// <summary>
        /// Get all Jobsites for a given customer. This is used in the Equipment Search screen for the Mobile App.
        /// </summary>
        /// <param name="customerAuto">Customer auto number</param>
        /// <returns>Returns a List of JobsiteEntity with the results</returns>
        public List<JobsiteEntity> GetJobsitesByCustomer(long customerAuto)
        {
            var result = new List<JobsiteEntity>();
            using (var dataEntities = new InfoTrakDataEntities())
            {
                var jobsites = from jobsite in dataEntities.CRSFs
                    join customer in dataEntities.CUSTOMERs on jobsite.customer_auto equals customer.customer_auto
                    where customer.customer_auto == customerAuto
                    select jobsite;

                result.AddRange(
                    jobsites.Select(
                        jobsite => new JobsiteEntity {JobsiteId = jobsite.crsf_auto, JobsiteName = jobsite.site_name}));
            }
            return result;
        }
    }
}