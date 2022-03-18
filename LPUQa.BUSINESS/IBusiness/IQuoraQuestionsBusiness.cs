using LPUQa.MODEL.QuestionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.BUSINESS.IBusiness
{
    public interface IQuoraQuestionsBusiness
    {
        #region Get
        #region GetShareTypes
        List<ShareTypeModel> ShareTypes();
        #endregion
        bool CanUserPost(string userId);
        #endregion

        #region Post
        #region AddNewQuoraGlobalpost
        string AddNewQuoraGlobalpost(QuestionModel globalPost);
        #endregion

        #region Add New Quora Global Question
        string AddNewQuoraGlobalQuestion(QuestionModel globalQuestion);
        #endregion
        #endregion

        #region Put
        #region Update Like in post
        string UpdateLike(string postId);
        #endregion
        #endregion
    }
}
