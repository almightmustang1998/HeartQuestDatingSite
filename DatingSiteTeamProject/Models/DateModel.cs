using System.Data.SqlClient;
using System.Data;
using Utilities;

namespace DatingSiteApi.Models
{
    // This model represents date-related functionality in the dating site.
    // It includes properties for the sender and receiver IDs, date response, day and month, time, description, and location.
    // It provides methods to handle requesting, modifying, responding to date requests, and retrieving lists of date requests.
    // Database operations are performed using stored procedures.

    public class DateModel
    {
        private DBConnect objDB;
        private int senderId;
        private int recieverId;
        private string response;
        private string dayAndMonthOfDate;
        private string timeOfDate;
        private string dateDescription;
        private string location;

        public DateModel()
        {
            objDB = new DBConnect();
            senderId = 0;
            recieverId = 0;
            response = string.Empty;
            dayAndMonthOfDate = string.Empty;
            timeOfDate = string.Empty;
            dateDescription = string.Empty;
            location = string.Empty;
        }

        public DateModel(int senderId, int recieverId)
        {
            objDB = new DBConnect();
            this.senderId = senderId;
            this.recieverId = recieverId;
            this.response = string.Empty;
            dayAndMonthOfDate = string.Empty;
            timeOfDate = string.Empty;
            dateDescription = string.Empty;
            location = string.Empty;
        }

        public DateModel(int senderId, int recieverId, string response)
        {
            objDB = new DBConnect();
            this.senderId = senderId;
            this.recieverId = recieverId;
            this.response = response;
            dayAndMonthOfDate = string.Empty;
            timeOfDate = string.Empty;
            dateDescription = string.Empty;
            location = string.Empty;
        }

        public DateModel(int senderId, int recieverId, string dayAndMonth, string timeOfDate, string dateDescription, string location)
        {
            objDB = new DBConnect();
            this.senderId = senderId;
            this.recieverId = recieverId;
            this.response = string.Empty;
            this.dayAndMonthOfDate = dayAndMonth;
            this.timeOfDate = timeOfDate;
            this.dateDescription = dateDescription;
            this.location = location;
        }


        public string DayAndMonthOfDate
        {
            get
            {
                return dayAndMonthOfDate;
            }
            set
            {
                dayAndMonthOfDate = value;
            }
        }

        public string TimeOfDate
        {
            get
            {
                return timeOfDate;
            }
            set
            {
                timeOfDate = value;
            }
        }

        public string DateDescription
        {
            get
            {
                return dateDescription;
            }
            set
            {
                dateDescription = value;
            }
        }

        public string Location
        {
            get
            {
                return dateDescription;
            }
            set
            {
                dateDescription = value;
            }
        }


        public string Response
        {
            get
            {
                return response;
            }
            set
            {
                response = value;
            }
        }

        public int SenderId
        {
            get
            {
                return senderId;
            }
            set
            {
                senderId = value;
            }
        }

        public int RecieverId
        {
            get
            {
                return recieverId;
            }
            set
            {
                recieverId = value;
            }
        }

        public int RequestDate()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.RequestDate";

            //get user info 
            SqlParameter parameterSender = new SqlParameter("@senderID", this.SenderId);
            SqlParameter parameterReciever = new SqlParameter("@recieverID", this.RecieverId);
            SqlParameter paramTimeStamp = new SqlParameter("@timeStamp", DateTime.Now.ToString());
            SqlParameter paramStatus = new SqlParameter("@stat", "Sent");

            objCommand.Parameters.Add(parameterSender);
            objCommand.Parameters.Add(parameterReciever);
            objCommand.Parameters.Add(paramTimeStamp);
            objCommand.Parameters.Add(paramStatus);

            return objDB.DoUpdate(objCommand);
        }

        public int ModifyDate()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.ModifyDate";

            SqlParameter parameterSender = new SqlParameter("@recieverId", this.SenderId);
            SqlParameter parameterReciever = new SqlParameter("@senderId", this.RecieverId);
            SqlParameter paramDate = new SqlParameter("@dayMonth", this.DayAndMonthOfDate);
            SqlParameter paramTime = new SqlParameter("@dateTime", this.TimeOfDate);
            SqlParameter paramDescription = new SqlParameter("@dateDesc", this.DateDescription);
            SqlParameter paramLocation = new SqlParameter("@placeofdate", this.Location);

            objCommand.Parameters.Add(parameterSender);
            objCommand.Parameters.Add(parameterReciever);
            objCommand.Parameters.Add(paramDate);
            objCommand.Parameters.Add(paramTime);
            objCommand.Parameters.Add(paramDescription);
            objCommand.Parameters.Add(paramLocation);

            return objDB.DoUpdate(objCommand);
        }

        public int RespondToDateRequest()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.RespondToDateRequest";

            //get user info 
            SqlParameter parameterSender = new SqlParameter("@senderId", this.SenderId);
            SqlParameter parameterReciever = new SqlParameter("@recieverId", this.RecieverId);
            SqlParameter paramStatus = new SqlParameter("@newStat", this.Response);
            SqlParameter paramTimeStamp = new SqlParameter("@timeStamp", DateTime.Now.ToString());

            objCommand.Parameters.Add(parameterSender);
            objCommand.Parameters.Add(parameterReciever);
            objCommand.Parameters.Add(paramStatus);
            objCommand.Parameters.Add(paramTimeStamp);

            return objDB.DoUpdate(objCommand);
        }

        public DataSet GetListOfIncomingDateRequests()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.ViewIncomingDateRequests";
            SqlParameter parameterUser = new SqlParameter("@currentUser", this.SenderId);
            objCommand.Parameters.Add(parameterUser);
            return objDB.GetDataSet(objCommand);
        }

        public DataSet GetListOfSentRequests()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.ViewSentDateRequests";
            SqlParameter parameterUser = new SqlParameter("@currentUser", this.SenderId);
            objCommand.Parameters.Add(parameterUser);
            return objDB.GetDataSet(objCommand);
        }

        public DataSet GetListOfAcceptedRequests()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "dbo.ViewAcceptedDateRequests";
            SqlParameter parameterUser = new SqlParameter("@currentUser", this.SenderId);
            objCommand.Parameters.Add(parameterUser);
            return objDB.GetDataSet(objCommand);
        }
    }
}
