using LPUQa.BUSINESS.IBusiness;
using LPUQa.MODEL.QuestionModel;
using LPUQa.REPOSITORY.IRepository;
using LPUQa.UTILITIES.CustomeExceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.BUSINESS.Business
{
    public class QuoraQuestionsBusiness : IQuoraQuestionsBusiness
    {
        #region Constructors and Parameters
        private readonly IQuoraQuestionsRepository _quoraQuestionsRepository;
        public QuoraQuestionsBusiness(IQuoraQuestionsRepository quoraQuestionsRepository)
        {
            this._quoraQuestionsRepository = quoraQuestionsRepository;
        }
        #endregion

        #region Get
        #region GetShareTypes
        public List<ShareTypeModel> ShareTypes()
        {
            return this._quoraQuestionsRepository.ShareTypes();
        }
        #endregion

        #region Check User Can Create New post or not
        public bool CanUserPost(string userId)
        {
            try
            {
                if (userId != null || userId != string.Empty)
                {
                    return this._quoraQuestionsRepository.CanUserPost(userId);
                }
                else
                {
                    return false;
                }
            }
            catch (UserExceptions exception)
            {

                throw;
            }
            catch (Exception ex) { throw; }
        }
        #endregion

        #endregion

        #region Post

        #region AddNewQuoraGlobalpost
        public string AddNewQuoraGlobalpost(QuestionModel globalQuestion)
        {
            try
            {
                if (this._quoraQuestionsRepository.CanUserPost(globalQuestion.Value))
                {
                    return this._quoraQuestionsRepository.AddNewQuoraGlobalpost(globalQuestion);
                }
                else
                {
                    return "";
                }
            }
            catch (UserExceptions ex)
            {
                throw new UserExceptions("", "", "", "");
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion

        #region Add New Quora Global Question
        public string AddNewQuoraGlobalQuestion(QuestionModel globalQuestion)
        {
            try
            {
                if (this._quoraQuestionsRepository.CanUserPost(globalQuestion.Key))
                {
                    return this._quoraQuestionsRepository.AddNewQuoraGlobalQuestion(globalQuestion);
                }
                else
                {
                    return "";
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

        public string UpdateLike(string postId)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

    }
}
