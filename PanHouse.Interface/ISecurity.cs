using PanHouse.Model;
using System;

namespace PanHouse.Interface
{
    /// <summary>
    /// Interface for the Security
    /// </summary>
    public interface ISecurity
    {
        /// <summary>
        /// AuthenticateUser
        /// </summary>
        /// <param name="Domain_name"></param>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        AccountModal AuthenticateUser(string Domain_Name, string userId, string password);

        /// <summary>
        /// ValidateUserEmailId
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        Authenticate validateUserEmailId(string EmailId);

        /// <summary>
        /// LogOut
        /// </summary>
        /// <param name="token"></param>
        void LogOut(string token);

        /// <summary>
        /// Send mail for the forgot password
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        bool sendMailForForgotPassword(SMTPDetails sMTPDetails, string emailId, string subject, string content, int TenantId);

        /// <summary>
        /// Ge tForgetPassowrd MailContent
        /// </summary>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        void GetForgetPassowrdMailContent(int TenantId, string url, string emailid, out string content, out string subject);
        
        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="customChangePassword"></param>
        /// <param name="TenantId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        bool ChangePassword(CustomChangePassword customChangePassword, int TenantId, int UserId);
    }
}
