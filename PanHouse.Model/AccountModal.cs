using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Model
{
    /// <summary>
    /// AccountModal
    /// </summary>
    public class AccountModal
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// FirstName 
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// LastName 
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// UserEmailID 
        /// </summary>
        public string UserEmailID { get; set; }
        /// <summary>
        /// IsActive
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// LoginTime
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// Is Valid
        /// </summary>
        public bool IsValidUser { get; set; }
    }
}
