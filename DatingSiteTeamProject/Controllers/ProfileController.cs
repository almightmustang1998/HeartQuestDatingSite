using Microsoft.AspNetCore.Mvc;
using DatingSiteTeamProject.Models;
using System.Data.SqlClient;
using Utilities;
using System.Data.Common;
using System.Reflection;
using System.Data;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.X509Certificates;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DatingSiteTeamProject.Controllers
{
    // This controller handles profile-related operations in the dating site,
    // including viewing profiles, inserting profiles, uploading and deleting images, and filtering searches.
    public class ProfileController : Controller
    {
        DBConnect objDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();
        ProfileDBModel profileDBModel = new ProfileDBModel();

        // Action to display the profile home view with the list of profiles
        public IActionResult ProfileHomeView()
        {
            ViewBag.States = profileDBModel.GetStates();

            string userIdCookie = Request.Cookies["UserId"];
            ViewBag.UserId = userIdCookie;
            if (userIdCookie == null)
            {
                return RedirectToAction("Registration_View", "Registration");
            }

            int userId = int.Parse(userIdCookie);
            string apiUrl = $"https://cis-iis2.temple.edu/Spring2024/CIS3342_TUG41350/WebApi/api/ProfileApi/GetUserProfiles?userId={userId}";

            WebRequest request = WebRequest.Create(apiUrl);
            WebResponse response = request.GetResponse();

            Stream theDataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(theDataStream);

            string data = reader.ReadToEnd();
            reader.Close();
            response.Close();

            List<AuthenicationModel> listOfProfiles = JsonSerializer.Deserialize<List<AuthenicationModel>>(data);

            return View(listOfProfiles);
        }

        // Action to view a specific profile by profile ID
        public IActionResult ViewProfile(int profileId)
        {
            string userIdCookie = Request.Cookies["UserId"];
            ViewBag.UserId = userIdCookie;
            if (userIdCookie == null)
            {
                return RedirectToAction("Registration_View", "Registration");
            }
            try
            {
                DataSet profilesDataSet = profileDBModel.GetProfile(profileId);
                if (profilesDataSet.Tables.Count > 0 && profilesDataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow userProfileRow = profilesDataSet.Tables[0].Rows[0];
                    ProfileModel profileModel = new ProfileModel
                    {
                        Age = userProfileRow["Age"].ToString(),
                        City = userProfileRow["City"].ToString(),
                        State = userProfileRow["State"].ToString(),
                        PhotoUrl = userProfileRow["PhotoUrl"].ToString(),
                        Commitment = userProfileRow["Commitment"].ToString(),
                        Description = userProfileRow["Description"].ToString(),
                        Quote = userProfileRow["Quote"].ToString(),
                        Movie = userProfileRow["Movie"].ToString(),
                        OtherInterests = userProfileRow["Interests"].ToString(),
                        Book = userProfileRow["Book"].ToString(),
                        Occupation = userProfileRow["Occupation"].ToString(),
                        Food = userProfileRow["Food"].ToString(),
                        LastMeal = userProfileRow["LastMeal"].ToString(),
                        DreamDestinations = userProfileRow["DreamDestinations"].ToString(),
                        GoalsInFive = userProfileRow["GoalsInFive"].ToString(),
                        DealBreakers = userProfileRow["DealBreakers"].ToString(),
                        Languages = userProfileRow["Languages"].ToString(),
                        Images = new List<ImageInfo>()
                    };
                    if (profilesDataSet.Tables[0].Columns.Contains("ImageData"))
                    {
                        foreach (DataRow imageRow in profilesDataSet.Tables[0].Rows)
                        {
                            profileModel.Images.Add(new ImageInfo
                            {
                                Data = (byte[])imageRow["ImageData"],
                                ImageTitle = imageRow["ImageTitle"].ToString(),
                                ImageDescription = imageRow["ImageDescription"].ToString(),
                                Type = imageRow["ImageType"].ToString()
                            });
                        }
                    }
                    UserModel userModel = new UserModel
                    {
                        Name = userProfileRow["Fullname"].ToString(),
                        Username = userProfileRow["Username"].ToString(),
                        Id = Convert.ToInt32(userProfileRow["Id"])
                    };

                    AuthenicationModel authenticatedProfile = new AuthenicationModel
                    {
                        Profile = profileModel,
                        User = userModel
                    };

                    return View(authenticatedProfile);
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        // Action to insert a new profile view
        public async Task<IActionResult> InsertProfile_View()
        {
            ProfileModel profile = new ProfileModel();
            string userIdCookie = Request.Cookies["UserId"];
            if (!int.TryParse(userIdCookie, out int userId))
            {
                ViewData["Error"] = "Invalid User ID.";
                return View(profile);
            }
            profile.UserId = userId;
            try
            {
                // Fetch all images for the user
                DataSet dsImages = profileDBModel.GetProfileImages(userId);
                profile.Images = new List<ImageInfo>();
                if (dsImages.Tables.Count > 0 && dsImages.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsImages.Tables[0].Rows)
                    {
                        profile.Images.Add(new ImageInfo
                        {
                            Data = (byte[])row["ImageData"],
                            Type = row["ImageType"].ToString(),
                            ImageTitle = row["ImageTitle"].ToString(),
                            ImageDescription = row["ImageDescription"].ToString()

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "An error occurred while loading profile data: " + ex.Message;
            }
            return View(profile);
        }

        // Action to upload an image
        [HttpPost]
        public async Task<IActionResult> UploadImage(ImageInfo imageInfo, IFormFile imageFile)
        {
            if (imageFile != null && imageInfo != null && ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    imageInfo.Data = memoryStream.ToArray();
                    imageInfo.Type = imageFile.ContentType;
                }
                string userIdCookie = Request.Cookies["UserId"];
                int userId = int.Parse(userIdCookie);

                bool uploadSuccess = profileDBModel.InsertImage(imageInfo, userId);
                if (uploadSuccess)
                {
                    TempData["UploadStatus"] = "Image was successfully uploaded.";
                    return RedirectToAction("InsertProfile_View");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please select an image to upload.";
            }
            return View("InsertProfile_View");
        }

        // Action to delete an image
        [HttpPost]
        public IActionResult DeleteImage(string ImageTitle, string ImageDescription)
        {
            string userIdCookie = Request.Cookies["UserId"];
            int userId = int.Parse(userIdCookie);
            string a = ImageTitle;
            string b = ImageDescription;
            bool success = profileDBModel.DeleteImage(userId, ImageTitle, ImageDescription);
            if (success)
            {
                TempData["SuccessMessage"] = "Image successfully deleted.";
            }else{
                TempData["ErrorMessage"] = "Error deleting image.";
            }
            return RedirectToAction("InsertProfile_View");
        }

        //action to insert a new user profile
        [HttpPost]
        public IActionResult InsertUserProfile(ProfileModel profile)
        {
            ModelState.Remove("Images");
            ModelState.Remove("UploadStatus");
            ModelState.Remove("imageUploadForm");
            ModelState.Remove("OtherInterests");
            ModelState.Remove("ImageFile");
            ModelState.Remove("AddImage");
            ModelState.Remove("ImageTitle");
            ModelState.Remove("ImageDescription");

            string userIdCookie = Request.Cookies["UserId"];
            if (userIdCookie == null)
            {
                return RedirectToAction("Login_View", "Login");
            }
            int userId = int.Parse(userIdCookie);
            profile.UserId = userId;
            if (profile != null && ModelState.IsValid)
            {

                bool isInserted = profileDBModel.InsertUserProfile(profile, userId);

                //int profileId = (int)objCommand.Parameters["@profileID"].Value;
                bool isUpdated = profileDBModel.InsertVerificationAnswers(userId, profile.Answer1, profile.Answer2, profile.Answer3);
                if (isInserted && isUpdated)
                {
                    ViewData["SuccessMessage"] = "Successfully created user profile.";
                    return RedirectToAction("ProfileHomeView", "Profile", new { id = profile.UserId });
                }else{
                    ViewData["ValidationMessage"] = "Username may be taken. Please choose another one.";
                    return View();
                }
            }
            return View("InsertProfile_View");
        }

        //action to search profiles based on filters
        [HttpPost]
        public IActionResult SearchFilter(string occupation, string city, string state, int lowerAge, int upperAge, string[] commitmentTypes)
        {
            string userIdCookie = Request.Cookies["UserId"];
            ViewBag.UserId = userIdCookie;
            ViewBag.States = profileDBModel.GetStates();
            ViewBag.SelectedState = state;
            ViewBag.Occupation = occupation;
            ViewBag.City = city;
            ViewBag.LowerAge = lowerAge;
            ViewBag.UpperAge = upperAge;
            //ViewBag.FriendsType = cTypes;
            //ViewBag.SelectedCommitmentTypes = commitmentTypes[0];
            if (userIdCookie == null)
            {
                return RedirectToAction("Registration_View", "Registration");
            }
            int currentUser = int.Parse(userIdCookie);
            if (string.IsNullOrWhiteSpace(occupation))
            {
                occupation = "";
            }
            if (string.IsNullOrWhiteSpace(city))
            {
                city = "";
            }
            if (string.IsNullOrWhiteSpace(state))
            {
                state = "Any";
            }
            if (lowerAge == null || lowerAge <= 0)
            {
                lowerAge = 0;
            }

            if (upperAge == null || upperAge <= 0)
            {
                upperAge = 1000;
            }
            string commitmentTypeString = "";
            if (commitmentTypes != null && commitmentTypes.Length > 0)
            {
                commitmentTypeString = string.Join(",", commitmentTypes);
            }
            var profiles = profileDBModel.SearchProfiles(currentUser, occupation, city, state, lowerAge, upperAge, commitmentTypeString);
            return View("ProfileHomeView", profiles);
        }
    }
}
