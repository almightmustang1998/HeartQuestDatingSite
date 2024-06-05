using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;

using DatingSiteTeamProject.Models;

    //this class is for http related code that is needed across multiple controllers for this dating site
    public class DatingHttp
    {
        public static void SendHttpRequest(string apiUrl, string method, string jsonContent)
        {
            WebRequest request = WebRequest.Create(apiUrl);
            request.Method = method;
            request.ContentType = "application/json";

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(jsonContent);
            }

            using (WebResponse response = request.GetResponse())
            using (Stream theDataStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(theDataStream))
            {
                reader.ReadToEnd();
            }
        }

        public static List<AuthenicationModel> GetProfilesFromApi(string apiUrl)
        {
            WebRequest request = WebRequest.Create(apiUrl);

            using (WebResponse response = request.GetResponse())
            using (Stream theDataStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(theDataStream))
            {
                string data = reader.ReadToEnd();
                return JsonSerializer.Deserialize<List<AuthenicationModel>>(data);
            }
        }
}
