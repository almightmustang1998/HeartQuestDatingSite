using Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using DatingSiteTeamProject.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Components.Web;
using DatingSiteTeamProject.Helpers;


namespace DatingSiteTeamProject.Models
{
    // This class handles database operations related to user accounts in the dating site,
    // including updating user details and retrieving user information.
    public class AccountDBModel
    {
        DBConnect objDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();

        public int UpdateUser(UserModel user, string userIdCookie)
        {

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.UpdateUser";

            string hashedPassword = Hasher.HashPassword(user.Password);

            objCommand.Parameters.AddWithValue("@UpdateFullName", user.Name);
            objCommand.Parameters.AddWithValue("@UpdateUsername", user.Username);
            objCommand.Parameters.AddWithValue("@UpdateEmailAddress", user.Email);
            objCommand.Parameters.AddWithValue("@UpdatePassword", hashedPassword);
            objCommand.Parameters.AddWithValue("@CurrentUserId", int.Parse(userIdCookie));

            try
            {
                return objDB.DoUpdate(objCommand);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public UserModel GetUserDetails(int userId)
        {
            objCommand.Parameters.Clear();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.ViewUserAccount";

            //SqlParameter paramHashedPassword = new SqlParameter("@PassReturned", SqlDbType.VarChar, -1);
            //paramHashedPassword.Direction = ParameterDirection.Output;

            //objCommand.Parameters.Add(paramHashedPassword);

            objCommand.Parameters.AddWithValue("@EnterUserId", userId);
            //string correctPassword = Hasher.HashPassword(objCommand.Parameters["@PassReturned"].Value.ToString());

            try
            {
                DataSet ds = objDB.GetDataSet(objCommand);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    return new UserModel
                    {
                        Id = userId,
                        Name = row["Fullname"].ToString(),
                        Email = row["Email"].ToString(),
                        Username = row["Username"].ToString()
                    };
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
