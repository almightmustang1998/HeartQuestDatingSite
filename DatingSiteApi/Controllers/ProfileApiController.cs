using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DatingSiteApi.Models;
using System.Data.SqlClient;
using Utilities;
using System.Data;

// This controller handles profile-related operations in the dating site,
// specifically retrieving user profiles based on the current user's ID.

namespace DatingSiteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileApiController : Controller
    {
        DBConnect objDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();

        [HttpGet]
        [HttpGet("GetUserProfiles")]
        public List<AuthenicationModel> GetUserProfiles(int userId)
        {
            List<AuthenicationModel> listOfProfiles = new List<AuthenicationModel>();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.GenerateListOfProfiles";
            objCommand.Parameters.AddWithValue("@currentUser", userId);

            DataSet profilesDataSet = objDB.GetDataSet(objCommand);

            foreach (DataRow row in profilesDataSet.Tables[0].Rows)
            {
                ProfileModel profileModel = new ProfileModel
                {
                    Age = row["Age"].ToString(),
                    City = row["City"].ToString(),
                    State = row["State"].ToString(),
                    PhotoUrl = row["PhotoUrl"].ToString(),
                    Commitment = row["Commitment"].ToString(),
                    Description = row["Description"].ToString(),
                };

                UserModel userModel = new UserModel
                {
                    Name = row["Fullname"].ToString(),
                    Username = row["Username"].ToString(),
                    Id = (int)Convert.ToInt64(row["Id"]),
                };

                AuthenicationModel authenticatedProfile = new AuthenicationModel
                {
                    Profile = profileModel,
                    User = userModel
                };

                listOfProfiles.Add(authenticatedProfile);
            }
            return listOfProfiles;
        }

        

    }
}
