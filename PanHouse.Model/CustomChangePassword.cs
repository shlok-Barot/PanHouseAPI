using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Model
{
    public class CustomChangePassword
    {
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// Password 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// NewPassword
        /// </summary>
        public string NewPassword { get; set; }
    }
}
