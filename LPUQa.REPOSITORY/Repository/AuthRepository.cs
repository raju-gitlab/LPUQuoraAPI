using LPUQa.MODEL.Master;
using LPUQa.REPOSITORY.IRepository;
using LPUQa.UTILITIES.CustomeExceptions;
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
    public class AuthRepository : IAuthRepository
    {
        #region Contructor and parameters
        public string DataKey { get; set; }
        public string Datavalue { get; set; }
        private readonly IConfiguration _configuration;
        public AuthRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        #endregion

        #region Get

        #region CheckUserAcailability
        public bool CheckUserAcailability(string EmailId)
        {
            string connectionString = this._configuration.GetConnectionString("Dev");
            string query = "IF EXISTS(SELECT Id FROM UserTbl WHERE EmailId = @EmailId)" +
            " select 1;"+
            " else"+
            " select 0; ";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", EmailId));
                        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion

        #region Login
        public int Login(CommonModel common)
        {
            string connectionString = this._configuration.GetConnectionString("Dev");
            string query = "SELECT [Password],Twofactorauthentication from UserTbl WHERE EmailId = @emailId";
            string query2 = "DECLARE @UserId int = (SELECT Id from UserTbl where EmailId = @email)"+
            " INSERT INTO PasswordAuthorizationTbl(UserId, Authorizationkey, IsUsed, RequestTime, ValidateTill)"+
            " VALUES"+
            " (@UserId, @Authorizationkey, @IsUsed, @RequestTime, @ValidateTill)";
            CommonModel authModel = new CommonModel();
            SqlConnection con = null;
            SqlCommand cmd = null;
            SqlTransaction transaction = null;
            using (con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    transaction = con.BeginTransaction("First");
                    using(cmd = new SqlCommand(query,con,transaction))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@emailId", common.Value));
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if(rdr.Read())
                        {
                            authModel.Key = rdr["Password"].ToString();
                            Datavalue = rdr["Twofactorauthentication"].ToString();
                        }
                        if(authModel != null)
                        {
                            con.Close();
                            if (Datavalue == "True" || Datavalue == "1")
                            {
                                con.Open();
                                transaction = con.BeginTransaction("First");
                                using (cmd = new SqlCommand(query2, con, transaction))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.Add(new SqlParameter("@email", common.Key));
                                    cmd.Parameters.AddWithValue("@Authorizationkey", Guid.NewGuid().ToString());
                                    cmd.Parameters.AddWithValue("@IsUsed", false);
                                    cmd.Parameters.AddWithValue("@RequestTime", DateTimeOffset.UtcNow);
                                    cmd.Parameters.AddWithValue("@ValidateTill", DateTimeOffset.UtcNow.AddMinutes(5));
                                }
                                if(cmd.ExecuteNonQuery() > 0)
                                {
                                    transaction.Commit();
                                    return 1;
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return -1;
                                }
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new NullValueException("Data read form database not succeded");
                        }
                    }
                }
                catch (NullValueException nvEx)
                {
                    transaction.Rollback();
                    throw new NullValueException("Data read form database not succeded");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region TwoFactorAuth
        public bool TwoFactorAuth(CommonModel common)
        {
            string CS = this._configuration.GetConnectionString("Dev");
            string query = "";
            string query1 = "";
            SqlConnection con = null;
            SqlCommand cmd = null;
            try
            {
                using(con = new SqlConnection(CS))
                {
                    con.Open();
                    using(cmd = new SqlCommand(query,con))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader rdr = cmd.ExecuteReader();
                        cmd.Parameters.Add(new SqlParameter("@", common.Value));
                        if(rdr.Read())
                        {
                            DataKey = rdr[""].ToString();
                        }
                    }
                    if(DataKey != null && DataKey != string.Empty)
                    {
                        con.Close();
                        if(DataKey == common.Key)
                        {
                            con.Open();
                            using(cmd = new SqlCommand(query1,con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add(new SqlParameter("@", common.Value));
                            }
                            if(cmd.ExecuteNonQuery() > 0)
                            {
                                return true;
                            }
                            else
                            {
                                throw new NullValueException("Data not modified");
                            }
                        }
                        else
                        {
                            throw new NullValueException("Either authentication link is expired or incorrect captcha");
                        }
                    }
                    else
                    {
                        throw new NullValueException("Data read failed or data is not found");
                    }
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

        #region New Register
        public bool Register(AuthModel register)
        {
            string UserImagePath = "";
            string CS = this._configuration.GetConnectionString("Dev");
            string query = Convert.ToString("declare @UserId1 as int = (SELECT Id FROM UserRole WHERE UTypeID = @userId)" +
            " declare @Gender1 as int = (SELECT Id FROM Gender WHERE GenderUID = @gender)" +
            " INSERT into UserTbl(FirstName, MiddleName, LastName, EmailId, gender, Password, PasswordSalt, UserImagePath, UserType, UserOrganizationName, CreatedOn, ModifiedOn, IsVerified, IsActivated, IsDeleted, Userguid, AccountType) VALUES " +
            " (@FirstName, @MiddleName, @LastName, @EmailId, @Gender1, @Password, @PasswordSalt, @UserImagePath, @UserId1, @UserOrganizationName, @CreatedOn, @ModifiedOn, @IsVerified, @IsActivated, @IsDeleted, @Userguid, @AccountType)");
            SqlTransaction transaction = null;
            using (SqlConnection con = new SqlConnection(CS))
            {
                try
                {
                    con.Open();
                    transaction = con.BeginTransaction("FirstTransaction");
                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@FirstName", register.FirstName);
                        cmd.Parameters.AddWithValue("@MiddleName", (register.MiddleName != string.Empty || register.MiddleName != null) ? register.MiddleName : string.Empty);
                        cmd.Parameters.AddWithValue("@LastName", register.LastName);
                        cmd.Parameters.AddWithValue("@EmailId", register.EmailId);
                        cmd.Parameters.Add(new SqlParameter("@gender", register.Gender.GenderUId));
                        cmd.Parameters.Add(new SqlParameter("@userId", register.UserType));
                        cmd.Parameters.AddWithValue("@Password", register.Password);
                        cmd.Parameters.AddWithValue("@PasswordSalt", register.PasswordSalt);
                        cmd.Parameters.AddWithValue("@CreatedOn", DateTimeOffset.UtcNow);
                        cmd.Parameters.AddWithValue("@ModifiedOn", DateTimeOffset.UtcNow);
                        cmd.Parameters.AddWithValue("@IsVerified", false);
                        cmd.Parameters.AddWithValue("@IsActivated", true);
                        cmd.Parameters.AddWithValue("@IsDeleted", false);
                        cmd.Parameters.AddWithValue("@UserOrganizationName", register.UserOrganizationName);
                        cmd.Parameters.AddWithValue("@Userguid", Guid.NewGuid().ToString());
                        cmd.Parameters.AddWithValue("@UserImagePath", "UserImagePath");
                        cmd.Parameters.AddWithValue("@AccountType", 1);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    con.Close();
                    throw;
                }
            }
        }
        #endregion

        #endregion

        #region Put
        
        #region Update User Credenrials

        #region Update using exsiting password
        public bool Updateexsitingpassword(CommonModel credentials)
        {
            string CS = this._configuration.GetConnectionString("Dev");
            string query = "";
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("", credentials.Value));
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
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

        #region ForgetPassword

        #region Add New Validation key for forget password
        public bool SetVAlidationKey(string EmailId)
        {
            try
            {
                string CS = this._configuration.GetConnectionString("Dev");
                string Insertquery = "";
                string Updatequery = Convert.ToString("UPDATE PasswordAuthorizationTbl SET Authorizationkey = @Authorizationkey , RequestTime = @RequestTime, ValidateTill = @ValidateTill" +
                " WHERE UserId = (SELECT Id FROM UserTbl WHERE EmailId = @emailId) AND IsUsed = @IsUsed");
                string CheckDuplicate = Convert.ToString("IF NOT EXISTS(SELECT Id FROM PasswordAuthorizationTbl WHERE UserId = (SELECT Id FROM UserTbl WHERE EmailId = @emailId) AND IsUsed = 0)" +
                " SELECT 1;" +
                " ELSE" +
                " SELECT 0; ");
                int i;
                SqlConnection con;
                SqlCommand cmd;
                using (con = new SqlConnection(CS))
                {
                    //check for Authorixation key is already available or not
                    con.Open();
                    using (cmd = new SqlCommand(CheckDuplicate, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@emailId", EmailId));
                        i = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                    }
                    if (i > 0)
                    {
                        //If Duplicate key not exist in database then Insert a new key
                        con.Open();
                        using (cmd = new SqlCommand(Insertquery, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(new SqlParameter("@emailId", EmailId));
                            cmd.Parameters.AddWithValue("@Authorizationkey", Guid.NewGuid().ToString());
                            cmd.Parameters.AddWithValue("@RequestTime", DateTimeOffset.UtcNow);
                            cmd.Parameters.AddWithValue("@IsUsed", false);
                            cmd.Parameters.AddWithValue("@ValidateTill", (DateTime.Now.Minute + 30));
                            if (cmd.ExecuteNonQuery() > 0)
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
                        //Update Auth Key if key already Exists and also not used previous key
                        con.Close();
                        con.Open();
                        using (cmd = new SqlCommand(Updatequery, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(new SqlParameter("@emailId", EmailId));
                            cmd.Parameters.AddWithValue("@Authorizationkey", Guid.NewGuid().ToString());
                            cmd.Parameters.AddWithValue("@RequestTime", DateTimeOffset.UtcNow);
                            cmd.Parameters.AddWithValue("@IsUsed", false);
                            cmd.Parameters.AddWithValue("@ValidateTill", (DateTime.Now.Minute + 30));
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
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
        
        #region Validate user key at password update time
        public bool ValidateUserKey(CommonModel key)
        {
            string CS = this._configuration.GetConnectionString("Dev");
            string query = "";
            using (SqlConnection con = new SqlConnection(CS))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@emailId", key.Value));
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        DataKey = (rdr["AuthorixationKey"].ToString() != null || rdr["AuthorixationKey"].ToString() != string.Empty) ? rdr["AuthorixationKey"].ToString() : string.Empty;

                    }
                    if (DataKey == key.Key)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

        }
        #endregion
        
        #region update password
        public bool UpdatePassword(CommonModel key)
        {
            string CS = this._configuration.GetConnectionString("Dev");
            string query = "";
            using (SqlConnection con = new SqlConnection(CS))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@", key.Key));
                    cmd.Parameters.AddWithValue("@", key.Value);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        } 
        #endregion
        
        #endregion

        #endregion
        #endregion
    }
}