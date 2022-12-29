using PanHouse.Interface;
using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanHouse.WebAPI.Provider
{
    public class CreditDebitCaller
    {
        #region variable
        public ICreditDebit _CreditDebitRepository;
        #endregion

        #region custom method
        /// <summary>
        /// getCreditDebitList
        /// </summary>
        /// <param name="creditDebit"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public List<CreditDebitModel> getCreditDebitList(ICreditDebit creditDebit, int TenantID)
        {
            _CreditDebitRepository = creditDebit;
            return _CreditDebitRepository.getCreditDebitList(TenantID);
        }

        /// <summary>
        /// AddCreditDetails
        /// </summary>
        /// <param name="creditDebit"></param>
        /// <param name="creditModel"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public int AddCreditDetails(ICreditDebit creditDebit, CreditModel creditModel, int TenantID)
        {
            _CreditDebitRepository = creditDebit;
            return _CreditDebitRepository.addCreditDetails(creditModel, TenantID);
        }
        /// <summary>
        /// AddDebitDetails
        /// </summary>
        /// <param name="creditDebit"></param>
        /// <param name="debitModel"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public int AddDebitDetails(ICreditDebit creditDebit, DebitModel debitModel, int TenantID)
        {
            _CreditDebitRepository = creditDebit;
            return _CreditDebitRepository.addDebitDetails(debitModel, TenantID);
        }

        /// <summary>
        /// customerShareMassanger
        /// </summary>
        /// <param name="creditDebit"></param>
        /// <param name="CustomerId"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public string customerShareMassanger(ICreditDebit creditDebit, int CustomerId, int TenantID)
        {
            _CreditDebitRepository = creditDebit;
            return _CreditDebitRepository.customerShareMassanger(CustomerId, TenantID);
        }
        #endregion
    }
}
