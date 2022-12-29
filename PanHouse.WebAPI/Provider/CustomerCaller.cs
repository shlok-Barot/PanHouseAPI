using PanHouse.Interface;
using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanHouse.WebAPI.Provider
{
    public class CustomerCaller
    {
        #region variable
        public ICustomer _customerRepository;
        #endregion

        #region Customer method
        /// <summary>
        /// GetcustomerList
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public List<CustomerMaster> GetcustomerList(ICustomer customer, string CustomerName, int TenantID)
        {
            _customerRepository = customer;
            return _customerRepository.GetCustomerList(CustomerName,TenantID);
        }

        /// <summary>
        /// Add customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="customerMaster"></param>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        public int addCustomer(ICustomer customer, CustomerMaster customerMaster, int TenantId)
        {
            _customerRepository = customer;
            return _customerRepository.addCustomerDetails(customerMaster, TenantId);
        }

        /// <summary>
        /// Update customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="customerMaster"></param>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        public int updateCustomer(ICustomer customer, CustomerMaster customerMaster, int TenantId)
        {
            _customerRepository = customer;
            return _customerRepository.updateCustomerDetails(customerMaster, TenantId);
        }

        /// <summary>
        /// DeleteCustomer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="CustomerID"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public int DeleteCustomer(ICustomer customer, int CustomerID, int TenantID)
        {
            _customerRepository = customer;
            return _customerRepository.DeleteCustomer(CustomerID, TenantID);
        }
        /// <summary>
        /// GetcustomerList
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="TenantID"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public List<CustomerMaster> GetCustomerName(ICustomer customer, string searchText, int TenantID)
        {
            _customerRepository = customer;
            return _customerRepository.GetCustomername(searchText,TenantID);
        }
        #endregion
    }
}
