using DatingSiteApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Utilities;
using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Numerics;

//// This controller handles date-related operations in the dating site, including requesting, responding, modifying dates,
// and retrieving lists of incoming, accepted, and sent date requests. It uses DateModel to process requests and returns AuthenicationModel objects.

namespace DatingSiteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DateApiController : Controller
    {
    
        // Endpoint to handle date requests
        [HttpPost("RequestDate")]
        public Boolean RequestDate([FromBody] DateModel date)
        {
            int result = date.RequestDate();

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Endpoint to handle responses to date requests
        [HttpPut("RespondToDateRequest")]
        public Boolean RespondToDateRequest([FromBody] DateModel date)
        {
            int result = date.RespondToDateRequest();

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Endpoint to modify date details
        [HttpPut("ModifyDate")]
        public Boolean ModifyDate([FromBody] DateModel date)
        {
            int result = date.ModifyDate();

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Endpoint to retrieve a list of incoming date requests for a given user
        [HttpGet("GetListOfIncomingDateRequests")]
        public List<AuthenicationModel> GetListOfIncomingDateRequests(int id)
        {
            List<AuthenicationModel> listOfProfiles = new List<AuthenicationModel>();
            DateModel date = new DateModel();
            date.SenderId = id;

            DataSet profilesDataSet = date.GetListOfIncomingDateRequests();

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
                    Height = row["Height"].ToString(),
                    Weight = row["Weight"].ToString(),
                };

                UserModel userModel = new UserModel
                {
                    Name = row["Fullname"].ToString(),
                    Username = row["Username"].ToString(),
                    Email = row["Email"].ToString(),
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

        // Endpoint to retrieve a list of accepted date requests for a given user
        [HttpGet("GetListOfAcceptedRequests")]
        public List<AuthenicationModel> GetListOfAcceptedRequests(int id)
        {
            List<AuthenicationModel> listOfProfiles = new List<AuthenicationModel>();
            DateModel date = new DateModel();
            date.SenderId = id;

            DataSet profilesDataSet = date.GetListOfAcceptedRequests();

            foreach (DataRow row in profilesDataSet.Tables[0].Rows)
            {

                ProfileModel profileModel = new ProfileModel
                {
                    Age = row["Age"].ToString(),
                    City = row["City"].ToString(),
                    State = row["State"].ToString(),
                    PhotoUrl = row["PhotoUrl"].ToString(),
                    Commitment = row["Commitment"].ToString(),
                    Occupation = row["Occupation"].ToString(),
                    Description = row["Description"].ToString(),
                    Height = row["Height"].ToString(),
                    Weight = row["Weight"].ToString(),
                };

                UserModel userModel = new UserModel
                {
                    Name = row["Fullname"].ToString(),
                    Username = row["Username"].ToString(),
                    Email = row["Email"].ToString(),
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

        // Endpoint to retrieve a list of sent date requests for a given user
        [HttpGet("GetListOfSentRequests")]
        public List<AuthenicationModel> GetListOfSentRequests(int id)
        {
            List<AuthenicationModel> listOfProfiles = new List<AuthenicationModel>();
            DateModel date = new DateModel();
            date.SenderId = id;

            DataSet profilesDataSet = date.GetListOfSentRequests();

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
