using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Syra.Admin.SignalR
{
    public class SyraHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
        public void Send(string name, string message, string clientid)
        {
            // Call the broadcastMessage method to update clients.

            //Call LUIS API // GET ITS INTENT / DECIDE WHAT TO BE SENT OUT

            //https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/136e3019-5e1e-487e-a086-d1541f89c47b?subscription-key=e6d3d39c8364452baec720946d038b6f&timezoneOffset=-360&q=

            var Uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/136e3019-5e1e-487e-a086-d1541f89c47b?subscription-key=e6d3d39c8364452baec720946d038b6f&timezoneOffset=-360&q=" + message;
            HttpWebRequest request = WebRequest.Create(Uri) as System.Net.HttpWebRequest;
            Encoding encoding = new UTF8Encoding();
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            String jsonresponse = "";
            String temp = null;
            while (!reader.EndOfStream)
            {
                temp = reader.ReadLine();
                jsonresponse += temp;
            }
            //return jsonresponse;

            // Process LUIS Response 


            Clients.Caller.broadcastMessage(name, jsonresponse);


        }
    }
}