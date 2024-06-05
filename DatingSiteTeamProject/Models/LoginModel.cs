using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using DatingSiteTeamProject.Helpers;
using Utilities;

namespace DatingSiteTeamProject.Models
{
    // This class defines a model for user login in the dating site,
    // including properties for username, password, user ID, and the "remember me" option.
    public class LoginModel
    {
        private string username;
        private string password;
        private int id;
        private bool rememberMe;

        //constructor for userModel
        public LoginModel()
        {
            this.username = string.Empty;
            this.password = string.Empty;
            this.rememberMe = false;
        }


        public LoginModel( string userName, string password, int id, bool rememberMe)
        {
            this.username = userName;
            this.password = password;
            this.id = id;
            this.rememberMe = rememberMe;
        }

        public LoginModel(string userName, string password)
        {

            this.username = userName;
            this.password = password;
        }


        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Username should only contain letters and numbers!")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }

        [Required(ErrorMessage = "Password is required.")]
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public bool RememberMe
        {
            get
            {
                return rememberMe;
            }
            set
            {
                rememberMe = value;
            }
        }
        public void CheckRememberMe(IRequestCookieCollection cookies)
        {
            if (cookies.ContainsKey("UserId"))
            {
                this.RememberMe = true;
            }
            else
            {
                this.RememberMe = false;
            }
        }

        private LoginModel GetUserDetailsById(int userId)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetUserDetailsById";
            objCommand.Parameters.AddWithValue("@UserId", userId);

            DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                return new LoginModel
                {
                    Username = row["Username"].ToString(),
                    // You should NOT send the password to the front end even if it is hashed.
                    Password = "", // Leave this empty for security
                    RememberMe = true
                };
            }
            return new LoginModel(); // return an empty model if no user found
        }

        //need to learn how to add cookie here
    }
}
