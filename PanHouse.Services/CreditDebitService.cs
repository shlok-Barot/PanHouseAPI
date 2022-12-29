using MySql.Data.MySqlClient;
using PanHouse.Interface;
using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PanHouse.Services
{
    public class CreditDebitService : ICreditDebit
    {
        #region Cunstructor
        MySqlConnection conn = new MySqlConnection();
        public CreditDebitService(string _connecctionString)
        {
            conn.ConnectionString = _connecctionString;
        }
        #endregion

        #region Custom Method
        /// <summary>
        /// getCreditDebitList
        /// </summary>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public List<CreditDebitModel> getCreditDebitList(int TenantID)
        {
            List<CreditDebitModel> creditDebitList = new List<CreditDebitModel>();
            DataSet ds = new DataSet();

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SP_GetCustomerCreditDebitList", conn);
                cmd.Parameters.AddWithValue("@Tenant_Id", TenantID);
                cmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                if (ds != null && ds.Tables[0] != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CreditDebitModel creditDebitModel = new CreditDebitModel();
                        creditDebitModel.CustomerID = ds.Tables[0].Rows[i]["CustomerMasterID"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["CustomerMasterID"]);
                        creditDebitModel.CustomerName = ds.Tables[0].Rows[i]["CustomerName"] == DBNull.Value ? string.Empty : Convert.ToString(ds.Tables[0].Rows[i]["CustomerName"]);
                        creditDebitModel.DebitAmount = ds.Tables[0].Rows[i]["DebitAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["DebitAmount"]);
                        creditDebitModel.CreditAmount = ds.Tables[0].Rows[i]["CreditAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["CreditAmount"]);
                        creditDebitModel.NeedToPay = ds.Tables[0].Rows[i]["NeedToPay"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["NeedToPay"]);
                        creditDebitModel.ExtraCredit = ds.Tables[0].Rows[i]["ExtraCredited"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["ExtraCredited"]);

                        creditDebitList.Add(creditDebitModel);
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

            return creditDebitList;
        }


        /// <summary>
        /// AddCreditDetails
        /// </summary>
        /// <param name="creditModel"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public int addCreditDetails(CreditModel creditModel, int TenantID)
        {
            MySqlCommand cmd = new MySqlCommand();
            int Success = 0;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_CreateCreditData", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@TenantID", TenantID);
                cmd1.Parameters.AddWithValue("@CustomerID", creditModel.CustomerID);
                cmd1.Parameters.AddWithValue("@CreditAmount", creditModel.CreditAmount);
                cmd1.Parameters.AddWithValue("@Credit_Description", creditModel.Description);
                cmd1.Parameters.AddWithValue("@CreatedBy", creditModel.CreatedBy);

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
        /// addDebitDetails
        /// </summary>
        /// <param name="debitModel"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public int addDebitDetails(DebitModel debitModel, int TenantID)
        {
            MySqlCommand cmd = new MySqlCommand();
            int Success = 0;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_CreateDebitData", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@TenantID", TenantID);
                cmd1.Parameters.AddWithValue("@CustomerID", debitModel.CustomerID);
                cmd1.Parameters.AddWithValue("@DebitAmount", debitModel.DebitAmount);
                cmd1.Parameters.AddWithValue("@Debit_Description", debitModel.Description);
                cmd1.Parameters.AddWithValue("@CreatedBy", debitModel.CreatedBy);

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
        /// customerShareMassanger
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public string customerShareMassanger(int CustomerId, int TenantID)
        {
            MySqlCommand cmd = new MySqlCommand();
            DataSet ds = new DataSet();
            string Message = "";
            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_CustomerShareMassanger", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Tenant_Id", TenantID);
                cmd1.Parameters.AddWithValue("@Customer_Id", CustomerId);

                Message = Convert.ToString(cmd1.ExecuteScalar());
                MySqlDataAdapter da = new MySqlDataAdapter
                {
                    SelectCommand = cmd1
                };
                da.Fill(ds);
                if (ds != null && ds.Tables[0] != null)
                {
                    Message = ds.Tables[0].Rows[0]["Message"] == DBNull.Value ? String.Empty : Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
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
            return Message;
        }
        #endregion
    }
}
