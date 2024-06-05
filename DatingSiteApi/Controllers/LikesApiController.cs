using DatingSiteApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Utilities;
using System.Data;
using System.Data.Common;

// This controller manages like operations in the dating site, including liking and unliking users,
// and retrieving lists of profiles that liked the current user and profiles the current user likes.
// It uses LikeModel to process requests and returns AuthenicationModel objects.

namespace DatingSiteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesApiController : Controller
    {
        //endpoint to like a user
        [HttpPost("LikeUser")]
        public Boolean LikeUser([FromBody] LikeModel like) 
        {
            int result = like.LikeUser();

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //endpoint to unlike a user
        [HttpDelete("UnlikeUser")]
        public Boolean UnlikeUser([FromBody] LikeModel like)
        {
            int result = like.Unlike();

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Endpoint to get profiles that like the current user
        [HttpGet("GetProfilesThatLikeCurrentUser")]
        public List<AuthenicationModel> GetProfilesThatLikeCurrentUser(int id)
        {
            List<AuthenicationModel> listOfProfiles = new List<AuthenicationModel>();
            LikeModel like = new LikeModel();
            like.LikerId = id;

            DataSet profilesDataSet = like.GetLikedYou();


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
                    DreamDestinations = row["DreamDestinations"].ToString(),
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

        // Endpoint to get profiles that the current user likes
        [HttpGet("GetProfilesThatCurrentUserLikes")]
        public List<AuthenicationModel> GetProfilesThatCurrentUserLikes(int id)
        {
            List<AuthenicationModel> listOfProfiles = new List<AuthenicationModel>();
            LikeModel like = new LikeModel();
            like.LikerId = id;

            DataSet profilesDataSet = like.GetLikedByUser();

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
