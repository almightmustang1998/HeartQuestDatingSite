using DatingSiteTeamProject.Helpers;
using DatingSiteTeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Utilities;

namespace DatingSiteTeamProject.Controllers
{
    // This controller handles the verification code process in the dating site,
    // including verifying the code and logging the user in if the code is valid.
    public class VerifyCodeController : Controller
    {
       //action to verify that code sent to user email is correct
        public IActionResult VerifyCode(AuthenicationModel authentication)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            string userEmail = HttpContext.Request.Cookies["UserEmail"];
            ViewBag.UserEmail = userEmail;

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.CheckVerificationCode";


            SqlParameter paramEmail = new SqlParameter("@email", authentication.User.Email);
            SqlParameter paramFourDigitCode = new SqlParameter("@verificationCode", authentication.FourDigitCode);

            objCommand.Parameters.Add(paramEmail);
            objCommand.Parameters.Add(paramFourDigitCode);

            SqlParameter paramIsValid = new SqlParameter("@isValid", SqlDbType.Bit);
            paramIsValid.Direction = ParameterDirection.Output;
            objCommand.Parameters.Add(paramIsValid);

            objDB.GetDataSet(objCommand);

            bool isValid = (bool)paramIsValid.Value;

            objCommand.Parameters.Clear();

            //if email and verification code match, log user in
            if (isValid)
            {
                //get userId through stored procedure
                SqlParameter paramterID = new SqlParameter("@UserId", SqlDbType.Int);
                paramterID.Direction = ParameterDirection.Output;
                objCommand.Parameters.Add(paramterID);
                objCommand.Parameters.Add(paramEmail);

                objCommand.CommandText = "dbo.GetUserIdByEmail";

                objDB.GetDataSet(objCommand);

                int id = Convert.ToInt32(objCommand.Parameters["@UserId"].Value);

                return RedirectToAction("AccountDetail_View", "Account", new {id});
            }

            return View();
        }
    }
}
