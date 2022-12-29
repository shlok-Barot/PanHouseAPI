using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Model
{
    /// <summary>
    /// Debit Model
    /// </summary>
    public class DebitModel
    {
        /// <summary>
        /// DebitID 
        /// </summary>
        public int DebitID { get; set; }

        /// <summary>
        /// CustomerID
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// DebitAmount
        /// </summary>
        public decimal DebitAmount { get; set; }

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
