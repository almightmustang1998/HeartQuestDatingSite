namespace DatingSiteTeamProject.Models
{
    //used in the like section of the website to show profiles that current user liked and profiles that liked the current user
    public class LikesViewModel
    {
        public List<AuthenicationModel> ProfilesLikedByUser { get; set; }
        public List<AuthenicationModel> ProfilesThatLikeUser { get; set; }
    }

}
