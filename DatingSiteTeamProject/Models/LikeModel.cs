using System.Data.SqlClient;
using System.Data;
using Utilities;

namespace DatingSiteApi.Models
{
    // This model represents the like functionality in the dating site application.
    // It includes properties for the liker and likee IDs and provides methods to handle liking a user,
    // viewing users who liked the current user, viewing users liked by the current user, and unliking a user.
    // Database operations are performed using stored procedures.

    public class LikeModel
    {
        private DBConnect objDB;
        private int likerId;
        private int likeeId;

        public LikeModel()
        {
            objDB = new DBConnect();
            likerId = 0;
            likeeId = 0;
        }

        public LikeModel(int likerId, int likeeId)
        {
            objDB = new DBConnect();
            this.likerId = likerId;
            this.likeeId = likeeId;
        }

        public int LikerId
        {
            get
            {
                return likerId;
            }
            set
            {
                likerId = value;
            }
        }

        public int LikeeId
        {
            get
            {
                return likeeId;
            }
            set
            {
                likeeId = value;
            }
        }

        public int LikeUser()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.LikeUser";

            SqlParameter parameterLiker = new SqlParameter("@liker", this.likerId);
            SqlParameter parameterLikee = new SqlParameter("@likee", this.likeeId);
            SqlParameter paramTimeStamp = new SqlParameter("@timeStamp", DateTime.Now.ToString());

            objCommand.Parameters.Add(parameterLiker);
            objCommand.Parameters.Add(parameterLikee);
            objCommand.Parameters.Add(paramTimeStamp);

            return objDB.DoUpdate(objCommand);
        }

        public DataSet GetLikedYou()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.ViewLikes";
            SqlParameter currentUser = new SqlParameter("@currentUser", this.likerId);
            objCommand.Parameters.Add(currentUser);
            return objDB.GetDataSet(objCommand);
        }

        public DataSet GetLikedByUser()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.ViewLiked";
            SqlParameter parameterUserBeingViewed = new SqlParameter("@currentUser", this.likerId);
            objCommand.Parameters.Add(parameterUserBeingViewed);
            return objDB.GetDataSet(objCommand);
        }

        public int Unlike()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.Unlike";

            SqlParameter parameterDisliker = new SqlParameter("@Disliker", this.likerId);
            SqlParameter parameterPersonDisliked = new SqlParameter("@PersonBeingDisliked", this.likeeId);

            objCommand.Parameters.Add(parameterDisliker);
            objCommand.Parameters.Add(parameterPersonDisliked);

            return objDB.DoUpdate(objCommand);
        }
    }
}
