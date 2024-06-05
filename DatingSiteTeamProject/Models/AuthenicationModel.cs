using DatingSiteApi.Models;
using System.ComponentModel.DataAnnotations;

namespace DatingSiteApi.Models
{
    // This model represents authentication information in the dating site.
    // It includes properties for user details, profile details, security answer, a four-digit code, and an image.
    // Validation attributes are used to ensure data integrity and correctness.

    public class AuthenicationModel
    {
        private UserModel user;
        private ProfileModel profile;
        private string answer;
        private string fourDigitCode;
        private ImageInfo image;

        public AuthenicationModel()
        {
            this.user = new UserModel();
            this.profile = new ProfileModel();
            this.fourDigitCode = "";
        }

        public ImageInfo Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        public UserModel User
        {
            get { return this.user; }
            set { this.user = value; }
        }

        public ProfileModel Profile
        {
            get { return this.profile; }
            set { this.profile = value; }
        }

        [Required(ErrorMessage = "Answer is required.")]
        [StringLength(50, ErrorMessage = "Answer cannot be longer than 50 characters.")]
        public string Answer
        {
            get { return this.answer; }
            set { this.answer = value; }
        }

        [Required(ErrorMessage = "Digital code is required.")]
        public string FourDigitCode
        {
            get { return this.fourDigitCode; }
            set { this.fourDigitCode = value; }
        }
    }
}
