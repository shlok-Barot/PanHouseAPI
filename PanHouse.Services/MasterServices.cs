using MySql.Data.MySqlClient;
using PanHouse.Interface;
using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PanHouse.Services
{
    public class MasterServices : IMasterInterface
    {
        #region connectionString
        MySqlConnection conn = new MySqlConnection();
        public MasterServices(string _connectionString)
        {
            conn.ConnectionString = _connectionString;
        }
        #endregion

        #region SMTP Information 
        /// <summary>
        /// GetSMTPDetails
        /// </summary>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public SMTPDetails GetSMTPDetails(int TenantID)
        {
            DataSet ds = new DataSet();
            SMTPDetails sMTPDetails = new SMTPDetails();

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_getSMTPDetails", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Tenant_ID", TenantID);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd1;
                da.Fill(ds);
                if (ds != null && ds.Tables[0] != null)
                {
                    sMTPDetails.EnableSsl = Convert.ToBoolean(ds.Tables[0].Rows[0]["EnabledSSL"]);
                    sMTPDetails.SMTPPort = Convert.ToString(ds.Tables[0].Rows[0]["SMTPPort"]);
                    sMTPDetails.FromEmailId = Convert.ToString(ds.Tables[0].Rows[0]["EmailUserID"]);
                    sMTPDetails.IsBodyHtml = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBodyHtml"]);
                    sMTPDetails.Password = Convert.ToString(ds.Tables[0].Rows[0]["EmailPassword"]);
                    sMTPDetails.SMTPHost = Convert.ToString(ds.Tables[0].Rows[0]["SMTPHost"]);
                    sMTPDetails.SMTPServer = Convert.ToString(ds.Tables[0].Rows[0]["SMTPHost"]);
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

            return sMTPDetails;
        }
        #endregion

        #region State
        /// <summary>
        /// GetStateList
        /// </summary>
        /// <returns></returns>
        public List<StateMaster> getStateList()
        {
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            List<StateMaster> stateMaster = new List<StateMaster>();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_GetStateList", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd1;
                da.Fill(ds);
                if (ds != null && ds.Tables[0] != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        StateMaster state = new StateMaster();
                        state.StateID = Convert.ToInt32(ds.Tables[0].Rows[i]["StateId"]);
                        state.CountryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CountryId"]);
                        state.StateName = Convert.ToString(ds.Tables[0].Rows[i]["StateName"]);
                        stateMaster.Add(state);
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
            return stateMaster;
        }

        public List<CityMaster> getCityList(int StateId)
        {
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            List<CityMaster> cityMaster = new List<CityMaster>();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_GetCityList", conn);
                cmd1.Parameters.AddWithValue("@State_Id", StateId);
                cmd1.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd1;
                da.Fill(ds);
                if (ds != null && ds.Tables[0] != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CityMaster city = new CityMaster();
                        city.StateID = Convert.ToInt32(ds.Tables[0].Rows[i]["StateId"]);
                        city.CityID = Convert.ToInt32(ds.Tables[0].Rows[i]["CityId"]);
                        city.CityName = Convert.ToString(ds.Tables[0].Rows[i]["CityName"]);
                        cityMaster.Add(city);
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
            return cityMaster;
        }
        #endregion
    }
}
