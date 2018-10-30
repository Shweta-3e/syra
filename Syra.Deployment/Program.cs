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
            CreateResource();
            Console.ReadKey();
        }

        public static void CreateResource()
        {

            string apiversion = ConfigurationSettings.AppSettings["apiversion"].ToString();
            string resourcename = "";
            Console.WriteLine("Enter your Resource Name : ");
            resourcename = Console.ReadLine();
            var Uri = ConfigurationSettings.AppSettings["staticUri"].ToString() + resourcename;
            var resourceUri = Uri + apiversion;
            string location = ConfigurationSettings.AppSettings["resourcelocation"].ToString();
            var message = JsonConvert.SerializeObject(new { Location = location });
            HttpWebRequest request = WebRequest.Create(resourceUri) as System.Net.HttpWebRequest;
            Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(message);
            request.KeepAlive = true;
            request.Method = "PUT";
            request.ContentLength = data.Length;
            request.ContentType = "application/json";
            string token = GetAccessToken();
            string token_auth = "Bearer " + token;
            request.Headers.Add("Authorization", token_auth);
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            String jsonresponse = "";
            String temp = null;
            while ((temp = reader.ReadLine()) != null)
            {
                jsonresponse += temp;
                Console.WriteLine("Resource is created successfully.\n");
            }
            StorageAccount(Uri, token_auth);
        }


        public static void StorageAccount(string Uri, string token)
        {
            Console.WriteLine("Enter your Resource Name : ");
            string resource = Console.ReadLine();
            Console.WriteLine("Enter your Storage Account Name : ");
            string storageaccount = Console.ReadLine();
            string resource_api = ConfigurationSettings.AppSettings["resource_api"].ToString();
            var storageUri = ConfigurationSettings.AppSettings["staticUri"].ToString() + resource + ConfigurationSettings.AppSettings["storageaccurl"].ToString() + storageaccount + resource_api;
            string resourceloc = ConfigurationSettings.AppSettings["storagelocation"].ToString();
            string storagekind = ConfigurationSettings.AppSettings["storagekind"].ToString();
            var bodystring = JsonConvert.SerializeObject(new { sku = new { name = "Standard_GRS" }, kind = "Storage", location = resourceloc });
            Encoding encoding = new UTF8Encoding();
            byte[] resourcedata = encoding.GetBytes(bodystring);
            HttpWebRequest resourcerequest = WebRequest.Create(storageUri) as System.Net.HttpWebRequest;
            resourcerequest.KeepAlive = true;
            resourcerequest.Method = "PUT";
            resourcerequest.ContentLength = resourcedata.Length;
            resourcerequest.ContentType = "application/json";
            resourcerequest.Headers.Add("Authorization", token);
            Stream writer = resourcerequest.GetRequestStream();
            writer.Write(resourcedata, 0, resourcedata.Length);
            writer.Close();
            HttpWebResponse resourceresponse = (HttpWebResponse)resourcerequest.GetResponse();
            var resourcereader = new StreamReader(resourceresponse.GetResponseStream());
            string temp = null;
            string jsonresponse = "";
            while ((temp = resourcereader.ReadLine()) != null)
            {
                jsonresponse += temp;
                Console.WriteLine("Storage is created successfully.\n");
            }
            CreateBlobContainer(resource_api, token);
        }

        public static void CreateBlobContainer(string resourceapi, string token)
        {
            try
            {
                Console.WriteLine("Enter your Resource Name : ");
                string resource = Console.ReadLine();
                Console.WriteLine("Enter your Storage Account Name : ");
                string storageaccount = Console.ReadLine();
                Console.WriteLine("Enter the container name : ");
                string container = Console.ReadLine();
                string containerurl = ConfigurationSettings.AppSettings["staticUri"].ToString() + resource + ConfigurationSettings.AppSettings["storageaccurl"].ToString() + storageaccount + ConfigurationSettings.AppSettings["containerurl"].ToString() + container + resourceapi;
                var reqbody = JsonConvert.SerializeObject(new { });
                Encoding encoding = new UTF8Encoding();
                byte[] resourcedata = encoding.GetBytes(reqbody);
                HttpWebRequest resourcerequest = WebRequest.Create(containerurl) as System.Net.HttpWebRequest;
                resourcerequest.KeepAlive = true;
                resourcerequest.Method = "PUT";
                resourcerequest.ContentLength = resourcedata.Length;
                resourcerequest.ContentType = "application/json";
                resourcerequest.Headers.Add("Authorization", token);
                Stream writer = resourcerequest.GetRequestStream();
                writer.Write(resourcedata, 0, resourcedata.Length);
                writer.Close();
                HttpWebResponse resourceresponse = (HttpWebResponse)resourcerequest.GetResponse();
                var resourcereader = new StreamReader(resourceresponse.GetResponseStream());
                string temp = null;
                string jsonresponse = "";
                while ((temp = resourcereader.ReadLine()) != null)
                {
                    jsonresponse += temp;
                    Console.WriteLine("Blob storage is created successfully.");
                }
            }
            catch (Exception e)
            {
                string message = "Storage name conflicts";
                Console.WriteLine(message);
                CreateBlobContainer(resourceapi, token);
            }
        }

        public static string GetAccessToken()
        {
            string tenantname = ConfigurationSettings.AppSettings["tenant"].ToString();
            string clientid = ConfigurationSettings.AppSettings["clientId"].ToString();
            string clientsecret = ConfigurationSettings.AppSettings["clientSecret"].ToString();
            var authString = ConfigurationSettings.AppSettings["authString"].ToString() + tenantname + "/oauth2/token";
            var resourceUrl = ConfigurationSettings.AppSettings["resourceUrl"].ToString();
            var authenticationContext = new AuthenticationContext(authString);
            var clientCred = new ClientCredential(clientid, clientsecret);
            var authenticationResult = authenticationContext.AcquireTokenAsync(resourceUrl, clientCred).Result;
            var token = authenticationResult.AccessToken;
            return token;
        }
    }
}
