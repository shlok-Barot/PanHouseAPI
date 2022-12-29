using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Interface
{
    public interface IMasterInterface
    {
        /// <summary>
        /// GetSMTPDetails
        /// </summary>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        SMTPDetails GetSMTPDetails(int TenantID);

        /// <summary>
        /// getStateList
        /// </summary>
        /// <returns></returns>
        List<StateMaster> getStateList();

        /// <summary>
        /// getCityList
        /// </summary>
        /// <param name="StateId"></param>
        /// <returns></returns>
        List<CityMaster> getCityList(int StateId);
    }
}
