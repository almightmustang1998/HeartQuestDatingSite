namespace DatingSiteTeamProject.Models
{
    //used for the date section of the tab where users can see the list of accepted dates,
    //list of incoming date requests, and list of sent date requests
    public class DateViewModel
    {
        public List<AuthenicationModel> ListOfAcceptedRequests { get; set; }

        public List<AuthenicationModel> ListOfIncomingDateRequests { get; set; }

        public List<AuthenicationModel> ListOfSentRequests { get; set; }


    }
}
