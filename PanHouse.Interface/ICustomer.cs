using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Interface
{
    /// <summary>
    /// Interface for the Customer
    /// </summary>
    public interface ICustomer
    {
        List<CustomerMaster> GetCustomerList(string CustomerName, int TenantID);

        int addCustomerDetails(CustomerMaster customerMaster, int TenantId);

        int updateCustomerDetails(CustomerMaster customerMaster, int TenantId);

        int DeleteCustomer(int CustomerID, int TenantID);

        List<CustomerMaster> GetCustomername(string searchText, int TenantId);
    }
}
