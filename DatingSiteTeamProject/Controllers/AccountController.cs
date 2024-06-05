using Microsoft.AspNetCore.Mvc;
using DatingSiteTeamProject.Models;
using System.Data.SqlClient;
using Utilities;
using System.Data.Common;
using System.Reflection;
using System.Data;
using DatingSiteTeamProject.Helpers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DatingSiteTeamProject.Controllers
{
    // This controller handles account-related operations in the dating site.
    // It includes actions for viewing and updating user account details.
    public class AccountController : Controller
    {
        DBConnect objDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();
        AccountDBModel accountDBModel = new AccountDBModel();

        // Action to view account details
        public IActionResult AccountDetail_View()
        {
            UserModel userModel = new UserModel();

            try
            {
                string userIdCookie = Request.Cookies["UserId"];
                if (int.TryParse(userIdCookie, out int userId))
                {
                    userModel = accountDBModel.GetUserDetails(userId);
                    if (userModel != null)
                    {
                        return View(userModel);
                    }
                    else
                    {
                        ViewData["Error"] = "No user data found.";
                    }
                }
                else
                {
                    ViewData["Error"] = "Invalid User ID.";
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "An error occurred while retrieving user details.";
            }

            return View(userModel);
        }

        // Action to update user account details
        public IActionResult UpdateUserAccount(UserModel user)
        {
            string userIdCookie = Request.Cookies["UserId"];

            //check to see if userModel object is passed
            if (user != null && ModelState.IsValid)
            {
                int result = accountDBModel.UpdateUser(user, userIdCookie);


                if (result == -1)
                {
                    ViewData["ValidationMessage"] = "Username already exists. Please choose another one.";
                }
                else
                {
                    ViewData["ValidationMessage"] = "Successfully updated User";
                }

            }

            return View("AccountDetail_View", user);
        }

        //shows the account view
        public IActionResult AccountUpdate()
        {
            return View();
        }
    }
}
