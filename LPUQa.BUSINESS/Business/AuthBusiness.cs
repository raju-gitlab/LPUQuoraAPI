using LPUQa.BUSINESS.IBusiness;
using LPUQa.MODEL.Master;
using LPUQa.REPOSITORY.IRepository;
using LPUQa.UTILITIES.CustomeExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.BUSINESS.Business
{
    public class AuthBusiness : IAuthBusiness
    {
        #region Contructor and parameteres
        private readonly IAuthRepository _authRepository;
        public AuthBusiness(IAuthRepository authRepository)
        {
            this._authRepository = authRepository;
        }
        #endregion

        #region Get
        
        #region CheckUserAcailability
        public bool CheckUserAcailability(string EmailId)
        {
            return this._authRepository.CheckUserAcailability(EmailId);
        }
        #endregion

        #region Login
        public int Login(CommonModel common)
        {
            return this._authRepository.Login(common);
        }

        #endregion

        #region TwoFactorAuth
        public bool TwoFactorAuth(CommonModel common)
        {
            try
            {
                return this._authRepository.TwoFactorAuth(common);
            }
            catch(NullValueException nVex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #endregion

        #region Post
        #region New Register
        public bool Register(AuthModel register)
        {
            try
            {
                if (this._authRepository.CheckUserAcailability(register.EmailId))
                {
                    return this._authRepository.Register(register);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #region Put
        #region Update User Credenrials

        #region Update using exsiting password
        public bool Updateexsitingpassword(CommonModel credentials)
        {
            return this._authRepository.Updateexsitingpassword(credentials);
        }
        #endregion

        #endregion

        #region ForgetPassword
        public bool SetVAlidationKey(string EmailId)
        {
            return this._authRepository.SetVAlidationKey(EmailId);
        }
        public bool ValidateUserKey(CommonModel key)
        {
            return this._authRepository.ValidateUserKey(key);
        }
        public bool UpdatePassword(CommonModel key)
        {
            return this._authRepository.UpdatePassword(key);
        }
        #endregion
        #endregion
    }
}
