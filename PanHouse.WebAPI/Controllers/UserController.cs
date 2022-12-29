using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
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
    public class UserController : ControllerBase
    {
        #region variable
        private IConfiguration configuration;
        private readonly string _connectioSting;
        private readonly string ProfileImg_Resources;
        private readonly string ProfileImg_Image;
        public string JsonData;
        #endregion

        #region Cunstructor
        MySqlConnection conn = new MySqlConnection();
        public UserController(IConfiguration _Iconfig)
        {
            configuration = _Iconfig;
            JsonData = configuration.GetValue<string>("JSONData");
            _connectioSting = configuration.GetValue<string>("ConnectionStrings:DataAccessMySqlProvider");
            ProfileImg_Resources = configuration.GetValue<string>("ProfileImg_Resources");
            ProfileImg_Image = configuration.GetValue<string>("ProfileImg_Image");
        }
        #endregion

        #region Custom Method
        /// <summary>
        /// GetUserProfileDetail
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserProfileDetail")]
        public ResponseModel GetUserProfileDetail()
        {
            List<UserProfileDetailsModel> ObjUserProfileDetails = new List<UserProfileDetailsModel>();
            ResponseModel objResponseModel = new ResponseModel();
            int StatusCode = 0;
            string statusMessage = "";

            try
            {
                Authenticate authenticate = new Authenticate();

                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);
                string url = configuration.GetValue<string>("APIURL") + ProfileImg_Resources + "/" + ProfileImg_Image;
                UserCaller userCaller = new UserCaller();
                ObjUserProfileDetails = userCaller.GetUserProfileDetails(new UserServices(_connectioSting), authenticate.UserMasterID, url);
                StatusCode =
                ObjUserProfileDetails.Count == 0 ?
                    (int)EnumMaster.StatusCode.RecordNotFound : (int)EnumMaster.StatusCode.Success;

                statusMessage = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)StatusCode);

                objResponseModel.Status = true;
                objResponseModel.StatusCode = StatusCode;
                objResponseModel.Message = statusMessage;
                objResponseModel.ResponseData = ObjUserProfileDetails;
            }
            catch (Exception)
            {
                throw;
            }

            return objResponseModel;
        }
        /// <summary>
        /// UpdateUserProfileDetails
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateUserProfileDetails")]
        public ResponseModel UpdateUserProfileDetails(IFormFile File)
        {
            UserProfileDetailsModel UserProfileDetailsModel = new UserProfileDetailsModel();
            ProfileDetailsmodel profileDetailsmodel = new ProfileDetailsmodel();
            var Keys = Request.Form;
            UserProfileDetailsModel = JsonConvert.DeserializeObject<UserProfileDetailsModel>(Keys["UserProfileDetailsModel"]);
            var file = Request.Form.Files;
            string timeStamp = DateTime.Now.ToString("ddmmyyyyhhssfff");
            var folderName = Path.Combine(ProfileImg_Resources, ProfileImg_Image);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            try
            {
                if (file.Count > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file[0].ContentDisposition).FileName.Trim('"');
                    var fileName_Id = fileName.Replace(".", UserProfileDetailsModel.UserId + timeStamp + ".") + "";
                    var fullPath = Path.Combine(pathToSave, fileName_Id);
                    var dbPath = Path.Combine(folderName, fileName_Id);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file[0].CopyTo(stream);
                    }
                    UserProfileDetailsModel.ProfilePicture = fileName_Id;
                    string url = configuration.GetValue<string>("APIURL") + ProfileImg_Resources + "/" + ProfileImg_Image + "/" + fileName_Id;
                    profileDetailsmodel.ProfilePath = url;
                }
            }
            catch (Exception) { }
            ResponseModel objResponseModel = new ResponseModel();
            int StatusCode = 0;
            string statusMessage = "";

            try
            {
                Authenticate authenticate = new Authenticate();

                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);
                UserCaller userCaller = new UserCaller();

                int Result = userCaller.UpdateUserProfileDetail(new UserServices(_connectioSting), UserProfileDetailsModel);

                profileDetailsmodel.Result = Result;

                StatusCode = Result == 0 ?
                    (int)EnumMaster.StatusCode.RecordNotFound : (int)EnumMaster.StatusCode.Success;

                statusMessage = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)StatusCode);

                objResponseModel.Status = true;
                objResponseModel.StatusCode = StatusCode;
                objResponseModel.Message = statusMessage;
                objResponseModel.ResponseData = UserProfileDetailsModel;
            }
            catch (Exception)
            {
                throw;
            }

            return objResponseModel;
        }

        [HttpPost]
        [Route("ChangePassword")]
        [AllowAnonymous]
        public ResponseModel ChangePassword([FromBody] CustomChangePassword customChangePassword)
        {
            ResponseModel objResponseModel = new ResponseModel();
            SecurityCaller _securityCaller = new SecurityCaller();
            string _data = "";
            int StatusCode = 0;
            string statusMessage = "";
            try
            {
                Authenticate authenticate = new Authenticate();
                authenticate = JsonConvert.DeserializeObject<Authenticate>(JsonData);

                bool Result = _securityCaller.ChangePassword(new SecurityService(_connectioSting), customChangePassword, authenticate.TenantId, authenticate.UserMasterID);

                StatusCode = Result == false ?
                 (int)EnumMaster.StatusCode.RecordNotFound : (int)EnumMaster.StatusCode.Success;

                statusMessage = CommonFunction.GetEnumDescription((EnumMaster.StatusCode)StatusCode);

                objResponseModel.Status = true;
                objResponseModel.StatusCode = StatusCode;
                objResponseModel.Message = statusMessage;
                objResponseModel.ResponseData = Result;
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