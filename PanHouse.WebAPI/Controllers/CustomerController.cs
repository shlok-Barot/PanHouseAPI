using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CustomerController : ControllerBase
    {

        #region variable
        private IConfiguration configuration;
        public string JsonData;
        private readonly string _connectioSting;
        #endregion

        #region Cunstructor
        MySqlConnection conn = new MySqlConnection();
        public CustomerController(IConfiguration _Iconfig)
        {
            configuration = _Iconfig;
            JsonData = configuration.GetValue<string>("JSONData");
            _connectioSting = configuration.GetValue<string>("ConnectionStrings:DataAccessMySqlProvider");
        }
        #endregion

        #region Custom methods
        /// <summary>
        /// Get Customer list
        /// </summary>
        /// <param name="CustomerName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCustomerList")]
        public ResponseModel GetCustomerList(string CustomerName = "")
        {
            List<CustomerMaster> objCustomerList = new List<CustomerMaster>();
            ResponseModel objResponseModel = new ResponseModel();
            CustomerCaller customerCaller = new CustomerCaller();

            int statusCode = 0;
            string statusMessage = "";
            try
            {
                //string token = Convert.ToString(Request.Headers["X-Authorized-Token"]);
                Authenticate authenticate = new Authenticate();

                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);

                objCustomerList = customerCaller.GetcustomerList(new CustomerService(_connectioSting), CustomerName, authenticate.TenantId);

                statusCode = objCustomerList.Count == 0 ? (int)EnumMaster.StatusCode.RecordNotFound : (int)EnumMaster.StatusCode.Success;
                statusMessage = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)statusCode);

                objResponseModel.Status = true;
                objResponseModel.StatusCode = statusCode;
                objResponseModel.Message = statusMessage;
                objResponseModel.ResponseData = objCustomerList;
            }
            catch (Exception)
            {
                throw;
            }
            return objResponseModel;
        }

        /// <summary>
        /// CreateCustomer
        /// </summary>
        /// <param name="customerMaster"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateCustomer")]
        public ResponseModel CreateCustomer([FromBody]CustomerMaster customerMaster)
        {
            CustomerCaller customerCaller = new CustomerCaller();
            ResponseModel objResponseModel = new ResponseModel();
            int statusCode = 0;
            string statusMessage = "";
            try
            {
                Authenticate authenticate = new Authenticate();

                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);

                customerMaster.CreatedBy = authenticate.UserMasterID;

                int result = customerCaller.addCustomer(new CustomerService(_connectioSting), customerMaster, authenticate.TenantId);

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
        /// UpdateCustomer
        /// </summary>
        /// <param name="customerMaster"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateCustomer")]
        public ResponseModel UpdateCustomer([FromBody]CustomerMaster customerMaster)
        {
            CustomerCaller customerCaller = new CustomerCaller();
            ResponseModel objResponseModel = new ResponseModel();
            int statusCode = 0;
            string statusMessage = "";
            try
            {
                Authenticate authenticate = new Authenticate();

                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);
                customerMaster.ModifyBy = authenticate.UserMasterID;

                int result = customerCaller.updateCustomer(new CustomerService(_connectioSting), customerMaster, authenticate.TenantId);

                statusCode = result == 0 ? (int)EnumMaster.StatusCode.RecordNotFound : (int)EnumMaster.StatusCode.Success;
                statusMessage = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)statusCode);

                objResponseModel.Status = true;
                objResponseModel.StatusCode = statusCode;
                objResponseModel.Message = statusMessage;
            }
            catch (Exception)
            {
                throw;
            }

            return objResponseModel;
        }

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteCustomer")]
        public ResponseModel DeleteCustomer(int CustomerID)
        {
            CustomerCaller customerCaller = new CustomerCaller();
            ResponseModel objResponseModel = new ResponseModel();
            int statusCode = 0;
            string statusMessage = "";

            try
            {
                Authenticate authenticate = new Authenticate();

                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);

                int result = customerCaller.DeleteCustomer(new CustomerService(_connectioSting), CustomerID, authenticate.TenantId);

                statusCode =
               result == 0 ?
                    (int)EnumMaster.StatusCode.RecordInUse : (int)EnumMaster.StatusCode.RecordDeletedSuccess;

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
        /// GetCustomerName
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCustomerName")]
        public ResponseModel GetCustomerName(string searchText)
        {
            List<CustomerMaster> objCustomerList = new List<CustomerMaster>();
            ResponseModel objResponseModel = new ResponseModel();
            CustomerCaller customerCaller = new CustomerCaller();

            int statusCode = 0;
            string statusMessage = "";
            try
            {
                Authenticate authenticate = new Authenticate();

                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);

                objCustomerList = customerCaller.GetCustomerName(new CustomerService(_connectioSting), searchText, authenticate.TenantId);

                statusCode = objCustomerList.Count == 0 ? (int)EnumMaster.StatusCode.RecordNotFound : (int)EnumMaster.StatusCode.Success;
                statusMessage = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)statusCode);

                objResponseModel.Status = true;
                objResponseModel.StatusCode = statusCode;
                objResponseModel.Message = statusMessage;
                objResponseModel.ResponseData = objCustomerList;
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