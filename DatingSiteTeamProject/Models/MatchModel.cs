using System.Data.SqlClient;
using System.Data;
using Utilities;

namespace DatingSiteApi.Models
{
    // This model represents the matching functionality in the dating site application.
    // It includes properties for the matcher and matchee IDs and provides methods to handle user matching,
    // viewing matches, and unmatching users. Database operations are performed using stored procedures.

    public class MatchModel
    {
        private DBConnect objDB;
        private int matcherId;
        private int matcheeId;

        public MatchModel()
        {
            objDB = new DBConnect();
            matcherId = 0;
            matcheeId = 0;
        }

        public MatchModel(int matcherId, int matcheeId)
        {
            objDB = new DBConnect();
            this.matcherId = matcherId;
            this.matcheeId = matcheeId;
        }

        public int MatcherId
        {
            get
            {
                return matcherId;
            }
            set
            {
                matcherId = value;
            }
        }

        public int MatcheeId
        {
            get
            {
                return matcheeId;
            }
            set
            {
                matcheeId = value;
            }
        }

        public void MatchUser()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.MatchUser";

            SqlParameter parameterMatcher = new SqlParameter("@matcher", this.matcherId);
            SqlParameter parameterMatchee = new SqlParameter("@matchee", this.matcheeId);
            SqlParameter paramTimeStamp = new SqlParameter("@timeStamp", DateTime.Now.ToString());

            objCommand.Parameters.Add(parameterMatcher);
            objCommand.Parameters.Add(parameterMatchee);
            objCommand.Parameters.Add(paramTimeStamp);

            objDB.DoUpdate(objCommand);
        }

        public DataSet ViewMatches()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.ViewMatches";
            SqlParameter parameterCurrentUser = new SqlParameter("@currentUser", this.matcherId);
            objCommand.Parameters.Add(parameterCurrentUser);
            return objDB.GetDataSet(objCommand);
        }

        public void Unmatch()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.UnMatch";

            SqlParameter parameterUnmatcher = new SqlParameter("@Unmatcher", this.matcherId);
            SqlParameter parameterUnmatchedProfile = new SqlParameter("@PersonBeingUnmatched", this.matcheeId);

            objCommand.Parameters.Add(parameterUnmatcher);
            objCommand.Parameters.Add(parameterUnmatchedProfile);


            objDB.DoUpdate(objCommand);
        }
    }
}
