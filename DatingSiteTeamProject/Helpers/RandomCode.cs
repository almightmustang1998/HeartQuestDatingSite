using System.Text;

namespace DatingSiteTeamProject.Helpers
{
    // generates a random four digit number in between 1000-9999
    public class RandomCode
    {
        public static string VerificationCode()
        {
            Random random = new Random();

            int random4DigitNumber = random.Next(1000, 9999);
           
            return random4DigitNumber.ToString();
        }
    }
}
