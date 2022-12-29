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
    public class CreditDebitController : ControllerBase
    {
        #region variable
        private IConfiguration configuration;
        public string JsonData;
        private readonly string _connectioSting;
        #endregion
        #region Cunstructor
        MySqlConnection conn = new MySqlConnection();
        public CreditDebitController(IConfiguration _Iconfig)
        {
            configuration = _Iconfig;
            JsonData = configuration.GetValue<string>("JSONData");
            _connectioSting = configuration.GetValue<string>("ConnectionStrings:DataAccessMySqlProvider");
        }
        #endregion

        #region Custom method

        /// <summary>
        /// GetCreditDebitList
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCreditDebitList")]
        public ResponseModel GetCreditDebitList()
        {
            List<CreditDebitModel> objCreditDebitList = new List<CreditDebitModel>();
            ResponseModel objResponseModel = new ResponseModel();
            CreditDebitCaller creditDebitCaller = new CreditDebitCaller();

            int statusCode = 0;
            string statusMessage = "";
            try
            {
                //string token = Convert.ToString(Request.Headers["X-Authorized-Token"]);
                Authenticate authenticate = new Authenticate();

                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);

                objCreditDebitList = creditDebitCaller.getCreditDebitList(new CreditDebitService(_connectioSting), authenticate.TenantId);

                statusCode = objCreditDebitList.Count == 0 ? (int)EnumMaster.StatusCode.RecordNotFound : (int)EnumMaster.StatusCode.Success;
                statusMessage = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)statusCode);

                objResponseModel.Status = true;
                objResponseModel.StatusCode = statusCode;
                objResponseModel.Message = statusMessage;
                objResponseModel.ResponseData = objCreditDebitList;
            }
            catch (Exception)
            {
                throw;
            }
            return objResponseModel;
        }

        /// <summary>
        /// AddCreditDetails
        /// </summary>
        /// <param name="creditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddCreditDetails")]
        public ResponseModel AddCreditDetails([FromBody]CreditModel creditModel)
        {
            CreditDebitCaller creditDebitCaller = new CreditDebitCaller();
            ResponseModel objResponseModel = new ResponseModel();
            int statusCode = 0;
            string statusMessage = "";
            try
            {
                Authenticate authenticate = new Authenticate();

                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);

                creditModel.CreatedBy = authenticate.UserMasterID;

                int result = creditDebitCaller.AddCreditDetails(new CreditDebitService(_connectioSting), creditModel, authenticate.TenantId);

                statusCode = result == 0 ? (int)EnumMaster.StatusCode.RecordNotFound : (int)EnumMaster.StatusCode.Success;
                statusMessage = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)statusCode);
                objResponseModel.Status = true;
                objResponseModel.StatusCode = statusCode;
                objResponseModel.Message = statusMessage;
                objResponseModel.ResponseData = result;

            }
            catch (Exception)
            {
                throw;
            }

            return objResponseModel;
        }

        /// <summary>
        /// AddCreditDetails
        /// </summary>
        /// <param name="debitModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddDebitDetails")]
        public ResponseModel AddDebitDetails([FromBody]DebitModel debitModel)
        {
            CreditDebitCaller creditDebitCaller = new CreditDebitCaller();
            ResponseModel objResponseModel = new ResponseModel();
            int statusCode = 0;
            string statusMessage = "";
            try
            {
                Authenticate authenticate = new Authenticate();

                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);

                debitModel.CreatedBy = authenticate.UserMasterID;

                int result = creditDebitCaller.AddDebitDetails(new CreditDebitService(_connectioSting), debitModel, authenticate.TenantId);

                statusCode = result == 0 ? (int)EnumMaster.StatusCode.RecordNotFound : (int)EnumMaster.StatusCode.Success;
                statusMessage = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)statusCode);
                objResponseModel.Status = true;
                objResponseModel.StatusCode = statusCode;
                objResponseModel.Message = statusMessage;
                objResponseModel.ResponseData = result;

            }
            catch (Exception)
            {
                throw;
            }

            return objResponseModel;
        }

        /// <summary>
        /// Customer Share Massanger
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CustomerShareMassanger")]
        public ResponseModel CustomerShareMassanger(int CustomerId)
        {
            CreditDebitCaller creditDebitCaller = new CreditDebitCaller();
            ResponseModel objResponseModel = new ResponseModel();
            int statusCode = 0;
            string statusMessage = "";
            string obj = string.Empty;

            try
            {
                Authenticate authenticate = new Authenticate();

                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);

                obj = creditDebitCaller.customerShareMassanger(new CreditDebitService(_connectioSting), CustomerId, authenticate.TenantId);
                statusCode =
                obj.Length == 0 ? (int)EnumMaster.StatusCode.RecordNotFound : (int)EnumMaster.StatusCode.Success;

                statusMessage = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)statusCode);

                objResponseModel.Status = true;
                objResponseModel.StatusCode = statusCode;
                objResponseModel.Message = statusMessage;
                objResponseModel.ResponseData = obj;
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