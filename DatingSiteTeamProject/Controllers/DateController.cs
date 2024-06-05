using DatingSiteTeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DatingSiteTeamProject.Controllers
{
    // This controller handles date-related operations in the dating site,
    // including viewing date requests, responding to date requests, and modifying date details.

    public class DateController : Controller
    {
        // Action to display the date view with lists of incoming, accepted, and sent date requests
        public IActionResult DateView()
        {
            string userIdCookie = Request.Cookies["UserId"];
            ViewBag.UserId = userIdCookie;
            if (userIdCookie == null)
            {
                return RedirectToAction("Registration_View", "Registration");
            }

            int userId = int.Parse(userIdCookie);
            string listOfIncomingDateRequestsApiUrl = $"https://cis-iis2.temple.edu/Spring2024/CIS3342_TUG41350/WebApi/api/DateApi/GetListOfIncomingDateRequests?id={userId}";
            string listOfAcceptedRequestsApiUrl = $"https://cis-iis2.temple.edu/Spring2024/CIS3342_TUG41350/WebApi/api/DateApi/GetListOfAcceptedRequests?id={userId}";
            string listOfSentRequestsApiUrl = $"https://cis-iis2.temple.edu/Spring2024/CIS3342_TUG41350/WebApi/api/DateApi/GetListOfSentRequests?id={userId}";

            List<AuthenicationModel> listOfIncomingDateRequests = DatingHttp.GetProfilesFromApi(listOfIncomingDateRequestsApiUrl);
            List<AuthenicationModel> listOfAcceptedRequests = DatingHttp.GetProfilesFromApi(listOfAcceptedRequestsApiUrl);
            List<AuthenicationModel> listOfSentRequests = DatingHttp.GetProfilesFromApi(listOfSentRequestsApiUrl);

            DateViewModel viewModel = new DateViewModel
            {
                ListOfIncomingDateRequests = listOfIncomingDateRequests,
                ListOfAcceptedRequests = listOfAcceptedRequests,
                ListOfSentRequests = listOfSentRequests
            };

            return View(viewModel);
        }

        // Action to respond to a date request
        public IActionResult RespondToDateRequest(int senderId, int recieverId, string response)
        {
      
            DateModel date = new DateModel(senderId, recieverId, response);

            string jsonDate = JsonSerializer.Serialize(date);

            string apiUrl = $"https://cis-iis2.temple.edu/Spring2024/CIS3342_TUG41350/WebApi/api/DateApi/RespondToDateRequest";

            DatingHttp.SendHttpRequest(apiUrl, "PUT", jsonDate);

            return RedirectToAction("ProfileHomeView", "Profile");

        }

        // Action to modify a date request
        public IActionResult ModifyDateRequest(int senderId, int recieverId, DateTime dayAndMonthOfDate, TimeSpan time, string dateDescription, string location)
        {
            string dateAsString = dayAndMonthOfDate.ToString("yyyy-MM-dd"); 
            string timeAsString = time.ToString(@"hh\:mm");
            DateModel date = new DateModel(senderId, recieverId, dateAsString, timeAsString, dateDescription, location);
            string jsonDate = JsonSerializer.Serialize(date);

            string apiUrl = $"https://cis-iis2.temple.edu/Spring2024/CIS3342_TUG41350/WebApi/api/DateApi/ModifyDate";

            DatingHttp.SendHttpRequest(apiUrl, "PUT", jsonDate);

            return RedirectToAction("ProfileHomeView", "Profile");

        }
    }
}
