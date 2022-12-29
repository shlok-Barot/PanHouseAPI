using MySql.Data.MySqlClient;
using PanHouse.Interface;
using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PanHouse.Services
{
    public class CustomerService : ICustomer
    {
        #region Cunstructor
        MySqlConnection conn = new MySqlConnection();
        public CustomerService(string _connecctionString)
        {
            conn.ConnectionString = _connecctionString;
        }
        #endregion

        #region Custom method
        /// <summary>
        /// GetCustomerList
        /// </summary>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public List<CustomerMaster> GetCustomerList(string CustomerName,int TenantID) 
        {
            List<CustomerMaster> customerList = new List<CustomerMaster>();
            MySqlCommand cmd = new MySqlCommand();
            DataSet ds = new DataSet();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_GetCustomerList", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Tenant_Id", TenantID);
                cmd1.Parameters.AddWithValue("@Customer_name", CustomerName);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd1;
                da.Fill(ds);

                if (ds != null && ds.Tables[0] != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CustomerMaster customerMaster = new CustomerMaster();
                        customerMaster.CustomerID = Convert.ToInt32(ds.Tables[0].Rows[i]["CustomerMasterID"]);
                        customerMaster.CustomerName = Convert.ToString(ds.Tables[0].Rows[i]["CustomerName"]);
                        customerMaster.CustomerPhoneNumber = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]);
                        customerMaster.CustomerEmailId = Convert.ToString(ds.Tables[0].Rows[i]["CustomerEmailID"]);
                        customerMaster.Gender = Convert.ToString(ds.Tables[0].Rows[i]["Gender"]);
                        customerMaster.CustomerAddress = Convert.ToString(ds.Tables[0].Rows[i]["Address"]);
                        customerMaster.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"]);

                        customerList.Add(customerMaster);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return customerList;
        }
        /// <summary>
        /// GetCustomername
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public List<CustomerMaster> GetCustomername(string searchText, int TenantID)
        {
            List<CustomerMaster> customerList = new List<CustomerMaster>();
            MySqlCommand cmd = new MySqlCommand();
            DataSet ds = new DataSet();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_GetCustomerName", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Tenant_Id", TenantID);
                cmd1.Parameters.AddWithValue("@SearchText", searchText);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd1;
                da.Fill(ds);

                if (ds != null && ds.Tables[0] != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CustomerMaster customerMaster = new CustomerMaster();
                        customerMaster.CustomerID = Convert.ToInt32(ds.Tables[0].Rows[i]["CustomerMasterID"]);
                        customerMaster.CustomerName = Convert.ToString(ds.Tables[0].Rows[i]["CustomerName"]);

                        customerList.Add(customerMaster);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return customerList;
        }

        /// <summary>
        /// Add customer Details
        /// </summary>
        /// <param name="customerMaster"></param>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        public int addCustomerDetails(CustomerMaster customerMaster, int TenantId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int Success = 0;
            try
            {

                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_CreateCustomer", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@TenantID", TenantId);
                cmd1.Parameters.AddWithValue("@CustomerName", customerMaster.CustomerName);
                cmd1.Parameters.AddWithValue("@customerPhoneNumber", customerMaster.CustomerPhoneNumber);
                cmd1.Parameters.AddWithValue("@customerEmailId", customerMaster.CustomerEmailId);
                cmd1.Parameters.AddWithValue("@Gender", customerMaster.Gender);
                cmd1.Parameters.AddWithValue("@CustomerAddress", customerMaster.CustomerAddress);
                cmd1.Parameters.AddWithValue("@CreatedBy", customerMaster.CreatedBy);

                Success = Convert.ToInt32(cmd1.ExecuteScalar());
                conn.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return Success;
        }

        /// <summary>
        /// Update customer Details
        /// </summary>
        /// <param name="customerMaster"></param>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        public int updateCustomerDetails(CustomerMaster customerMaster, int TenantId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int Success = 0;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_UpdateCustomer", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@CustomerID", customerMaster.CustomerID);
                cmd1.Parameters.AddWithValue("@TenantID", TenantId);
                cmd1.Parameters.AddWithValue("@CustomerName", customerMaster.CustomerName);
                cmd1.Parameters.AddWithValue("@PhoneNumber", customerMaster.CustomerPhoneNumber);
                cmd1.Parameters.AddWithValue("@EmailId", customerMaster.CustomerEmailId);
                cmd1.Parameters.AddWithValue("@Gender", customerMaster.Gender);
                cmd1.Parameters.AddWithValue("@Address", customerMaster.CustomerAddress);
                cmd1.Parameters.AddWithValue("@IsActive", customerMaster.IsActive);
                cmd1.Parameters.AddWithValue("@Modified_By", customerMaster.ModifyBy);

                Success = Convert.ToInt32(cmd1.ExecuteNonQuery());
                conn.Close();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return Success;
        }

        /// <summary>
        /// DeleteCustomer
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        public int DeleteCustomer(int CustomerID, int TenantId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int Success = 0;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_DeleteCustomer", conn);
                cmd1.Parameters.AddWithValue("@Customer_ID", CustomerID);
                cmd1.Parameters.AddWithValue("@Tenant_ID", TenantId);
                cmd1.CommandType = CommandType.StoredProcedure;
                Success = Convert.ToInt32(cmd1.ExecuteScalar());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return Success;
        }

        #endregion
    }
}
