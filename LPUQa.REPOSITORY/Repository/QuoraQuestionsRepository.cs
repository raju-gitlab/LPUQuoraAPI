using LPUQa.MODEL.QuestionModel;
using LPUQa.REPOSITORY.IRepository;
using LPUQa.UTILITIES.CustomeExceptions;
using LPUQa.UTILITIES.LogManager;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.REPOSITORY.Repository
{
    public class QuoraQuestionsRepository : IQuoraQuestionsRepository
    {
        #region Parameters and Constructor
        public dynamic globalParam { get; set; }
        private readonly IConfiguration _configuration;
        public QuoraQuestionsRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        #endregion

        #region Get
        #region GetShareTypes
        public List<ShareTypeModel> ShareTypes()
        {
            string CS = this._configuration.GetConnectionString("Dev");
            string query = "SELECT ShareType,ShareUID FROM PostShareType";
            List<ShareTypeModel> types = new List<ShareTypeModel>();
            try
            {
                using(SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using(SqlCommand cmd = new SqlCommand(query,con))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while(rdr.Read())
                        {
                            types.Add(new ShareTypeModel
                            {
                                ShareType = rdr["ShareType"].ToString(),
                                ShareUID = rdr["ShareUID"].ToString()
                            });
                        }
                        if(types.Count != 0)
                        {
                            return types;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region CanUserPost
        public bool CanUserPost(string userId)
        {
            string CS = this._configuration.GetConnectionString("Dev");
            string query = "select CanPost FROM UserTbl WHERE Userguid = @UUID";
            try
            {
                if (userId != null || userId != string.Empty)
                {
                    using (SqlConnection con = new SqlConnection(CS))
                    {
                        con.Open();
                        using(SqlCommand cmd = new SqlCommand(query,con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(new SqlParameter("@UUID", userId));
                            SqlDataReader rdr = cmd.ExecuteReader();
                            if(rdr.Read())
                            {
                                globalParam = Convert.ToInt32(rdr["CanPost"]);
                            }
                        }
                        if(globalParam == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (UserExceptions exception)
            {

                throw new UserExceptions("", "", "", "");
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
        public string AddNewQuoraGlobalpost(QuestionModel globalQuestion)
        {
            string CS = this._configuration.GetConnectionString("Dev");
            string Query = Convert.ToString("INSERT INTO QuoraGlobalpostTbl(PostName,PostDescription,PostImagePath,AnswerPath,PostUserId,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,LikeCount,DislikeCout,ShareCount,ShareType,Ishidden,IsDeleted,ReasonForRemove,PostId)" +
            " VALUES" +
            " (@PostName, @PostDescription, @PostImagePath, @AnswerPath, (SELECT Id FROM UserTbl WHERE Userguid = @userId), @CreatedBy, @CreatedOn, @ModifiedBy, @ModifiedOn, @LikeCount, @DislikeCout, @ShareCount, (SELECT Id FROM PostShareType WHERE ShareUID = @shareId), @Ishidden, @IsDeleted, @ReasonForRemove, @PostId)");
            SqlTransaction transaction = null;
            using(SqlConnection con = new SqlConnection(CS))
            {
                try
                {
                    con.Open();
                    transaction = con.BeginTransaction("First");
                    using (SqlCommand cmd = new SqlCommand(Query,con,transaction))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@shareId", globalQuestion.ShareType));
                        cmd.Parameters.Add(new SqlParameter("@userId", globalQuestion.Value));
                        cmd.Parameters.AddWithValue("@PostName", globalQuestion.PostName);
                        cmd.Parameters.AddWithValue("@PostDescription", globalQuestion.PostDescription);
                        cmd.Parameters.AddWithValue("@PostImagePath", string.Empty);
                        cmd.Parameters.AddWithValue("@AnswerPath", string.Empty);
                        cmd.Parameters.AddWithValue("@CreatedBy", globalQuestion.Value); 
                        cmd.Parameters.AddWithValue("@CreatedOn", DateTimeOffset.UtcNow);
                        cmd.Parameters.AddWithValue("@ModifiedBy", globalQuestion.Value);
                        cmd.Parameters.AddWithValue("@ModifiedOn", DateTimeOffset.UtcNow);
                        cmd.Parameters.AddWithValue("@LikeCount", 0);
                        cmd.Parameters.AddWithValue("@DislikeCount", 0);
                        cmd.Parameters.AddWithValue("@ShareCount", 0);
                        cmd.Parameters.AddWithValue("@Ishidden", false);
                        cmd.Parameters.AddWithValue("@IsDeleted", false);
                        cmd.Parameters.AddWithValue("@ReasonForRemove", string.Empty);
                        cmd.Parameters.AddWithValue("@PostId", Guid.NewGuid().ToString());
                        if(cmd.ExecuteNonQuery() > 0)
                        {
                            return globalQuestion.PostName;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
        #endregion

        #region Add New Quora Global Question
        public string AddNewQuoraGlobalQuestion(QuestionModel globalQuestion)
        {
            string query = Convert.ToString("DECLARE @UserId int = (SELECT Id from UserTbl WHERE Userguid = @UUID)"+
            " DECLARE @ShareId int = (SELECT Id from PostShareType WHERE ShareUID = @SUID)"+
            " INSERT INTO QuoraGlobalQuestionTbl(QuestionTitle, Answerpath, PostUserId, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, ShareType, Ishidden, IsDeleted, ReasonForRemove, QuestionUID)"+
            " VALUES"+
            " (@QuestionTitle, @Answerpath, @UserId, @CreatedBy, @CreatedOn, @ModifiedBy, @ModifiedOn, @ShareId, @Ishidden, @IsDeleted, @ReasonForRemove, @QuestionUID)");
            
            string query2 = Convert.ToString("DECLARE @UserId int = (SELECT Id from UserTbl WHERE Userguid = @UUID)"+
            " DECLARE @QuesId int = (SELECT Id from QuoraGlobalQuestionTbl WHERE QuestionUID = @QUID)"+
            " INSERT INTO UserActivityTbl(UserId, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, TotalAnswered, TotalQuestioned, TotalComments, TotalLikes, TotalDislikes, TotalReputationGained, TotalReportGained, QuestionId,TotalShareCount)"+
            " VALUES" +
            " (@UserId, @CreatedBy, @CreatedOn, @ModifiedBy, @ModifiedOn, @TotalAnswered, @TotalQuestioned, @TotalComments, @TotalLikes, @TotalDislikes, @TotalReputationGained, @TotalReportGained, @QuesId, @TotalShareCount)");
            
            string CS = this._configuration.GetConnectionString("Dev");
            SqlConnection con = null;
            SqlCommand cmd = null;
            SqlTransaction transaction = null;
            string QuestinoId = Guid.NewGuid().ToString();
            using (con = new SqlConnection(CS))
            {
                try
                {
                    con.Open();
                    transaction = con.BeginTransaction("First");
                    using (cmd = new SqlCommand(query,con,transaction))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@UUID",globalQuestion.Key));
                        cmd.Parameters.Add(new SqlParameter("@SUID", globalQuestion.Value));
                        cmd.Parameters.AddWithValue("@QuestionTitle", globalQuestion.PostName);
                        cmd.Parameters.AddWithValue("@Answerpath", globalQuestion.PostDescription);
                        cmd.Parameters.AddWithValue("@PostUserId", 2);
                        cmd.Parameters.AddWithValue("@CreatedBy", globalQuestion.Key);
                        cmd.Parameters.AddWithValue("CreatedOn", DateTimeOffset.UtcNow);
                        cmd.Parameters.AddWithValue("@ModifiedBy", " ");
                        cmd.Parameters.AddWithValue("@ModifiedOn", " ");
                        cmd.Parameters.AddWithValue("@ShareType", 1);
                        cmd.Parameters.AddWithValue("@Ishidden", false);
                        cmd.Parameters.AddWithValue("@IsDeleted", true);
                        cmd.Parameters.AddWithValue("@ReasonForRemove", string.Empty);
                        cmd.Parameters.AddWithValue("@QuestionUID", QuestinoId);
                    }
                    if(cmd.ExecuteNonQuery() > 0)
                    {
                        using(cmd = new SqlCommand(query2,con,transaction))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(new SqlParameter("@QUID", QuestinoId));
                            cmd.Parameters.Add(new SqlParameter("@UUID", globalQuestion.Key));
                            cmd.Parameters.AddWithValue("@CreatedBy", globalQuestion.Key);
                            cmd.Parameters.AddWithValue("@CreatedOn", DateTimeOffset.UtcNow);
                            cmd.Parameters.AddWithValue("@ModifiedBy", globalQuestion.Key);
                            cmd.Parameters.AddWithValue("@ModifiedOn", DateTimeOffset.UtcNow);
                            cmd.Parameters.AddWithValue("@TotalAnswered", 0);
                            cmd.Parameters.AddWithValue("@TotalQuestioned", 0);
                            cmd.Parameters.AddWithValue("@TotalComments", 0);
                            cmd.Parameters.AddWithValue("@TotalLikes", 0);
                            cmd.Parameters.AddWithValue("@TotalDislikes", 0);
                            cmd.Parameters.AddWithValue("@TotalReputationGained", 0);
                            cmd.Parameters.AddWithValue("@TotalReportGained", 0);
                            cmd.Parameters.AddWithValue("@TotalShareCount", 0);
                        }
                    }
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        transaction.Commit();
                        return QuestinoId;
                    }
                    else
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();   
                    throw;
                }
            }
        }
        #endregion
        #endregion

        #region Put
        #region Update Like in post
        public string UpdateLike(string postId)
        {
            string CS = this._configuration.GetConnectionString("Dev");
            string Query1 = ""; 
            string Query2 = "";
            SqlConnection con = null;
            SqlCommand cmd = null;
            SqlTransaction transaction = null;
            try
            {
                using(con = new SqlConnection(CS))
                {
                    con.Open();
                    transaction = con.BeginTransaction("First");
                    using(cmd = new SqlCommand(Query1,con,transaction))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@UserId", ""));
                        cmd.Parameters.AddWithValue("", "");
                    }
                    if(cmd.ExecuteNonQuery() > 0)
                    {
                        con.Close();
                        using(cmd = new SqlCommand(Query2,con,transaction))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(new SqlParameter("@PostId", postId));
                            cmd.Parameters.Add(new SqlParameter("@UserId",""));
                            cmd.Parameters.AddWithValue("IsLiked", "");
                            cmd.Parameters.AddWithValue("IsDisLiked", "");
                            cmd.Parameters.AddWithValue("CreatedOn", DateTimeOffset.UtcNow);
                            cmd.Parameters.AddWithValue("ModifiedOn", null);
                        }
                        if(cmd.ExecuteNonQuery() > 0)
                        {
                            transaction.Commit();
                            return postId;
                        }
                        else
                        {
                            transaction.Rollback();
                            return null;
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new Exception();
                    }
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion
        #endregion
    }
}