using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Configuration;
using System.Threading.Tasks;
using System;
using System.Net;
using System.IO;
using System.Text;

namespace Syra.Development
{
    class Program
    {
        static void Main(string[] args)
        {

            string tenantname = ConfigurationSettings.AppSettings["tenant"].ToString();
            string clientid = ConfigurationSettings.AppSettings["clientId"].ToString();
            string clientsecret = ConfigurationSettings.AppSettings["clientSecret"].ToString();
            GetAccessToken(tenantname, clientid, clientsecret);
            //Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
        public static void GetAccessToken(string tenantName, string clientId, string clientSecret)
        {
            var authString = ConfigurationSettings.AppSettings["authString"].ToString() + tenantName + "/oauth2/token";
            var resourceUrl = ConfigurationSettings.AppSettings["resourceUrl"].ToString();
            var authenticationContext = new AuthenticationContext(authString);
            var clientCred = new ClientCredential(clientId, clientSecret);
            var authenticationResult = authenticationContext.AcquireTokenAsync(resourceUrl, clientCred).Result;
            var token = authenticationResult.AccessToken;
            string chatbotname = "";
            Console.WriteLine("Enter your Chatbot Name : \n");
            chatbotname = Console.ReadLine();
            var resourceUri = ConfigurationSettings.AppSettings["staticUri"].ToString() + chatbotname + ConfigurationSettings.AppSettings["apiversion"].ToString();
            HttpWebRequest request = WebRequest.Create(resourceUri) as System.Net.HttpWebRequest;
            string location = ConfigurationSettings.AppSettings["resourcelocation"].ToString();
            var message = JsonConvert.SerializeObject(new { Location = location });
            Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(message);
            request.KeepAlive = true;
            request.Method = "PUT";
            request.ContentLength = data.Length;
            request.ContentType = "application/json";
            string token_auth = "Bearer " + token;
            request.Headers.Add("Authorization", token_auth);
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            String jsonresponse = "";
            String temp = null;
            while ((temp = reader.ReadLine()) != null)
            {
                jsonresponse += temp;
                Console.WriteLine(jsonresponse);
            }
        }
    }
}
