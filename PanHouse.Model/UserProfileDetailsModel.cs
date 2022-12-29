using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Model
{
    /// <summary>
    /// For User Profile 
    /// </summary>
    public class UserProfileDetailsModel
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// FirstName
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// LastName
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// MobileNo
        /// </summary>
        public string MobileNo { get; set; }
        /// <summary>
        /// EmailId
        /// </summary>
        public string EmailId { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// CityName
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// CompanyName
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// ProfilePicture
        /// </summary>
        public string ProfilePicture { get; set; }
    }
    public class ProfileDetailsmodel
    {
        /// <summary>
        /// Result
        /// </summary>
        public int Result { get; set; }
        /// <summary>
        /// ProfilePath
        /// </summary>
        public string ProfilePath { get; set; }
    }
}
