using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Interface
{
    /// <summary>
    /// Interface for Credi/debit
    /// </summary>
    public interface ICreditDebit
    {
        List<CreditDebitModel> getCreditDebitList(int TenantID);

        int addCreditDetails(CreditModel creditModel, int TenantID);

        int addDebitDetails(DebitModel debitModel, int TenantID);
        string customerShareMassanger(int CustomerId, int TenantID);
    }
}
