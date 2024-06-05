using DatingSiteTeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;

namespace DatingSiteTeamProject.Controllers
{
    // This controller handles 'like' operations in the dating site,
    // including liking/unliking a user and viewing likes for the current user.

    public class LikesController : Controller
    {
        public IActionResult LikeUser(int likerId, int likeId)
        {
            LikeModel like = new LikeModel(likerId, likeId);

            string jsonLike = JsonSerializer.Serialize(like);

            string apiUrl = $"https://cis-iis2.temple.edu/Spring2024/CIS3342_TUG41350/WebApi/api/LikesApi/LikeUser";

            DatingHttp.SendHttpRequest(apiUrl, "POST", jsonLike);

            return RedirectToAction("ProfileHomeView", "Profile");

        }


        public IActionResult UnlikeUser(int likerId, int likeId)
        {
         
            LikeModel like = new LikeModel(likerId, likeId);

            string jsonLike = JsonSerializer.Serialize(like);

            string apiUrl = $"https://cis-iis2.temple.edu/Spring2024/CIS3342_TUG41350/WebApi/api/LikesApi/UnlikeUser";

            DatingHttp.SendHttpRequest(apiUrl, "DELETE", jsonLike);

            return RedirectToAction("ProfileHomeView", "Profile");
        }

        //view likes for current user
        public IActionResult LikesView()
        {
            string userIdCookie = Request.Cookies["UserId"];
            ViewBag.UserId = userIdCookie;
            if (userIdCookie == null)
            {
                return RedirectToAction("Registration_View", "Registration");
            }

            int userId = int.Parse(userIdCookie);
            string profilesThatLikeCurrentUserApiUrl = $"https://cis-iis2.temple.edu/Spring2024/CIS3342_TUG41350/WebApi/api/LikesApi/GetProfilesThatLikeCurrentUser?id={userId}";
            string profilesThatCurrentUserLikesApiUrl = $"https://cis-iis2.temple.edu/Spring2024/CIS3342_TUG41350/WebApi/api/LikesApi/GetProfilesThatCurrentUserLikes?id={userId}"; ;

            List<AuthenicationModel> profilesThatLikeCurrentUser = DatingHttp.GetProfilesFromApi(profilesThatLikeCurrentUserApiUrl);
            List<AuthenicationModel> profilesThatCurrentUserLikes = DatingHttp.GetProfilesFromApi(profilesThatCurrentUserLikesApiUrl);

            LikesViewModel viewModel = new LikesViewModel
            {
                ProfilesThatLikeUser = profilesThatLikeCurrentUser,
                ProfilesLikedByUser = profilesThatCurrentUserLikes
            };

            return View(viewModel);

        }


        
    }
}
