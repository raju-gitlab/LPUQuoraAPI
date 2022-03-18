using LPUQa.MODEL.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.BUSINESS.IBusiness
{
    public interface IAuthBusiness
    {
        #region Get
        bool CheckUserAcailability(string EmailId);
        int Login(CommonModel common);

        bool TwoFactorAuth(CommonModel common);
        #endregion

        #region Post
        bool Register(AuthModel register);
        #endregion

        #region Put
        #region Update User Credenrials

        #region Update using exsiting password
        bool Updateexsitingpassword(CommonModel credentials);
        #endregion

        #region ForgetPassword
        bool SetVAlidationKey(string EmailId);
        bool ValidateUserKey(CommonModel key);
        bool UpdatePassword(CommonModel key);
        #endregion

        #endregion
        #endregion
    }
}
