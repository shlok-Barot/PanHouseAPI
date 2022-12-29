using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PanHouse.Interface;
using PanHouse.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PanHouse.Services
{
    public class SecurityService : ISecurity
    {
        #region Constructor
        MySqlConnection conn = new MySqlConnection();
        public SecurityService(string _connectionString)
        {
            conn.ConnectionString = _connectionString;
        }
        #endregion

        #region Custom Methods

        #region Encrypt -Decrypt Methods (AES)
        /// <summary>
        /// Decrypt String from Cipher text
        /// </summary>
        /// <param name="DecryptStringAES"></param>
        /// <returns></returns>
        public static string DecryptStringAES(string cipherText)
        {

            var keybytes = Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");
            var iv = Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = Decrypt(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }
        /// <summary>
        /// DecryptStringAESJSON
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string DecryptStringAESJSON(string cipherText)
        {

            var keybytes = Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");
            var iv = Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = Decrypt(encrypted, keybytes, iv);
            return decriptedFromJavascript;
        }

        /// <summary>
        /// Decrypt string which call from DecryptStringAES
        /// </summary>
        /// <param name="DecryptStringFromBytes"></param>
        /// <returns></returns>
        private static string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        /// <summary>
        /// Encrypt string from plain text
        /// </summary>
        /// <param name="Encrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText)
        {

            var key = Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");
            var iv = Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;


            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            string finaltoken = Convert.ToBase64String(encrypted, 0, encrypted.Length);

            // Return the encrypted bytes from the memory stream.
            return finaltoken;
        }
        #endregion

        #region Login/Authenticate Methods
        /// <summary>
        /// AuthenticateUser
        /// </summary>
        /// <param name="Domain_Name"></param>
        /// <param name="User_EmailID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AccountModal AuthenticateUser(string Domain_Name, string User_EmailID, string password)
        {
            AccountModal accountModal = new AccountModal();

            try
            {
                ////Decrypt Data 

                Domain_Name = DecryptStringAES(Domain_Name);
                User_EmailID = DecryptStringAES(User_EmailID);

                Authenticate authenticate = new Authenticate();
                ////Check whether Login is valid or not

                authenticate = isValidLogin(Domain_Name, User_EmailID, password);

                if (authenticate.UserMasterID > 0)
                {
                    /*Valid User then generate token and save to the database */

                    ////Generate Token 
                    authenticate.Token = generateAuthenticateToken(authenticate.Domain_Name, authenticate.AppID, authenticate.UserEmailID);
                    string _token = JsonConvert.SerializeObject(authenticate);
                    //authenticate.Token = _token;

                    // Save User Token
                    //SaveUserToken(authenticate);

                    accountModal.Message = "Valid user";

                    ////Double encryption: We are doing encryption of encrypted token 
                    accountModal.Token = Encrypt(_token);
                    accountModal.IsValidUser = true;
                    accountModal.FirstName = authenticate.FirstName;
                    accountModal.LastName = authenticate.LastName;
                    accountModal.UserEmailID = User_EmailID;
                }
                else
                {
                    //Wrong Username or password
                    accountModal.Message = "Invalid username or password";
                    accountModal.Token = "";
                    accountModal.IsValidUser = false;
                }

            }
            catch (Exception)
            {
                throw;
            }

            return accountModal;
        }

        /// <summary>
        /// isValidLogin
        /// </summary>
        /// <param name="Domain_Name"></param>
        /// <param name="User_EmailID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private Authenticate isValidLogin(string Domain_Name, string User_EmailID, string password)
        {
            MySqlCommand cmd = new MySqlCommand();
            DataSet ds = new DataSet();
            Authenticate authenticate = new Authenticate();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_ValidateUserLogin", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Domain_Name", Domain_Name);
                cmd1.Parameters.AddWithValue("@User_EmailID", User_EmailID);
                cmd1.Parameters.AddWithValue("@User_Password", password);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd1);
                da.SelectCommand = cmd1;
                da.Fill(ds);
                if (ds != null && ds.Tables[0] != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        bool status = Convert.ToBoolean(ds.Tables[0].Rows[0]["Status"]);

                        if (status)
                        {
                            authenticate.AppID = Convert.ToString(ds.Tables[0].Rows[0]["ApplicationId"]);
                            authenticate.Domain_Name = Domain_Name;
                            authenticate.UserMasterID = Convert.ToInt32(ds.Tables[0].Rows[0]["UserID"]);
                            authenticate.TenantId = Convert.ToInt32(ds.Tables[0].Rows[0]["Tenant_Id"]);
                            authenticate.FirstName = Convert.ToString(ds.Tables[0].Rows[0]["FirstName"]);
                            authenticate.LastName = Convert.ToString(ds.Tables[0].Rows[0]["LastName"]);
                            authenticate.Message = "Valid User";
                            authenticate.UserEmailID = User_EmailID;
                            authenticate.CurrentDatetime = DateTime.Now;
                        }
                        else
                        {
                            authenticate.Domain_Name = "";
                            authenticate.UserMasterID = 0;
                            authenticate.FirstName = "";
                            authenticate.LastName = "";
                            authenticate.Message = "In-valid username or passowrd";
                        }
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
                if (ds != null)
                {
                    ds.Dispose();
                }
            }

            return authenticate;
        }

        /// <summary>
        /// GenerateAuthenticateToken
        /// </summary>
        /// <param name="Domain_Name"></param>
        /// <param name="ApplicationId"></param>
        /// <param name="UserEmailId"></param>
        /// <returns></returns>
        private string generateAuthenticateToken(string Domain_Name, string ApplicationId, string UserEmailId)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(Domain_Name + "_" + ApplicationId + "_" + UserEmailId);
                string token = Convert.ToBase64String(bytes);
                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();
                string SecreateToken = Encrypt(token) + "." + Convert.ToBase64String(time.Concat(key).ToArray());

                return SecreateToken;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Save user token to current session
        /// </summary>
        /// <param name="authenticate"></param>
        /// <returns></returns>
        private Authenticate SaveUserToken(Authenticate authenticate)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_createCurrentSession", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@UserMaster_ID", authenticate.UserMasterID);
                cmd1.Parameters.AddWithValue("@Security_Token", authenticate.Token);
                cmd1.Parameters.AddWithValue("@App_Id", authenticate.AppID);
                cmd1.Parameters.AddWithValue("@Tenant_Id", authenticate.TenantId);
                cmd1.ExecuteNonQuery();
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
            return authenticate;
        }

        /// <summary>
        /// ValidateUserEmailId
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        public Authenticate validateUserEmailId(string EmailId)
        {
            Authenticate authenticate = new Authenticate();
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_validateEmailId", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Email_Id", EmailId);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd1;
                da.Fill(ds);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        authenticate.FirstName = Convert.ToString(ds.Tables[0].Rows[0]["FirstName"]);
                        authenticate.LastName = Convert.ToString(ds.Tables[0].Rows[0]["LastName"]);
                        authenticate.TenantId = Convert.ToInt16(ds.Tables[0].Rows[0]["TenantId"]);
                        authenticate.UserEmailID = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                        authenticate.UserMasterID = Convert.ToInt16(ds.Tables[0].Rows[0]["UserMasterID"]);
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
                if (ds != null)
                {
                    ds.Dispose();
                }
            }

            return authenticate;
        }

        /// <summary>
        /// LogOut
        /// </summary>
        /// <param name="token_data"></param>
        public void LogOut(string token_data)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_LogoutUser", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@token_data", token_data);
                cmd1.ExecuteNonQuery();
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

        }
        /// <summary>
        /// GetForgetPassowrdMailContent
        /// </summary>
        /// <param name="TenantId"></param>
        /// <param name="url"></param>
        /// <param name="emailid"></param>
        /// <param name="content"></param>
        /// <param name="subject"></param>
        public void GetForgetPassowrdMailContent(int TenantId, string url, string emailid, out string content, out string subject)
        {
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            content = "";
            subject = "";
            try
            {
                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_GetForgetPassowrdMailContent", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@_TenantId", TenantId);
                cmd1.Parameters.AddWithValue("@_url", url);
                cmd1.Parameters.AddWithValue("@_emilId", emailid);

                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd1;
                da.Fill(ds);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        content = Convert.ToString(ds.Tables[0].Rows[0]["MailContent"]);
                        subject = Convert.ToString(ds.Tables[0].Rows[0]["MailSubject"]);
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
        }
        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="customChangePassword"></param>
        /// <param name="TenantID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool ChangePassword(CustomChangePassword customChangePassword, int TenantID, int UserID)
        {
            bool result = false;
            int success = 0;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SP_UserChangePassword", conn);
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@_Password", customChangePassword.Password);
                cmd.Parameters.AddWithValue("@_NewPassword", customChangePassword.NewPassword);
                cmd.Parameters.AddWithValue("@User_ID", UserID);
                cmd.Parameters.AddWithValue("@Tenant_Id", TenantID);
                cmd.CommandType = CommandType.StoredProcedure;
                success = Convert.ToInt32(cmd.ExecuteScalar());
                if (success.Equals(1))
                {
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if(conn != null)
                {
                    conn.Close();
                }
            }
            return result;
        }
        #endregion
        #region Send mail for forget password
        /// <summary>
        /// SendMailForForgotPassword
        /// </summary>
        /// <param name="sMTPDetails"></param>
        /// <param name="emailId"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        public bool sendMailForForgotPassword(SMTPDetails sMTPDetails, string emailId, string subject, string content, int TenantId)
        {
            bool isSent = false;
            try
            {
                CommonService commonService = new CommonService();
                isSent = commonService.SendEmail(sMTPDetails, emailId, subject, content, null, null, TenantId);

                return isSent;
            }
            catch (Exception)
            {
                isSent = false;
            }

            return isSent;
        }
        #endregion
        #endregion

    }
}
