using DatingSiteTeamProject.Helpers;
using DatingSiteTeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Utilities;
using Microsoft.AspNetCore.SignalR.Protocol;
using System;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace DatingSiteTeamProject.Controllers
{
    // This controller handles authentication-related operations in the dating site,
    // including displaying the authentication view and sending email verification codes.

    public class AuthenticationController : Controller
    {
        // Action to display the authentication view with a random security question
        public IActionResult Authentication_View()
        {
            AuthenicationModel authentication = new AuthenicationModel();
            string question = authentication.Profile.RandomQuestion;
            ViewBag.QuestionBeingDisplayed = question;
            return View(authentication);
        }

        // Action to send email verification code after validating user's email and security answer
        [HttpGet]
        public IActionResult SendEmailVerification(AuthenicationModel member)
        {
            AuthenicationModel authentication = new AuthenicationModel();
            string question = authentication.Profile.RandomQuestion;
            ViewBag.QuestionBeingDisplayed = question;

            DBConnect objDB = new DBConnect();

            ModelState.Remove("FourDigitCode");
            ModelState.Remove("ImageTitle");
            ModelState.Remove("ImageDescription");
            ModelState.Remove("Images");
            ModelState.Remove("ImageFile");
            ModelState.Remove("UploadStatus");
            ModelState.Remove("LastMeal");
            ModelState.Remove("DreamDestinations");
            ModelState.Remove("GoalsInFive");
            ModelState.Remove("DealBreakers");
            ModelState.Remove("Languages");
            ModelState.Remove("PhotoUrl");
            ModelState.Remove("Age");
            ModelState.Remove("Height");
            ModelState.Remove("Weight");
            ModelState.Remove("Description");
            ModelState.Remove("Quote");
            ModelState.Remove("City");
            ModelState.Remove("State");
            ModelState.Remove("Interests");
            ModelState.Remove("Occupation");
            ModelState.Remove("Answer1");
            ModelState.Remove("Answer2");
            ModelState.Remove("Answer3");


            if (member == null)
            {
                return View("Authentication_View", member);
            }

            //if (!ModelState.IsValid)
            //{
            //    return View("Authentication_View", member);
            //}
            if (member != null)
            {
                try
                {
                    SqlCommand objCommand = new SqlCommand();
                    objCommand.CommandType = CommandType.StoredProcedure;
                    objCommand.CommandText = "dbo.CheckUserAnswer";


                    SqlParameter paramEmail = new SqlParameter("@email", member.User.Email);
                    SqlParameter paramAnswer = new SqlParameter("@answer", member.Answer);

                    objCommand.Parameters.Add(paramEmail);
                    objCommand.Parameters.Add(paramAnswer);

                    SqlParameter paramterIsValid = new SqlParameter("@isValid", SqlDbType.Bit);
                    paramterIsValid.Direction = ParameterDirection.Output;
                    objCommand.Parameters.Add(paramterIsValid);

                    objDB.GetDataSet(objCommand);
                    bool isValid = (bool)paramterIsValid.Value;


                    if (objCommand.Parameters["@isValid"].Value != DBNull.Value)
                    {

                        objCommand.Parameters.Clear();

                        //if email and answer is correct
                        if (isValid)
                        {
                            //uncomment when publishing
                            Helpers.Email email = new Helpers.Email();
                            string code = RandomCode.VerificationCode();

                            string strMessage = "This is your verification code: " + code;
                            objCommand.CommandText = "dbo.InsertVerification";


                            SqlParameter paramUserId = new SqlParameter("@email", member.User.Email);
                            SqlParameter paramVerificationCode = new SqlParameter("@verificationCode", Convert.ToInt32(code));
                            SqlParameter paramDateTime = new SqlParameter("@dateTime", DateTime.Now.ToString());

                            objCommand.Parameters.Add(paramUserId);
                            objCommand.Parameters.Add(paramVerificationCode);
                            objCommand.Parameters.Add(paramDateTime);

                            objDB.DoUpdate(objCommand);

                            //uncomment when publishing
                            email.SendMail(member.User.Email, "LoveQuest@datingsite.com", "Verification Code", strMessage);

                            //store email in cookie
                            CookieOptions options = new CookieOptions();
                            options.Expires = DateTime.Now.AddMinutes(5); //will only save user email for 5 mins in cookies
                            options.HttpOnly = true;

                            Response.Cookies.Append("UserEmail", member.User.Email, options);

                            // send user to different view and check db if the random code matches this code.
                            return View("~/Views/Authentication/VerifyCode_View.cshtml", member);
                        }

                        //email or password were not correct
                        else
                        {
                            ViewData["ErrorMessage"] = "Incorrect email/security answer.";
                            return View("Authentication_View", member);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return View("Error"); 
                }
            }
                return View(member);
        }



        


    }
}
