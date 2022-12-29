using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace PanHouse.Services
{
    public class CommonService
    {
        /// <summary>
        /// SendEmail
        /// </summary>
        /// <param name="sMTPDetails"></param>
        /// <param name="emailToAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool SendEmail(SMTPDetails smtpDetails, string emailToAddress, string subject, string body, string[] cc = null, string[] bcc = null, int tenantId = 0)
        {
            bool isMailSent = false;
            try
            {
                SmtpClient smtpClient = new SmtpClient(smtpDetails.SMTPServer, Convert.ToInt32(smtpDetails.SMTPPort));
                smtpClient.EnableSsl = smtpDetails.EnableSsl;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(smtpDetails.FromEmailId, smtpDetails.Password);
                {
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(smtpDetails.FromEmailId, "PanHouse");
                        if (cc != null)
                        {
                            if (cc.Length > 0)
                            {
                                for (int i = 0; i < cc.Length; i++)
                                {
                                    message.CC.Add(cc[i]);
                                }
                            }
                        }

                        if (bcc != null)
                        {
                            if (bcc.Length > 0)
                            {
                                for (int k = 0; k < bcc.Length; k++)
                                {
                                    message.CC.Add(bcc[k]);
                                }

                            }
                        }
                        message.Subject = subject == null ? "" : subject;
                        message.Body = body == null ? "" : body;
                        message.IsBodyHtml = smtpDetails.IsBodyHtml;
                        message.To.Add(emailToAddress);

                        smtpClient.Send(message);
                    }
                }
                isMailSent = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            return isMailSent;
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="Planetext"></param>
        public string Encrypt(string Planetext)
        {
            try
            {
                var key = "sblw-3hn8-sqoy19";
                byte[] inputArray = UTF8Encoding.UTF8.GetBytes(Planetext);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                var finaltoken = Convert.ToBase64String(resultArray, 0, resultArray.Length);
                return finaltoken;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public string Decrypt(string cipherText)
        {
            try
            {
                var key = "sblw-3hn8-sqoy19";
                byte[] inputArray = Convert.FromBase64String(cipherText.Replace(".", "+"));
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                var finalDecrptToken = UTF8Encoding.UTF8.GetString(resultArray);
                return finalDecrptToken;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
