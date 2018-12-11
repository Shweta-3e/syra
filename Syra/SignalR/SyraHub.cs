using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Syra.Admin.DbContexts;
using Syra.Admin.Entities;

namespace Syra.Admin.SignalR
{
    public class SyraHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
        //[HttpPost]
        //public string Send(string message, string clientid)
        //{
        //    // Call the broadcastMessage method to update clients.

        //    //Call LUIS API // GET ITS INTENT / DECIDE WHAT TO BE SENT OUT
        //    var detailsUri = "http://147.75.71.162:8585/customer/getcustomerdetails?clientid=" + clientid;
        //    HttpWebRequest detailsrequest = WebRequest.Create(detailsUri) as System.Net.HttpWebRequest;
        //    detailsrequest.Method = "GET";
        //    HttpWebResponse detailsresponse = (HttpWebResponse)detailsrequest.GetResponse();
        //    var detailsreader = new StreamReader(detailsresponse.GetResponseStream()).ReadToEnd();
        //    LuisDomain domain=JsonConvert.DeserializeObject<LuisDomain>(detailsreader);
        //    //https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/136e3019-5e1e-487e-a086-d1541f89c47b?subscription-key=e6d3d39c8364452baec720946d038b6f&timezoneOffset=-360&q=

        //    var Uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/" + domain.LuisAppId + "?subscription-key=" + domain.LuisAppKey + "&q=" + message;
        //    HttpWebRequest request = WebRequest.Create(Uri) as System.Net.HttpWebRequest;
        //    Encoding encoding = new UTF8Encoding();
        //    request.Method = "GET";
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    var reader = new StreamReader(response.GetResponseStream());
        //    String jsonresponse = "";
        //    String temp = null;
        //    while (!reader.EndOfStream)
        //    {
        //        temp = reader.ReadLine();
        //        jsonresponse += temp;
        //    }
        //    LuisReply Data = JsonConvert.DeserializeObject<LuisReply>(jsonresponse);
        //    List<string> LUISresponse=FetchFromDB(Data.topScoringIntent.intent,Data.entities[0].entity);
        //    return jsonresponse;
        //    //Clients.Caller.broadcastMessage(name, LUISresponse);


        //}

        public List<string> FetchFromDB(string Intent, string Entity)
        {
            List<string> data = new List<string>();
            string cs = ConfigurationManager.ConnectionStrings["connstring"].ToString();
            string sql = "Select * from dbo.BotContent where Intent='" + Intent + "' and Entity='" + Entity + "'";
            SqlConnection connection = new SqlConnection(cs);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {

                    data.Add(dataReader["BotReply"].ToString());
                }
                dataReader.Close();
                command.Dispose();
                connection.Close();
                return data;
            }
            catch (Exception)
            {
                Console.WriteLine("Can not open connection ! ");
                data.Add("Something Went Wrong!");
                return data;
            }
        }
    }
}