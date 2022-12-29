using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Model
{
    /// <summary>
    /// Customer Master
    /// </summary>
    public class CustomerMaster
    {
        /// <summary>
        /// CustomerID
        /// </summary>
        public int CustomerID { get; set; }
        /// <summary>
        /// TenantID
        /// </summary>
        public int TenantID { get; set; }
        /// <summary>
        /// CustomerName
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// CustomerPhoneNumber
        /// </summary>
        public string CustomerPhoneNumber { get; set; }
        /// <summary>
        /// CustomerEmailId
        /// </summary>
        public string CustomerEmailId { get; set; }
        /// <summary>
        /// Gender
        /// </summary>
        public string Gender { get; set; }
       
        /// <summary>
        /// CustomerAddress
        /// </summary>
        public string CustomerAddress { get; set; }
        /// <summary>
        /// Active / In active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Created By
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Modified By
        /// </summary>
        public int ModifyBy { get; set; }

        /// <summary>
        /// Modfied Date
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
    }
}
