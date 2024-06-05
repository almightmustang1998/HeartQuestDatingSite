using System.Diagnostics;

namespace DatingSiteTeamProject.Helpers
{
    //used for two step verification just in case user forgets their password
    public class Questions
    {
        private string question1 = "What is the name of your first pet?";
        private string question2 = "In which city were you born?";
        private string question3 = "What is the model of your first car?";
        private string answer1 = "";
        private string answer2 = "";
        private string answer3 = "";

           
        public Questions(string answer1, string answer2, string answer3)
        {
            this.answer1 = answer1;
            this.answer2 = answer2;
            this.answer3 = answer3;
        }

        public Questions()
        {
            this.answer1 = string.Empty;
            this.answer2 = string.Empty;
            this.answer3 = string.Empty;    
        }

        public string Answer1
        {
            get { return answer1; }
            set { answer1 = value; }
        }
      
        public string Answer2
        {
            get { return answer2; }
            set { answer2 = value; }
        }

        public string Answer3
        {
            get { return answer3; }
            set { answer3 = value; }
        }

        public string Question1
        {
            get { return question1; }
        }

        public string Question2
        {
            get { return question2; }
        }

        public string Question3
        {
            get { return question3; }
        }

        public string RandomQuestion()
        {
            List<String> randomQuestion = new List<String>();
            randomQuestion.Add(question1);
            randomQuestion.Add(question2);
            randomQuestion.Add(question3);

            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            return randomQuestion[randomNumber];

        }
    }
}
