using PanHouse.Interface;
using PanHouse.Model;
using PanHouse.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanHouse.WebAPI.Provider
{
    /// <summary>
    /// Security
    /// </summary>
    public class SecurityCaller
    {
        #region Variable Declaration
        private ISecurity _SecurityRepository;
        #endregion

        #region Custom Methods
        /// <summary>
        /// validateUser
        /// </summary>
        /// <param name="security"></param>
        /// <param name="Domain_Name"></param>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AccountModal validateUser(ISecurity security, string Domain_Name, string userId, string password)
        {
            _SecurityRepository = security;
            return _SecurityRepository.AuthenticateUser(Domain_Name, userId, password);
        }
        /// <summary>
        /// ValidateUserEmailId
        /// </summary>
        /// <param name="security"></param>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        public Authenticate validateUserEmailId(ISecurity security, string EmailId)
        {
            _SecurityRepository = security;
            return _SecurityRepository.validateUserEmailId(EmailId);
        }
        /// <summary>
        /// LogOut
        /// </summary>
        /// <param name="security"></param>
        /// <param name="token"></param>
        public void LogOut(ISecurity security, string token)
        {
            _SecurityRepository = security;
            _SecurityRepository.LogOut(token);
        }
        /// <summary>
        /// SendMail
        /// </summary>
        /// <param name="security"></param>
        /// <param name="sMTPDetails"></param>
        /// <param name="EmailId"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        public bool sendMail(ISecurity security, SMTPDetails sMTPDetails, string EmailId, string subject, string content, int TenantId)
        {
            _SecurityRepository = security;
            CommonService commonService = new CommonService();

            return _SecurityRepository.sendMailForForgotPassword(sMTPDetails, EmailId, subject, content, TenantId);
        }
        /// <summary>
        /// GetForgetPassowrdMailContent
        /// </summary>
        /// <param name="security"></param>
        /// <param name="TenantId"></param>
        /// <param name="url"></param>
        /// <param name="emailid"></param>
        /// <param name="content"></param>
        /// <param name="subject"></param>
        public void GetForgetPassowrdMailContent(ISecurity security, int TenantId, string url, string emailid, out string content, out string subject)
        {
            _SecurityRepository = security;
            _SecurityRepository.GetForgetPassowrdMailContent(TenantId, url, emailid, out content, out subject);
        }
        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="security"></param>
        /// <param name="customChangePassword"></param>
        /// <param name="TenantID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool ChangePassword(ISecurity security, CustomChangePassword customChangePassword,int TenantID, int UserID)
        {
            _SecurityRepository = security;
            return _SecurityRepository.ChangePassword(customChangePassword, TenantID, UserID);
        }
        #endregion

    }
}
