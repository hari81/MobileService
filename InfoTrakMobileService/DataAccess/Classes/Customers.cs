using System.Collections.Generic;
using System.Linq;
using InfoTrakMobileService.DataAccess.Entities;
using InfoTrakMobileService.DataAccess.Model;

namespace InfoTrakMobileService.DataAccess.Classes
{
    public class Customers
    {
        private static readonly Customers InstanceCustomers = new Customers();

        private Customers()
        {
        }

        public static Customers Instance
        {
            get { return InstanceCustomers; }
        }

        /// <summary>
        /// Returns the list with all the Customes in the sytem.
        /// </summary>
        public List<CustomerEntity> List
        {
            get { return GetCustomers(); }
        }

        private static List<CustomerEntity> GetCustomers()
        {
            var result = new List<CustomerEntity>();

            using (var dataEntities = new InfoTrakDataEntities())
            {
                var customers = from customer in dataEntities.CUSTOMERs
                    select customer;

                result.AddRange(
                    customers.Select(
                        customer =>
                            new CustomerEntity {CustomerId = customer.customer_auto, CustomerName = customer.cust_name}));
            }

            return result;
        }
    }
}