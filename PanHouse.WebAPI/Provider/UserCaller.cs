using PanHouse.Interface;
using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanHouse.WebAPI.Provider
{
    public class UserCaller
    {
        #region Variable
        public IUser _UserRepository;
        #endregion

        public List<UserProfileDetailsModel> GetUserProfileDetails(IUser User, int UserMasterID, string url)
        {
            _UserRepository = User;
            return _UserRepository.GetUserProfileDetails(UserMasterID, url);
        }

        public int UpdateUserProfileDetail(IUser user, UserProfileDetailsModel userProfileDetailsModel)
        {
            _UserRepository = user;
            return _UserRepository.UpdateUserProfileDetail(userProfileDetailsModel);
        }
    }
}
