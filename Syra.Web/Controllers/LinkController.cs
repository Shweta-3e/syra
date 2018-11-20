using Syra.Web.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Configuration;

namespace Syra.Web.Controllers
{

    //[EnableCors(origins: "http://whichbigdata.com", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [EnableCors(origins: "*", headers: "*", methods: "*")] // tune to your needs

    [Serializable]
    public class LinkController : ApiController
    {
        [HttpPost]
        public void SendLink(string Name,string IPAddress, string UniqueId,string Url)
        {

            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("storageconnectionstring"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(WebConfigurationManager.AppSettings["goalconversion_container"]);
                DateTime datetime = DateTime.Now;
                var date = datetime.ToString("dd-MM-yyyy");
                var time = datetime.ToString("HH:mm:ss");
                var blob_file_name = date + "" + ".csv";
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                string contents = UniqueId + " | " + Name + " | " + IPAddress + " | " + Url +" | "+ date +" | " + time + "\n";
                string oldcontent;
                bool blob_check = blockBlob.Exists();
                if (blob_check == false)
                {
                    oldcontent = null;
                }
                else
                {
                    using (StreamReader reader = new StreamReader(blockBlob.OpenRead()))
                    {
                        oldcontent = reader.ReadToEnd();
                    }
                }
                using (StreamWriter writer = new StreamWriter(blockBlob.OpenWrite()))
                {
                    writer.Write(oldcontent);
                    writer.Write(contents);
                }
            }
            catch (Exception e)
            {
                string error_log = e.Message;
                FileStream fs = File.Open(System.Web.HttpContext.Current.Server.MapPath(@"\api\LogDirectory\logfile_demo.csv"), FileMode.Append);
                Byte[] info = new System.Text.UTF8Encoding(true).GetBytes(error_log + "\n");
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
        }
    }
}