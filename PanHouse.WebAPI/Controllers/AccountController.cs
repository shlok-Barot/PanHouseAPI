using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PanHouse.Model;
using PanHouse.Services;
using PanHouse.WebAPI.Provider;

namespace PanHouse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region variable
        private IConfiguration configuration;
        private readonly string _connectioSting;
        public string OriginalUrl;
        #endregion

        public AccountController(IConfiguration _iConfig, IHttpContextAccessor httpcontextaccessor)
        {
            configuration = _iConfig;
            _connectioSting = configuration.GetValue<string>("ConnectionStrings:DataAccessMySqlProvider");
            var request = httpcontextaccessor.HttpContext.Request;

            string origin = request.Headers["Origin"].ToString();
            OriginalUrl = origin;
        }

        /// <summary>
        /// Authenticaton User
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("authenticateUser")]
        [HttpPost]
        public ResponseModel authenticateUser()
        {

            string X_Authorized_userId = Convert.ToString(Request.Headers["X-Authorized-userId"]);
            string X_Authorized_password = Convert.ToString(Request.Headers["X-Authorized-password"]);
            string X_Authorized_Domainname = Convert.ToString(Request.Headers["X-Authorized-Domainname"]);

            ResponseModel resp = new ResponseModel();
            try
            {
                SecurityCaller newSecurityCaller = new SecurityCaller();
                AccountModal account = new AccountModal();

                string userId = X_Authorized_userId.Replace(' ', '+');
                string password = X_Authorized_password.Replace(' ', '+');
                string Domain_Name = X_Authorized_Domainname.Replace(' ', '+');

                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(Domain_Name))
                {
                    account = newSecurityCaller.validateUser(new SecurityService(_connectioSting), Domain_Name, userId, password);

                    if (!string.IsNullOrEmpty(account.Token))
                    {
                        account.IsActive = true;
                        resp.Status = true;
                        resp.StatusCode = (int)EnumMaster.StatusCode.Success;
                        resp.ResponseData = account;
                        resp.Message = "Valid Login";
                    }
                    else
                    {
                        account.IsActive = false;
                        resp.Status = true;
                        resp.StatusCode = (int)EnumMaster.StatusCode.Success;
                        resp.ResponseData = account;
                        resp.Message = "In-Valid Login";
                    }
                }
                else
                {
                    resp.Status = false;
                    resp.ResponseData = account;
                    resp.Message = "Invalid Login";
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return resp;
        }

        /// <summary>
        /// ForgotPassword
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("ForgetPassword")]
        public ResponseModel ForgetPassword(string EmailId)
        {
            ResponseModel objResponseModel = new ResponseModel();

            try
            {
                /// validate User
                SecurityCaller securityCaller = new SecurityCaller();
                Authenticate authenticate = securityCaller.validateUserEmailId(new SecurityService(_connectioSting), EmailId);
                if (authenticate.UserMasterID > 0)
                {
                    MasterCaller masterCaller = new MasterCaller();
                    SMTPDetails sMTPDetails = masterCaller.GetSMTPDetails(new MasterServices(_connectioSting), authenticate.TenantId);

                    CommonService commonService = new CommonService();
                    string encryptedEmailId = commonService.Encrypt(EmailId);
                    //Request.Host.Value
                    //string url = configuration.GetValue<string>("websiteURL") + "/auth/forgotpassword?Id:" + encryptedEmailId;
                    string url = OriginalUrl + "/auth/changepassword?Id:" + encryptedEmailId;

                    string content = "";
                    string subject = "";

                    securityCaller.GetForgetPassowrdMailContent(new SecurityService(_connectioSting), authenticate.TenantId, url, EmailId, out content, out subject);

                    bool isUpdate = securityCaller.sendMail(new SecurityService(_connectioSting), sMTPDetails, EmailId, subject, content, authenticate.TenantId);
                    if (isUpdate)
                    {
                        objResponseModel.Status = true;
                        objResponseModel.StatusCode = (int)EnumMaster.StatusCode.Success;
                        objResponseModel.Message = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)(int)EnumMaster.StatusCode.Success);
                        objResponseModel.ResponseData = "Mail Sent Successfully";
                    }
                    else
                    {
                        objResponseModel.Status = false;
                        objResponseModel.StatusCode = (int)EnumMaster.StatusCode.InternalServerError;
                        objResponseModel.Message = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)(int)EnumMaster.StatusCode.InternalServerError);
                        objResponseModel.ResponseData = "Mail Sent Failure";
                    }
                }
                else
                {
                    objResponseModel.Status = false;
                    objResponseModel.StatusCode = (int)EnumMaster.StatusCode.RecordNotFound;
                    objResponseModel.Message = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)(int)EnumMaster.StatusCode.RecordNotFound);
                    objResponseModel.ResponseData = "Sorry User does not exist or active";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return objResponseModel;
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [Route("Logout")]
        [HttpPost]
        public ResponseModel Logout()
        {
            ResponseModel resp = new ResponseModel();

            string token = Convert.ToString(Request.Headers["X-Authorized-Header"]);
            token = SecurityService.DecryptStringAESJSON(token);

            SecurityCaller NewSecuritycaller = new SecurityCaller();

            NewSecuritycaller.LogOut(new SecurityService(_connectioSting), token);

            resp.Status = true;
            resp.StatusCode = (int)EnumMaster.StatusCode.Success;
            resp.ResponseData = null;
            resp.Message = "Logout Successfully!";

            return resp;
        }
    }
}