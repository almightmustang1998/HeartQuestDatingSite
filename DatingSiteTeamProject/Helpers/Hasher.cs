using System.Text;
using System.Security.Cryptography;

namespace DatingSiteTeamProject.Helpers
{
    //this class is used to hash a password
    public class Hasher
    {       //creates a hashed password based on the string provided
            public static string HashPassword(string password)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return Convert.ToBase64String(hashedBytes);
                }
            }
            //verifies that is a password is equal to its hashed version
            public static bool VerifyPassword(string enteredPassword, string storedHash)
            {
                string enteredPasswordHash = HashPassword(enteredPassword);
                return enteredPasswordHash == storedHash;
            }
    }
}
