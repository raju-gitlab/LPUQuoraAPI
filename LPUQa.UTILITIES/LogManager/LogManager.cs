using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.UTILITIES.LogManager
{
    public class LogManager
    {
        #region Constructor And Parameteres
        private readonly IConfiguration _configuration;
        public LogManager(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        #endregion

        LogManager() { }
        LogManager(Exception ex)
        {
            string CS = this._configuration.GetConnectionString("Dev");
            string query = "";
            SqlTransaction transaction = null;
            using(SqlConnection con = new SqlConnection(CS))
            {
                try
                {
                    con.Open();
                    transaction = con.BeginTransaction("First");
                    using(SqlCommand cmd = new SqlCommand(query,con,transaction))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("", ex.Message);
                        cmd.Parameters.AddWithValue("", ex.StackTrace);
                        cmd.Parameters.AddWithValue("", ex.InnerException);
                        cmd.Parameters.AddWithValue("", ex.Source);

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        LogManager(string ExceptionName, string LocatedIn) { }
    }
}
