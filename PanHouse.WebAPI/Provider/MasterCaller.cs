using PanHouse.Interface;
using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanHouse.WebAPI.Provider
{
    public partial class MasterCaller
    {
        #region variables

        private IMasterInterface _Imaster;
        #endregion

        #region SMTP Details 
        public SMTPDetails GetSMTPDetails(IMasterInterface _ImasterInterface, int TenantId)
        {
            _Imaster = _ImasterInterface;
            return _Imaster.GetSMTPDetails(TenantId);
        }
        #endregion

        #region State
        public List<StateMaster> getStateList(IMasterInterface _ImasterInterface)
        {
            _Imaster = _ImasterInterface;
            return _Imaster.getStateList();
        }

        public List<CityMaster> getCityList(IMasterInterface _ImasterInterface, int StateId)
        {
            _Imaster = _ImasterInterface;
            return _Imaster.getCityList(StateId);
        }
        #endregion
    }
}
