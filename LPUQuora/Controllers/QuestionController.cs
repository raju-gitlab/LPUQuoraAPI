using LPUQa.BUSINESS.IBusiness;
using LPUQa.MODEL.QuestionModel;
using LPUQa.UTILITIES.CustomeExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPUQa.WEB.Controllers
{
    public class QuestionController : ControllerBase
    {
        #region Constructor and Parameters
        private readonly IQuoraQuestionsBusiness _quoraQuestionsBusiness;
        public QuestionController(IQuoraQuestionsBusiness quoraQuestionsBusiness)
        {
            this._quoraQuestionsBusiness = quoraQuestionsBusiness;
        }
        #region Get

        #region GetShareTypes
        [HttpGet]
        public ActionResult GetShareTypes()
        {
            try
            {
                var result = this._quoraQuestionsBusiness.ShareTypes();
                if(result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #endregion

        #region Post
        #region AddNewQuoraGlobalpost
        [HttpPost]
        public ActionResult CreateNewPost([FromBody]QuestionModel questionModel)
        {
            try
            {
                    var result = this._quoraQuestionsBusiness.AddNewQuoraGlobalpost(questionModel);
                    if(result != string.Empty)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }

            }
            catch (UserExceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region UpdateQuoraGlobalQuestions
        [HttpPost]
        public ActionResult CreateNewQuestion([FromBody] QuestionModel questionModel)
        {
            var result = this._quoraQuestionsBusiness.AddNewQuoraGlobalQuestion(questionModel);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region CountLikeShareandSubscribe

        #endregion

        #endregion
        #endregion
    }
}
