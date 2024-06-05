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

    // This class handles database operations related to user login in the dating site,
    // including user authentication.
    public class LoginDBModel
    {
        DBConnect objDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();

        // Method to authenticate a user based on username and password
        public (int UserId, bool IsValid, string ErrorMessage) AuthenticateUser(string username, string password)
        {

            SqlParameter paramUsername = new SqlParameter("@Username", username);
            objCommand.Parameters.Add(paramUsername);
            SqlParameter parameterId = new SqlParameter("@UserId", SqlDbType.Int);
            parameterId.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(parameterId);
            SqlParameter paramHashedPassword = new SqlParameter("@PassReturned", SqlDbType.VarChar, -1);
            paramHashedPassword.Direction = ParameterDirection.Output;

            objCommand.Parameters.Add(paramHashedPassword);

            try
            {

                    objDB.GetDataSet(objCommand);
                int userId = Convert.ToInt32(objCommand.Parameters["@UserId"].Value);
                string returnedHash = paramHashedPassword.Value != DBNull.Value ? Convert.ToString(paramHashedPassword.Value) : null;

                if (userId > 0 && !string.IsNullOrEmpty(returnedHash) && Hasher.VerifyPassword(password, returnedHash))
                {
                    return (userId, true, null);
                }
                return (0, false, "Invalid username or password.");
            }
            catch (Exception ex)
            {
                return (0, false, ex.Message);
            }
        }
    }
}
