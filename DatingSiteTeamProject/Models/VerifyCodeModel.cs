using System.ComponentModel.DataAnnotations;

namespace DatingSiteTeamProject.Models

{
    //used to verify that a user inserted correct four digit code - part of two step verification process
    public class VerifyCodeModel
    {
        private UserModel user;
        private ProfileModel profile;
        private string fourDigitCode;

        public VerifyCodeModel()
        {
            this.user = new UserModel();
            this.profile = new ProfileModel();
            this.fourDigitCode = "";
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

        [Required(ErrorMessage = "Four digit code is required.")]
        public string FourDigitCode
        {
            get { return this.fourDigitCode; }
            set { this.fourDigitCode = value; }
        }
    }
}
