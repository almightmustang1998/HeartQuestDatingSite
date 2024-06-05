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

namespace DatingSiteTeamProject.Controllers
{
    // This controller handles updating profile-related operations in the dating site,
    // including viewing the profile update page, updating user profiles, uploading and deleting images.
    public class UpdateProfileController : Controller
    {
        DBConnect objDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();
        ProfileDBModel profileDBModel = new ProfileDBModel();

        //action to display the updated profile view
        public async Task<IActionResult> UpdateProfile_View()
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
                DataSet dsProfile = profileDBModel.GetProfileDetails(userId);
                if (dsProfile.Tables.Count > 0 && dsProfile.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsProfile.Tables[0].Rows[0];
                    profile.PhotoUrl = row["PhotoUrl"].ToString();
                    profile.Age = row["Age"].ToString();
                    profile.Height = Math.Round(float.Parse(row["Height"].ToString()), 1).ToString("F1");
                    profile.Weight = Math.Round(float.Parse(row["Weight"].ToString()), 0).ToString("F0");
                    profile.Description = row["Description"].ToString();
                    profile.Quote = row["Quote"].ToString();
                    profile.Movie = row["Movie"].ToString();
                    profile.Book = row["Book"].ToString();
                    profile.City = row["City"].ToString();
                    profile.State = row["State"].ToString();
                    profile.OtherInterests = row["Interests"].ToString();
                    profile.Occupation = row["Occupation"].ToString();
                    profile.Food = row["Food"].ToString();
                    profile.LastMeal = row["LastMeal"].ToString();
                    profile.DreamDestinations = row["DreamDestinations"].ToString();
                    profile.GoalsInFive = row["GoalsInFive"].ToString();
                    profile.DealBreakers = row["DealBreakers"].ToString();
                    profile.Languages = row["Languages"].ToString();
                    profile.Commitment = row["Commitment"].ToString();
                    profile.Status = Convert.ToBoolean(row["Private"]);
                    profile.Answer1 = row["Q1"].ToString();
                    profile.Answer2 = row["Q2"].ToString();
                    profile.Answer3 = row["Q3"].ToString();

                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "An error occurred while loading profile data: " + ex.Message;
            }

            return View(profile);
        }
        //action to update user profile information
        [HttpPost]
        public IActionResult UpdateUserProfile(ProfileModel profile)
        {
            string userIdCookie = Request.Cookies["UserId"];
            int userId = Convert.ToInt32(userIdCookie);
            ModelState.Remove("Images");
            ModelState.Remove("UploadStatus");
            ModelState.Remove("imageUploadForm");
            ModelState.Remove("OtherInterests");
            ModelState.Remove("ImageFile");
            ModelState.Remove("AddImage");
            ModelState.Remove("ImageTitle");
            ModelState.Remove("ImageDescription");
         
            string a = profile.Age;
            int i = profile.UserId;
            if (profile != null && ModelState.IsValid)
            {
                profileDBModel.UpdateUserProfile(userId, profile);
                profileDBModel.UpdateUserAnswers(userId, profile.Answer1, profile.Answer2, profile.Answer3);
                return RedirectToAction("UpdateProfile_View");
            }
            return View("UpdateProfile_View", profile);
        }

        //action to upload an image
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
                    return RedirectToAction("UpdateProfile_View");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please select an image to upload.";
            }
            return View("UpdateProfile_View");

        }

        //action to delete an image
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
            }
            else
            {
                TempData["ErrorMessage"] = "Error deleting image.";
            }
            return RedirectToAction("UpdateProfile_View");
        }

    }
}
