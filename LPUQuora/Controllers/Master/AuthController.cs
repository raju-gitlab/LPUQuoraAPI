using LPUQa.BUSINESS.IBusiness;
using LPUQa.MODEL.Master;
using LPUQa.UTILITIES.CustomeExceptions;
using LPUQa.UTILITIES.Querries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LPUQa.WEB.Controllers.Master
{
    public class AuthController : ControllerBase
    {
        #region Constructor and parameters
        private readonly IAuthBusiness _authBusiness;
        public AuthController(IAuthBusiness authBusiness)
        {
            this._authBusiness = authBusiness;
        }
        #endregion

        #region Get

        #region Login
        [HttpGet]
        public ActionResult Login([FromQuery] CommonModel credentials)
        {
            try
            {
                int result = this._authBusiness.Login(credentials);
                if (result == 1)
                {
                    return Ok(result);
                }
                else if (result == 0)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch(NullValueException nvEx)
            {
                return BadRequest(nvEx);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Two Factor Verification
        [HttpGet]
        public ActionResult SecondStepVerificatino([FromQuery]CommonModel keys)
        {
            try
            {
                bool result = this._authBusiness.TwoFactorAuth(keys);
                return Ok(result);
            }
            catch (NullValueException nvEx)
            {
                return BadRequest(nvEx);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region CheckEmail
        [HttpGet]
        public ActionResult ValidateUser([FromQuery]string Email)
        {
            bool result = this._authBusiness.CheckUserAcailability(Email);
            if(result)
            {
                return Ok(result);
            }
            else
            {
                return Ok(result);
            }
        }
        #endregion

        #endregion

        #region Post 

        #region AddNewUser
        [HttpPost]
        public IActionResult Registeruser([FromBody]AuthModel auth)
        {
            var result = this._authBusiness.Register(auth);
            if (result)
            {
                return Ok("New User Added Successfully");
            }
            else
            {
                return BadRequest("New User not added");
            }
        }
        #endregion
        
        #endregion

        #region Put

        #endregion
    }
}
