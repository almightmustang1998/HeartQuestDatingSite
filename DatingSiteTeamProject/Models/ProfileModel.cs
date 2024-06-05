using DatingSiteApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DatingSiteApi.Models
{
    // This model represents a user profile in the dating site application.
    // It includes various properties related to the user's profile information,
    // such as personal details, interests, occupation, security questions, and images.
    // Validation attributes are used to ensure data integrity and correctness.

    public class ProfileModel
        {
            private string photoUrl;
            private string age;
            private string height;
            private string weight;
            private string description;
            private string quote;
            private string movie;
            private string book;
            private string city;
            private string state;
            private List<string> interests;
            private string otherInterests;
            private string occupation;
            private string food;
            private string commitment;
            private bool status;
            private int userId;
            private Questions securityQuestions;
            private ImageInfo imageInfo;
            private string lastMeal;
            private string dreamDestinations;
            private string goalsInFive;
            private string dealBreakers;
            private string languages;
            private string uploadStatus;
            private IFormFile imageFile;
            private List<ImageInfo> images;
            public ProfileModel()
            {
                this.photoUrl = string.Empty;
                this.age = string.Empty;
                this.height = string.Empty;
                this.weight = string.Empty;
                this.description = string.Empty;
                this.quote = string.Empty;
                this.movie = string.Empty;
                this.book = string.Empty;
                this.city = string.Empty;
                this.state = string.Empty;
                this.interests = new List<string>();
                this.otherInterests = string.Empty;
                this.occupation = string.Empty;
                this.food = string.Empty;
                this.commitment = string.Empty;
                this.status = false;
                this.userId = 0;
                this.securityQuestions = new Questions();
                this.imageInfo = new ImageInfo();
                this.lastMeal = string.Empty;
                this.dreamDestinations = string.Empty;
                this.goalsInFive = string.Empty;
                this.dealBreakers = string.Empty;
                this.languages = string.Empty;
                this.uploadStatus = string.Empty;
                this.imageFile = null;
                this.images = new List<ImageInfo>();
            }

            public ProfileModel(string photoUrl, string age, string height, string weight,
                string description, string quote, string movie, string book, string city, string state,
                List<string> interests, string otherInterests, string occupation, string food, string commitment, bool status, int userId, Questions securityQuestions, ImageInfo imageInfo,
                string lastMeal, string dreamDestinations, string goalsInFive, string dealBreakers, string languages, string uploadStatus, IFormFile imageFile, List<ImageInfo> images)
            {
                this.photoUrl = photoUrl;
                this.age = age;
                this.height = height;
                this.weight = weight;
                this.description = description;
                this.quote = quote;
                this.movie = movie;
                this.book = book;
                this.city = city;
                this.state = state;
                this.interests = interests;
                this.otherInterests = otherInterests;
                this.occupation = occupation;
                this.food = food;
                this.commitment = commitment;
                this.status = status;
                this.userId = userId;
                this.securityQuestions = securityQuestions;
                this.imageInfo = imageInfo;
                this.lastMeal = lastMeal;
                this.dreamDestinations = dreamDestinations;
                this.goalsInFive = goalsInFive;
                this.dealBreakers = dealBreakers;
                this.languages = languages;
                this.uploadStatus = uploadStatus;
                this.imageFile = imageFile;
                this.images = images;

            }

            [Required(ErrorMessage = "Image title is required.")]
            public string ImageTitle
            {
                get { return this.imageInfo.ImageTitle; }
                set { this.imageInfo.ImageTitle = value; }
            }

            [Required(ErrorMessage = "Image Description is required.")]
            public string ImageDescription
            {
                get { return this.imageInfo.ImageDescription; }
                set { this.imageInfo.ImageDescription = value; }
            }
            public List<ImageInfo> Images
            {
                get { return this.images; }
                set { this.images = value; }
            }

            public IFormFile ImageFile
            {
                get { return this.imageFile; }
                set { this.imageFile = value; }
            }
            public string UploadStatus
            {
                get { return this.uploadStatus; }
                set { this.uploadStatus = value; }
            }

            [Required(ErrorMessage = "Last Meal is required.")]
            [StringLength(50, ErrorMessage = "LastMeal cannot be longer than 50 characters.")]
            public string LastMeal
            {
                get { return this.lastMeal; }
                set { this.lastMeal = value; }
            }

            [Required(ErrorMessage = "Place(s) is required.")]
            public string DreamDestinations
            {
                get { return this.dreamDestinations; }
                set { this.dreamDestinations = value; }
            }

            [Required(ErrorMessage = "Goal(s) is required.")]
            public string GoalsInFive
            {
                get { return this.goalsInFive; }
                set { this.goalsInFive = value; }
            }

            [Required(ErrorMessage = "DealBreakers(s) is required.")]
            public string DealBreakers
            {
                get { return this.dealBreakers; }
                set { this.dealBreakers = value; }
            }

            [Required(ErrorMessage = "Language(s) is required.")]
            public string Languages
            {
                get { return this.languages; }
                set { this.languages = value; }
            }

            [Required(ErrorMessage = "PhotoURL is required.")]
            public string PhotoUrl
            {
                get { return photoUrl; }
                set { photoUrl = value; }
            }

            [Required(ErrorMessage = "Age is required.")]
            [RegularExpression(@"^[0-9\s]+$", ErrorMessage = "Age should only contain numbers!")]
            public string Age
            {
                get { return age; }
                set { age = value; }
            }

            [Required(ErrorMessage = "Height is required.")]
            [RegularExpression(@"^[0-9]+([.][0-9]{1,2})?$", ErrorMessage = "Height should only contain numbers and a decimal/apostrophe(two digits afterwards).")]
            public string Height
            {
                get { return height; }
                set { height = value; }
            }

            [Required(ErrorMessage = "Weight is required.")]
            [RegularExpression(@"^[0-9\s]+$", ErrorMessage = "Weight should only contain numbers!")]
            public string Weight
            {
                get { return weight; }
                set { weight = value; }
            }

            [Required(ErrorMessage = "Description is required.")]
            public string Description
            {
                get { return description; }
                set { description = value; }
            }

            [Required(ErrorMessage = "Quote is required.")]
            public string Quote
            {
                get { return quote; }
                set { quote = value; }
            }
            public string Movie
            {
                get { return movie; }
                set { movie = value; }
            }
            public string Book
            {
                get { return book; }
                set { book = value; }
            }

            [Required(ErrorMessage = "City is required.")]
            [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City should only contain letters!")]
            [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters.")]
            public string City
            {
                get { return city; }
                set { city = value; }
            }

            [Required(ErrorMessage = "State is required.")]
            [RegularExpression(@"^[A-Z\s]+$", ErrorMessage = "State should only contain capital letters!")]
            [StringLength(2, ErrorMessage = "State should only be abbreviation.")]
            public string State
            {
                get { return state; }
                set { state = value; }
            }

            //[Required(ErrorMessage = "Interests is required.")]
            public List<string> Interests
            {
                get { return interests; }
                set { interests = value; }
            }

            public string OtherInterests
            {
                get { return otherInterests; }
                set { otherInterests = value; }
            }

            [Required(ErrorMessage = "Occupation is required.")]
            [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Occupation should only contain letters!")]
            [StringLength(50, ErrorMessage = "Occupation cannot be longer than 50 characters.")]
            public string Occupation
            {
                get { return occupation; }
                set { occupation = value; }
            }
            public string Food
            {
                get { return food; }
                set { food = value; }
            }
            public string Commitment
            {
                get { return commitment; }
                set { commitment = value; }
            }
            public bool Status
            {
                get { return status; }
                set { status = value; }
            }
            public int UserId
            {
                get { return userId; }
                set { userId = value; }
            }

            //we will use this to display the question
            public string RandomQuestion
            {
                get { return securityQuestions.RandomQuestion(); }
            }

            [Required(ErrorMessage = "Answer is required.")]
            [StringLength(50, ErrorMessage = "Answer cannot be longer than 50 characters.")]
            public string Answer1
            {
                get { return securityQuestions.Answer1; }
                set { securityQuestions.Answer1 = value; }
            }

            [Required(ErrorMessage = "Answer is required.")]
            [StringLength(50, ErrorMessage = "Answer cannot be longer than 50 characters.")]
            public string Answer2
            {
                get { return securityQuestions.Answer2; }
                set { securityQuestions.Answer2 = value; }
            }

            [Required(ErrorMessage = "Answer is required.")]
            [StringLength(50, ErrorMessage = "Answer cannot be longer than 50 characters.")]
            public string Answer3
            {
                get { return securityQuestions.Answer3; }
                set { securityQuestions.Answer3 = value; }
            }

            public string Question1
            {
                get { return securityQuestions.Question1; }
                set { securityQuestions.Answer1 = value; }
            }
            public string Question2
            {
                get { return securityQuestions.Question2; }
                set { securityQuestions.Answer2 = value; }
            }
            public string Question3
            {
                get { return securityQuestions.Question3; }
                set { securityQuestions.Answer3 = value; }
            }

            private List<ImageInfo> LoadImages(DataTable imageData)
            {
                List<ImageInfo> images = new List<ImageInfo>();
                foreach (DataRow row in imageData.Rows)
                {
                    images.Add(new ImageInfo
                    {
                        Data = (byte[])row["ImageData"],
                        ImageTitle = row["ImageTitle"].ToString(),
                        ImageDescription = row["ImageDescription"].ToString(),
                        Type = row["ImageType"].ToString()
                    });
                }
                return images;
            }
        }
}
