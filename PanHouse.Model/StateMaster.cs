using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Model
{
    /// <summary>
    /// State Master
    /// </summary>
    public class StateMaster
    {
        /// <summary>
        /// StateID
        /// </summary>
        public int StateID { get; set; }
        /// <summary>
        /// CountryID
        /// </summary>
        public int CountryID { get; set; }
        /// <summary>
        /// StateName
        /// </summary>
        public string StateName { get; set; }
        /// <summary>
        /// StateCode
        /// </summary>
        public int StateCode { get; set; }
    }
}
