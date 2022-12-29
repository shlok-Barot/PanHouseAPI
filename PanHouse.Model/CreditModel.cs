using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Model
{
    /// <summary>
    /// Credit Model
    /// </summary>
    public class CreditModel
    {
        /// <summary>
        /// CreditID
        /// </summary>
        public int CreditID { get; set; }

        /// <summary>
        /// CustomerID
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// CreditAmount
        /// </summary>
        public decimal CreditAmount { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime? CreatedDate { get; set; }
    }
}
