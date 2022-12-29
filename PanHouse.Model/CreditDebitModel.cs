using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Model
{

    /// <summary>
    /// CreditDebit Model
    /// </summary>
    public class CreditDebitModel
    {

        /// <summary>
        /// CustomerID
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// CustomerName
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// DebitAmount
        /// </summary>
        public decimal DebitAmount { get; set; }

        /// <summary>
        /// CreditAmount
        /// </summary>
        public decimal CreditAmount { get; set; }

        /// <summary>
        /// NeedToPay
        /// </summary>
        public decimal NeedToPay { get; set; }

        /// <summary>
        /// ExtraCredit
        /// </summary>
        public decimal ExtraCredit { get; set; }
    }
}
