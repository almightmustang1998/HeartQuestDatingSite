using DatingSiteApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Utilities;

// This controller handles match-related operations in the dating site,
// specifically retrieving a list of matches for a given user.

namespace DatingSiteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchApiController : Controller
    {
        // Endpoint to get matches for the specified user ID
        [HttpGet]
        [HttpGet("GetMatches")]
        public List<AuthenicationModel> GetMatches(int id)
        {
            List<AuthenicationModel> listOfProfiles = new List<AuthenicationModel>();
            MatchModel match = new MatchModel();
            match.MatcherId = id;

            DataSet profilesDataSet = match.ViewMatches();

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
                    Occupation = row["Occupation"].ToString(),
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
