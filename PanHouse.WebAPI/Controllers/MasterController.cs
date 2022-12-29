using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PanHouse.Model;
using PanHouse.Services;
using PanHouse.WebAPI.Filters;
using PanHouse.WebAPI.Provider;

namespace PanHouse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = SchemesNamesConst.TokenAuthenticationDefaultScheme)]
    public class MasterController : ControllerBase
    {
        #region variable
        private IConfiguration configuration;
        private readonly string _connectioSting;
        public string JsonData;
        #endregion

        #region Cunstructor
        MySqlConnection conn = new MySqlConnection();
        public MasterController(IConfiguration _Iconfig)
        {
            configuration = _Iconfig;
            JsonData = configuration.GetValue<string>("JSONData");
            _connectioSting = configuration.GetValue<string>("ConnectionStrings:DataAccessMySqlProvider");
        }
        #endregion

        #region Custom Method
        /// <summary>
        /// GetStateList
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetStateList")]
        public ResponseModel GetStateList()
        {
            List<StateMaster> objStateList = new List<StateMaster>();
            ResponseModel objResponseModel = new ResponseModel();
            int StatusCode = 0;
            string statusMessage = "";

            try
            {
                Authenticate authenticate = new Authenticate();
                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);

                MasterCaller _masterCaller = new MasterCaller();

                objStateList = _masterCaller.getStateList(new MasterServices(_connectioSting));

                StatusCode = objStateList.Count == 0 ?
                    (int)EnumMaster.StatusCode.RecordNotFound : (int)EnumMaster.StatusCode.Success;

                statusMessage = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)StatusCode);

                objResponseModel.Status = true;
                objResponseModel.StatusCode = StatusCode;
                objResponseModel.Message = statusMessage;
                objResponseModel.ResponseData = objStateList;
            }
            catch (Exception)
            {
                throw;
            }

            return objResponseModel;
        }

        [HttpPost]
        [Route("GetCityList")]
        public ResponseModel GetCityList(int StateId)
        {
            List<CityMaster> objCityList = new List<CityMaster>();
            ResponseModel objResponseModel = new ResponseModel();
            int StatusCode = 0;
            string statusMessage = "";

            try
            {
                Authenticate authenticate = new Authenticate();
                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);

                MasterCaller _masterCaller = new MasterCaller();

                objCityList = _masterCaller.getCityList(new MasterServices(_connectioSting), StateId);

                StatusCode = objCityList.Count == 0 ?
                    (int)EnumMaster.StatusCode.RecordNotFound : (int)EnumMaster.StatusCode.Success;

                statusMessage = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)StatusCode);

                objResponseModel.Status = true;
                objResponseModel.StatusCode = StatusCode;
                objResponseModel.Message = statusMessage;
                objResponseModel.ResponseData = objCityList;
            }
            catch (Exception)
            {
                throw;
            }

            return objResponseModel;
        }

        #endregion
    }
}