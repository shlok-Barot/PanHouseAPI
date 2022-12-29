using PanHouse.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanHouse.Interface
{
   public interface IUser
    {
        List<UserProfileDetailsModel> GetUserProfileDetails(int UserMasterID, string url);

        int UpdateUserProfileDetail(UserProfileDetailsModel userProfileDetailsModel);
    }
}
