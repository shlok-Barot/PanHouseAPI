using MySql.Data.MySqlClient;
using PanHouse.Interface;
using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PanHouse.Services
{
    public class UserServices : IUser
    {
        MySqlConnection conn = new MySqlConnection();

        #region Constructor
        public UserServices(string connectionString)
        {
            conn.ConnectionString = connectionString;
        }
        #endregion

        #region Custom method
        /// <summary>
        /// GetUserProfileDetails
        /// </summary>
        /// <param name="UserMasterID"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<UserProfileDetailsModel> GetUserProfileDetails(int UserMasterID, string url)
        {
            List<UserProfileDetailsModel> UserProfileDetailsList = new List<UserProfileDetailsModel>();
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_GetUserProfileDetails", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@User_ID", UserMasterID);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd1;
                da.Fill(ds);
                if (ds != null && ds.Tables[0] != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        UserProfileDetailsModel Userlist = new UserProfileDetailsModel();
                        Userlist.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserID"]);
                        Userlist.FirstName = ds.Tables[0].Rows[i]["FirstName"] == DBNull.Value ? string.Empty : Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]);
                        Userlist.LastName = ds.Tables[0].Rows[i]["LastName"] == DBNull.Value ? string.Empty : Convert.ToString(ds.Tables[0].Rows[i]["LastName"]);
                        Userlist.EmailId = ds.Tables[0].Rows[i]["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(ds.Tables[0].Rows[i]["EmailId"]);
                        Userlist.MobileNo = ds.Tables[0].Rows[i]["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]);
                        Userlist.Address = ds.Tables[0].Rows[i]["Address"] == DBNull.Value ? string.Empty : Convert.ToString(ds.Tables[0].Rows[i]["Address"]);
                        Userlist.CompanyName = ds.Tables[0].Rows[i]["CompanyName"] == DBNull.Value ? string.Empty : Convert.ToString(ds.Tables[0].Rows[i]["CompanyName"]);
                        Userlist.CityName = ds.Tables[0].Rows[i]["CityName"] == DBNull.Value ? string.Empty : Convert.ToString(ds.Tables[0].Rows[i]["CityName"]);
                        Userlist.ProfilePicture = ds.Tables[0].Rows[i]["ProfilePicture"] == DBNull.Value ? string.Empty : url + "/" + Convert.ToString(ds.Tables[0].Rows[i]["ProfilePicture"]);
                        UserProfileDetailsList.Add(Userlist);
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
            return UserProfileDetailsList;
        }
        /// <summary>
        /// UpdateUserProfileDetail
        /// </summary>
        /// <param name="userProfileDetailsModel"></param>
        /// <returns></returns>
        public int UpdateUserProfileDetail(UserProfileDetailsModel userProfileDetailsModel)
        {
            int UserID = 0;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SP_UpdateUserProfileDetails", conn);
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@User_ID", userProfileDetailsModel.UserId);
                cmd.Parameters.AddWithValue("@First_Name", userProfileDetailsModel.FirstName);
                cmd.Parameters.AddWithValue("@Last_Name", userProfileDetailsModel.LastName);
                cmd.Parameters.AddWithValue("@Mobile_No", userProfileDetailsModel.MobileNo);
                cmd.Parameters.AddWithValue("@Email_ID", userProfileDetailsModel.EmailId);
                cmd.Parameters.AddWithValue("@Address", userProfileDetailsModel.Address);
                cmd.Parameters.AddWithValue("@Profile_Picture", userProfileDetailsModel.ProfilePicture);

                cmd.CommandType = CommandType.StoredProcedure;
                UserID = Convert.ToInt32(cmd.ExecuteScalar());

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

            return UserID;
        }
        #endregion
    }
}
