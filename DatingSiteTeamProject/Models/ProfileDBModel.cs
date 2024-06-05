using Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using DatingSiteTeamProject.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace DatingSiteTeamProject.Models
{
    // This class handles database operations related to user profiles in the dating site ,
    // including CRUD operations and profile searches.
    public class ProfileDBModel
    {
        DBConnect objDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();
        string strSQL;

        // Method to get user profile details by user ID
        public ProfileModel GetUserProfile(int userId)
        {
            ProfileModel profile = new ProfileModel();
            objCommand.Parameters.Clear();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.ViewUserProfileDetail";
            objCommand.Parameters.AddWithValue("@EnterUserId", userId);

            DataSet myDS = objDB.GetDataSet(objCommand);
            if (myDS.Tables.Count > 0 && myDS.Tables[0].Rows.Count > 0)
            {
                DataRow row = myDS.Tables[0].Rows[0];
                profile = new ProfileModel
                {
                    PhotoUrl = row["PhotoUrl"].ToString(),
                    Age = row["Age"].ToString(),
                    Height = Math.Round(float.Parse(row["Height"].ToString()), 1).ToString("F1"),
                    Weight = Math.Round(float.Parse(row["Weight"].ToString()), 0).ToString("F2"),
                    Description = row["Description"].ToString(),
                    Quote = row["Quote"].ToString(),
                    Movie = row["Movie"].ToString(),
                    Book = row["Book"].ToString(),
                    City = row["City"].ToString(),
                    State = row["State"].ToString(),
                    OtherInterests = row["Interests"].ToString(),
                    Occupation = row["Occupation"].ToString(),
                    Food = row["Food"].ToString(),
                    LastMeal = row["LastMeal"].ToString(),
                    DreamDestinations = row["DreamDestinations"].ToString(),
                    GoalsInFive = row["GoalsInFive"].ToString(),
                    DealBreakers = row["DealBreakers"].ToString(),
                    Languages = row["Languages"].ToString(),
                    Commitment = row["Commitment"].ToString(),
                    Status = Convert.ToBoolean(row["Private"]),
                };

            }
            return profile;

        }

        // Method to get the list of states
        public List<SelectListItem> GetStates()
        {
            objCommand.Parameters.Clear();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GenerateStates";
            DataSet dsStates = objDB.GetDataSet(objCommand);
            List<SelectListItem> stateList = new List<SelectListItem>();

            foreach (DataRow row in dsStates.Tables[0].Rows)
            {
                stateList.Add(new SelectListItem
                {
                    Text = row["StateName"].ToString(),
                    Value = row["USStateAbbreviations"].ToString()
                });
            }

            return stateList;
        }

        // Method to search for profiles based on various criteria
        public List<AuthenicationModel> SearchProfiles(int currentUser, string occupation, string city, string state, int lowerAge, int upperAge, string commitmentTypes)
        {
            List<ProfileModel> profiles = new List<ProfileModel>();
            List<AuthenicationModel> listOfProfiles = new List<AuthenicationModel>();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "SearchFilter";

            objCommand.Parameters.AddWithValue("@currentUser", currentUser);
            objCommand.Parameters.AddWithValue("@EnterOccupation", occupation);
            objCommand.Parameters.AddWithValue("@EnterCity", city);
            objCommand.Parameters.AddWithValue("@EnterState", state);
            objCommand.Parameters.AddWithValue("@LowerAge", lowerAge);
            objCommand.Parameters.AddWithValue("@UpperAge", upperAge);
            objCommand.Parameters.AddWithValue("@CommitmentTypes", commitmentTypes);

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

        // Method to insert a profile image
        public bool InsertImage(ImageInfo imageInfo, int userId)
        {

            objCommand.Parameters.Clear();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.InsertImage";
            objCommand.Parameters.AddWithValue("@EnterImageData", imageInfo.Data);
            objCommand.Parameters.AddWithValue("@EnterImageType", imageInfo.Type);
            objCommand.Parameters.AddWithValue("@EnterUserId", userId);
            objCommand.Parameters.AddWithValue("@EnterImageTitle", imageInfo.ImageTitle);
            objCommand.Parameters.AddWithValue("@EnterImageDescription", imageInfo.ImageDescription);
            try
            {
                objDB.DoUpdate(objCommand);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Method to delete an image
        public bool DeleteImage(int userId, string imageTitle, string imageDescription)
        {

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.DeleteImage";
            objCommand.Parameters.AddWithValue("@EnterUserId", userId);
            objCommand.Parameters.AddWithValue("@EnterImageTitle", imageTitle);
            objCommand.Parameters.AddWithValue("@EnterImageDescription", imageDescription);

            try
            {
                objDB.DoUpdate(objCommand);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            

        }

        //returns all profile images by user id
        public DataSet GetProfileImages(int userId)
        {
            objCommand.Parameters.Clear();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.GetAllImages";
            objCommand.Parameters.AddWithValue("@EnterUserId", userId);

            return objDB.GetDataSet(objCommand);
        }

        //get profile details
        public DataSet GetProfileDetails(int userId)
        {
            objCommand.Parameters.Clear();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.ViewUserProfileDetail";
            objCommand.Parameters.AddWithValue("@EnterUserId", userId);

            return objDB.GetDataSet(objCommand);
        }

        //used to update security questions
        public void UpdateUserAnswers(int userId, string answer1, string answer2, string answer3)
        {
            objCommand.Parameters.Clear();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.UpdateUserAnswers";
            objCommand.Parameters.AddWithValue("@EnterUserId", userId);
            objCommand.Parameters.AddWithValue("@EnterAnswer1", answer1);
            objCommand.Parameters.AddWithValue("@EnterAnswer2", answer2);
            objCommand.Parameters.AddWithValue("@EnterAnswer3", answer3);

            objDB.DoUpdate(objCommand);
        }

        //inserts answers to security questions
        public bool InsertVerificationAnswers(int userId, string answer1, string answer2, string answer3)
        {
            try
            {
                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "VerificationAnswers";
                objCommand.Parameters.AddWithValue("@userID", userId);
                objCommand.Parameters.AddWithValue("@Answer1", answer1);
                objCommand.Parameters.AddWithValue("@Answer2", answer2);
                objCommand.Parameters.AddWithValue("@Answer3", answer3);

                int result = objDB.DoUpdate(objCommand);
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //used to update an exisiting user profile
        public void UpdateUserProfile(int userId, ProfileModel profile)
        {
            objCommand.Parameters.Clear();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.CheckAndEditProfile";

            List<string> allInterests = new List<string>(profile.Interests);
            if (!string.IsNullOrWhiteSpace(profile.OtherInterests))
            {
                allInterests.Add(profile.OtherInterests);
            }
            //gets profile list of strings and joins them into one string
            string interests = string.Join(",", allInterests);
            //string test = interests;
            objCommand.Parameters.AddWithValue("@UpdatePhotoUrl", profile.PhotoUrl);
            objCommand.Parameters.AddWithValue("@UpdateAge", Int32.Parse(profile.Age));
            objCommand.Parameters.AddWithValue("@UpdateHeight", float.Parse(profile.Height));
            objCommand.Parameters.AddWithValue("@UpdateWeight", float.Parse(profile.Weight));
            objCommand.Parameters.AddWithValue("@UpdateDescription", profile.Description);
            objCommand.Parameters.AddWithValue("@UpdateQuote", profile.Quote);
            objCommand.Parameters.AddWithValue("@UpdateMovie", profile.Movie);
            objCommand.Parameters.AddWithValue("@UpdateBook", profile.Book);
            objCommand.Parameters.AddWithValue("@UpdateCity", profile.City);
            objCommand.Parameters.AddWithValue("@UpdateState", profile.State);
            objCommand.Parameters.AddWithValue("@UpdateInterest", interests);
            objCommand.Parameters.AddWithValue("@UpdateOccupation", profile.Occupation);
            objCommand.Parameters.AddWithValue("@UpdateFood", profile.Food);
            objCommand.Parameters.AddWithValue("@UpdateLastMeal", profile.LastMeal);
            objCommand.Parameters.AddWithValue("@UpdateDreamDestinations", profile.DreamDestinations);
            objCommand.Parameters.AddWithValue("@UpdateGoalsInFive", profile.GoalsInFive);
            objCommand.Parameters.AddWithValue("@UpdateDealBreakers", profile.DealBreakers);
            objCommand.Parameters.AddWithValue("@UpdateLanguages", profile.Languages);
            objCommand.Parameters.AddWithValue("@UpdateCommitment", profile.Commitment);
            objCommand.Parameters.AddWithValue("@UpdatePrivate", profile.Status);
            objCommand.Parameters.AddWithValue("@EnterUserId", userId);

            objDB.DoUpdate(objCommand);
        }
        //returns a profile
        public DataSet GetProfile(int userId)
        {
            objCommand.Parameters.Clear();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.ViewUserProfile";
            objCommand.Parameters.AddWithValue("@UserBeingViewed", userId);

            return objDB.GetDataSet(objCommand);
        }
        //used to create new user profile
        public bool InsertUserProfile(ProfileModel profile, int userId)
        {
            objCommand.Parameters.Clear();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.CreateProfile";

            List<string> allInterests = new List<string>(profile.Interests);
            if (!string.IsNullOrWhiteSpace(profile.OtherInterests))
            {
                allInterests.Add(profile.OtherInterests);
            }
            //gets profile list of strings and joins them into one string
            string interests = string.Join(",", allInterests);

            SqlParameter paramImgUrl = new SqlParameter("@imgUrl", profile.PhotoUrl);
            SqlParameter paramAge = new SqlParameter("@age", Int32.Parse(profile.Age));
            SqlParameter paramHeight = new SqlParameter("@height", float.Parse(profile.Height));
            SqlParameter paramWeight = new SqlParameter("@weight", float.Parse(profile.Weight));
            SqlParameter paramDesc = new SqlParameter("@desc", profile.Description);
            SqlParameter paramQuote = new SqlParameter("@quote", profile.Quote);
            SqlParameter paramMovie = new SqlParameter("@movie", profile.Movie);
            SqlParameter paramBook = new SqlParameter("@book", profile.Book);
            SqlParameter paramCity = new SqlParameter("@city", profile.City);
            SqlParameter paramState = new SqlParameter("@state", profile.State);
            SqlParameter paramOccupation = new SqlParameter("@occupation", profile.Occupation);
            SqlParameter paramInterests = new SqlParameter("@interests", interests);
            SqlParameter paramFood = new SqlParameter("@food", profile.Food);
            SqlParameter paramCommitment = new SqlParameter("@commitment", profile.Commitment);
            SqlParameter paramPublic = new SqlParameter("@view", profile.Status);
            SqlParameter paramLastMeal = new SqlParameter("@lastmeal", profile.LastMeal);
            SqlParameter paramDreamDestinations = new SqlParameter("@dreamDestinations", profile.DreamDestinations);
            SqlParameter paramGoalsInFive = new SqlParameter("@goalsInFive", profile.GoalsInFive);
            SqlParameter paramDealBreakers = new SqlParameter("@dealBreakers", profile.DealBreakers);
            SqlParameter paramLanguages = new SqlParameter("@languages", profile.Languages);
            //need to get user id some other way...?
            SqlParameter paramUserId = new SqlParameter("@userID", profile.UserId);

            //ProfileID automatically created in DB 
            objCommand.Parameters.Add(paramImgUrl);
            objCommand.Parameters.Add(paramAge);
            objCommand.Parameters.Add(paramHeight);
            objCommand.Parameters.Add(paramWeight);
            objCommand.Parameters.Add(paramDesc);
            objCommand.Parameters.Add(paramQuote);
            objCommand.Parameters.Add(paramMovie);
            objCommand.Parameters.Add(paramBook);
            objCommand.Parameters.Add(paramCity);
            objCommand.Parameters.Add(paramState);
            objCommand.Parameters.Add(paramInterests);
            objCommand.Parameters.Add(paramOccupation);
            objCommand.Parameters.Add(paramFood);
            objCommand.Parameters.Add(paramCommitment);
            objCommand.Parameters.Add(paramPublic);
            objCommand.Parameters.Add(paramUserId);
            objCommand.Parameters.Add(paramLastMeal);
            objCommand.Parameters.Add(paramDreamDestinations);
            objCommand.Parameters.Add(paramGoalsInFive);
            objCommand.Parameters.Add(paramDealBreakers);
            objCommand.Parameters.Add(paramLanguages);

            SqlParameter parameterProfileID = new SqlParameter("@profileID", SqlDbType.Int);
            parameterProfileID.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(parameterProfileID);

            int rowsAffected = objDB.DoUpdate(objCommand);
            if (rowsAffected > 0)
            {
                profile.UserId = Convert.ToInt32(parameterProfileID.Value);
                return true;
            }
            return false;
        }
    
    }
}


