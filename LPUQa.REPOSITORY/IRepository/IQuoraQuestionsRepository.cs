using LPUQa.MODEL.QuestionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.REPOSITORY.IRepository
{
    public interface IQuoraQuestionsRepository
    {
        #region Get

        #region GetShareTypes
        List<ShareTypeModel> ShareTypes();
        #endregion

        #region CanUserPost
        bool CanUserPost(string userId); 
        #endregion

        #endregion

        #region Post

        #region AddNewQuoraGlobalpost
        string AddNewQuoraGlobalpost(QuestionModel globalQuestion);
        #endregion

        #region Add New Quora Global Question
        string AddNewQuoraGlobalQuestion(QuestionModel globalQuestion);
        #endregion

        #endregion

        #region Put
        
        #region Update Like in post
        string UpdateLike(string postId);
        #endregion

        #region MyRegion

        #endregion
        #endregion
    }
}
