using DatingSiteTeamProject.Helpers;
using System.Data.SqlClient;
using System.Data;
using Utilities;
using System.Data.Common;

namespace DatingSiteTeamProject.Models
{
    //used to register a new user to the site
    public class RegistrationDBModel
    {
        //check to see if userModel object is passed

        public int RegisterUser(UserModel user)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.Register";

            string hashedPassword = Hasher.HashPassword(user.Password);
            objCommand.Parameters.AddWithValue("@full_name", user.Name);
            objCommand.Parameters.AddWithValue("@user_name", user.Username);
            objCommand.Parameters.AddWithValue("@email", user.Email);
            objCommand.Parameters.AddWithValue("@pass_word", hashedPassword);

            // Output parameter for user ID
            SqlParameter parameterUserId = new SqlParameter("@UserId", SqlDbType.Int);
            parameterUserId.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(parameterUserId);

            try
            {
                objDB.DoUpdate(objCommand);
                return Convert.ToInt32(objCommand.Parameters["@UserId"].Value);
            }
            catch (Exception ex)
            {
                return -1;
            }

        }
    }

}
