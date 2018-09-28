using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Luis.Models;
using System.Linq;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Collections.Generic;
using MikeHabibChatBot.Controllers;
using System.Configuration;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Web.Configuration;

namespace MikeHabibChatBot.Dialogs
{
    [LuisModel("99576d42-c581-42a7-9c70-b1697e16e2d1", "f06c44f9c07d48858cf5327c8bf8b859")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        public int hellocount = 0;
        public int foulcount = 0;
        public int thankcount = 0;
        public int contactcount = 0;
        public bool fetchloc = true;
        public string uniqueidc;
       
        public Task StartAsync(IDialogContext context)
        {
            if (fetchloc == true)
            {
                FetchLocation(context);
                fetchloc = false;
            }
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }
      
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            string username;
            var activity = await result as Activity;
            //activity = context.Activity;
            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;
            if(activity.From.Name!=null)
            {
                username = activity.From.Name;
                Console.WriteLine("Username: " + username);
            }
            // return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            string text = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

            await context.PostAsync(text);
            if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, text); } else { await keeplog(message.Text, text); }

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Help For Tax")]
        public async Task helptax(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (fetchloc == true)
            {
                FetchLocation(context);
                fetchloc = false;
            }
            var message = await activity;
            var BDLUIS = await GetEntityFromLUIS(message.Text);

            if (BDLUIS.intents.Count() > 0)
            {
                if (BDLUIS.entities.Count() > 0)
                {
                    for (int i = 0; i < BDLUIS.entities.Count(); i++)
                    {
                        switch (BDLUIS.entities[i].type)
                        {  
                            case "Back Tax Help / Services":
                                foreach (string msg in FetchFromDB("Help For Tax", "Back Tax Help / Services"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Relief Service":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Relief Service"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Professional Tax Preparation Service":
                                foreach (string msg in FetchFromDB("Help For Tax", "Professional Tax Preparation Service"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "EDD Tax Audits":
                                foreach (string msg in FetchFromDB("Help For Tax", "EDD Tax Audits"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "IRS Tax Help":
                                foreach (string msg in FetchFromDB("Help For Tax", "IRS Tax Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "IRS Tax Help::Back Tax Payment Services":
                                foreach (string msg in FetchFromDB("Help For Tax", "IRS Tax Help::Back Tax Payment Services"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "IRS Tax Help::IRS 433 Form Help":
                                foreach (string msg in FetchFromDB("Help For Tax", "IRS Tax Help::IRS 433 Form Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "IRS Tax Help::IRS Bankruptcy":
                                foreach (string msg in FetchFromDB("Help For Tax", "IRS Tax Help::IRS Bankruptcy"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "IRS Tax Help::IRS Help":
                                foreach (string msg in FetchFromDB("Help For Tax", "IRS Tax Help::IRS Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "IRS Tax Help::IRS Officer":
                                foreach (string msg in FetchFromDB("Help For Tax", "IRS Tax Help::IRS Officer"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "IRS Tax Help::IRS Tax Audit Statistics & Overview":
                                foreach (string msg in FetchFromDB("Help For Tax", "IRS Tax Help::IRS Tax Audit Statistics & Overview"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "IRS Tax Help::IRS Tax Audit Services":
                                foreach (string msg in FetchFromDB("Help For Tax", "IRS Tax Help::IRS Tax Audit Services"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "IRS Tax Help::US Expat Tax Service":
                                foreach (string msg in FetchFromDB("Help For Tax", "IRS Tax Help::US Expat Tax Service"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Debt Relief Services":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Debt Relief Services"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Debt Relief Services::Collection Due Process Appeals":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Debt Relief Services::Collection Due Process Appeals"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Debt Relief Services::Offer in Compromise (OIC) Overview":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Debt Relief Services::Offer in Compromise (OIC) Overview"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Debt Relief Services::IRS Offer in Compromise Services":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Debt Relief Services::IRS Offer in Compromise Services"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Debt Relief Services::IRS Tax Forgiveness Program":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Debt Relief Services::IRS Tax Forgiveness Program"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Debt Relief Services::Penalty Abatement":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Debt Relief Services::Penalty Abatement"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Debt Relief Services::Wage Levy/Garnishment/Attachment Information":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Debt Relief Services::Wage Levy/Garnishment/Attachment Information"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Debt Relief Services::IRS Wage Garnishment Legal Help":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Debt Relief Services::IRS Wage Garnishment Legal Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Debt Relief Services::Tax Debt":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Debt Relief Services::Tax Debt"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Debt Relief Services::Tax Resolution Services":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Debt Relief Services::Tax Resolution Services"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Preparation::Income Tax Preparation Done Right":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Preparation::Income Tax Preparation Done Right"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Preparation::941 Payroll Problems":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Preparation::941 Payroll Problems"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Preparation::Payroll Tax Problems":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Preparation::Payroll Tax Problems"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Preparation::Tax Planning Services":
                                foreach (string msg in FetchFromDB("Help For Tax", "Tax Preparation::Tax Planning Services"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Virtual Tax::IRS Tax Debt Relief":
                                foreach (string msg in FetchFromDB("Help For Tax", "Virtual Tax::IRS Tax Debt Relief"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Articles::Tax Audit":
                                foreach (string msg in FetchFromDB("Help For Tax", "Articles::Tax Audit"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;

                            default:
                                if (contactcount<3)
                                {
                                    await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                                    if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                                    contactcount++;
                                }
                                else if (contactcount % 3 == 0)
                                {
                                    foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                                    {
                                        await context.PostAsync(msg);
                                        if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                    }
                                }

                                break;
                        }
                    }
                }
                else
                {
                    if (contactcount<3)
                    {
                        await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                        if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                        contactcount++;
                    }
                    else if (contactcount % 3 == 0)
                    {
                        foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                        {
                            await context.PostAsync(msg);
                            if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                        }
                    }
                }

            }
        }

        private static async Task<LuisReply> GetEntityFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            LuisReply Data = new LuisReply();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "https://api.projectoxford.ai/luis/v1/application?id=99576d42-c581-42a7-9c70-b1697e16e2d1&subscription-key=f06c44f9c07d48858cf5327c8bf8b859&q=" + Query;
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject<LuisReply>(JsonDataResponse);

                }
            }
            return Data;
        }

        public async Task keeplog(string question, string answer)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("storageconnectionstring"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(WebConfigurationManager.AppSettings["clientlog_container"]);
                string ipaddr = State.ip;
                string reg = State.region;
                string session_id = State.uuid;
                DateTime datetime = DateTime.Now;
                var date = datetime.ToString("dd-MM-yyyy");
                var time = datetime.ToString("HH:mm:ss");
                var blob_file_name = date + "" + ".csv";
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                string contents = session_id + " | " + reg + " | " + ipaddr + " | " + question + " | " + answer + " | " + date + " | " + time + "\n";
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
                FileStream fs_log = File.Open(System.Web.HttpContext.Current.Server.MapPath(@"\api\LogDirectory\logfile_demo.csv"), FileMode.Append);
                Byte[] info_log = new System.Text.UTF8Encoding(true).GetBytes(error_log + "\n");
                fs_log.Write(info_log, 0, info_log.Length);
                fs_log.Close();
            }
        }
        public async Task keeplog_demo(string question, string answer)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("storageconnectionstring"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(WebConfigurationManager.AppSettings["demolog_container"]);
                string response_demo = Demo.response;
                string ipaddr = State.ip;
                string reg = State.region;
                string session_id = State.uuid;
                DateTime datetime = DateTime.Now;
                var date = datetime.ToString("dd-MM-yyyy");
                var time = datetime.ToString("HH:mm:ss");
                var blob_file_name = date + "" + ".csv";
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                string contents = response_demo + " | " + session_id + " | " + reg + " | " + ipaddr + " | " + question + " | " + answer + " | " + date + " | " + time + "\n";
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
                FileStream fs_log = File.Open(System.Web.HttpContext.Current.Server.MapPath(@"\api\LogDirectory\logfile_demo.csv"), FileMode.Append);
                Byte[] info_log = new System.Text.UTF8Encoding(true).GetBytes(error_log + "\n");
                fs_log.Write(info_log, 0, info_log.Length);
                fs_log.Close();
            }
        }
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

        
        public  void FetchLocation(IDialogContext context)
        {
            string reg = State.region;
 
        }

        [LuisIntent("Help For Audit")]
        public async Task helpaudit(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (fetchloc == true)
            {
                FetchLocation(context);
                fetchloc = false;
            }
            var message = await activity;
            var BDLUIS = await GetEntityFromLUIS(message.Text);

            if (BDLUIS.intents.Count() > 0)
            {
                if (BDLUIS.entities.Count() > 0)
                {
                    for (int i = 0; i < BDLUIS.entities.Count(); i++)
                    {
                        switch (BDLUIS.entities[i].type)
                        {
                            case "IRS Audit Help":
                                foreach (string msg in FetchFromDB("Help For Audit", "IRS Audit Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Audit Help::Aerospace Audit Help":
                                foreach (string msg in FetchFromDB("Help For Audit", "Audit Help::Aerospace Audit Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Audit Help::Landscape Architects IRS Tax Audit Help":
                                foreach (string msg in FetchFromDB("Help For Audit", "Audit Help::Landscape Architects IRS Tax Audit Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                                
                            case "Audit Help::Art Galleries IRS Audit Tax Help":
                                foreach (string msg in FetchFromDB("Help For Audit", "Audit Help::Art Galleries IRS Audit Tax Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Audit Help::Construction IRS Audit Help":
                                foreach (string msg in FetchFromDB("Help For Audit", "Audit Help::Construction IRS Audit Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Audit Help::Childcare Business Owners IRS Audit Help":
                                foreach (string msg in FetchFromDB("Help For Audit", "Audit Help::Childcare Business Owners IRS Audit Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Audit Help::Entertainment Industry IRS Tax Audit Help":
                                foreach (string msg in FetchFromDB("Help For Audit", "Audit Help::Entertainment Industry IRS Tax Audit Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Audit Help::Tax Audit Defense":
                                foreach (string msg in FetchFromDB("Help For Audit", "Audit Help::Tax Audit Defense"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Audit Help::Boe Audit":
                                foreach (string msg in FetchFromDB("Help For Audit", "Audit Help::Boe Audit"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Audit Help::Debt settlement":
                                foreach (string msg in FetchFromDB("Help For Audit", "Audit Help::Debt settlement"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "AdditionalEntity::FTB":
                                foreach (string msg in FetchFromDB("Help For Audit", "AdditionalEntity::FTB"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Articles::Tax Audit":
                                foreach (string msg in FetchFromDB("Help For Audit", "Articles::Tax Audit"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;

                            default:
                                if (contactcount<3)
                                {
                                    await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                                    if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                                    contactcount++;
                                }
                                else if (contactcount % 3 == 0)
                                {
                                    foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                                    {
                                        await context.PostAsync(msg);
                                        if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                    }
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if (contactcount<3)
                    {
                        await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                        if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                        contactcount++;
                    }
                    else if (contactcount % 3 == 0)
                    {
                        foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                        {
                            await context.PostAsync(msg);
                            if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                        }
                    }
                }

            }
        }

        [LuisIntent("Search Resources")]
        public async Task readarticles(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (fetchloc == true)
            {
                FetchLocation(context);
                fetchloc = false;
            }
            var message = await activity;
            var BDLUIS = await GetEntityFromLUIS(message.Text);

            if (BDLUIS.intents.Count() > 0)
            {
                if (BDLUIS.entities.Count() > 0)
                {
                    for (int i = 0; i < BDLUIS.entities.Count(); i++)
                    {
                        switch (BDLUIS.entities[i].type)
                        {
                            case "Resources":
                                foreach (string msg in FetchFromDB("Search Resources", "Resources"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Articles":
                                foreach (string msg in FetchFromDB("Search Resources", "Articles"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Articles::Tax Audit":
                                foreach (string msg in FetchFromDB("Search Resources", "taxauditrepresentation"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "FAQs":
                                foreach (string msg in FetchFromDB("Search Resources", "FAQs"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Articles::Tax Settlement":
                                foreach (string msg in FetchFromDB("Search Resources", "Articles::Tax Settlement"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Articles::Installment Agreement":
                                foreach (string msg in FetchFromDB("Search Resources", "Articles::Installment Agreement"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Articles:: Tax Attorney":
                                foreach (string msg in FetchFromDB("Search Resources", "Articles:: Tax Attorney"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Articles::IRS Penalty Abatement Letter":
                                foreach (string msg in FetchFromDB("Search Resources", "Articles::IRS Penalty Abatement Letter"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Articles::Back Taxes Problems":
                                foreach (string msg in FetchFromDB("Search Resources", "Articles::Back Taxes Problems"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Articles::Back Taxes Payment Alternatives":
                                foreach (string msg in FetchFromDB("Search Resources", "Articles::Back Taxes Payment Alternatives"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Articles::941 Tax Problems & Help":
                                foreach (string msg in FetchFromDB("Search Resources", "Articles::941 Tax Problems & Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;

                            case "Articles::Back Taxes":
                                foreach (string msg in FetchFromDB("Search Resources", "Articles::Back Taxes"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Cost":
                                foreach (string msg in FetchFromDB("Search Resources", "Cost"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Virtual Tax::Crypto Tax":
                                foreach (string msg in FetchFromDB("Search Resources", "Virtual Tax::Crypto Tax"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Virtual Tax::Bitcoin Tax":
                                foreach (string msg in FetchFromDB("Search Resources", "Virtual Tax::Bitcoin Tax"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Virtual Tax":
                                foreach (string msg in FetchFromDB("Search Resources", "Virtual Tax"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Virtual Tax::Installment Plan":
                                foreach (string msg in FetchFromDB("Search Resources", "Virtual Tax::Installment Plan"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Missing Deadline":
                                foreach (string msg in FetchFromDB("Search Resources", "Missing Deadline"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); }  else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Preparation::941 Payroll Problems":
                                foreach (string msg in FetchFromDB("Search Resources", "Tax Preparation::941 Payroll Problems"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Preparation::Paying Back Taxes Information & Alternatives":
                                foreach (string msg in FetchFromDB("Search Resources", "Tax Preparation::Paying Back Taxes Information & Alternatives"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;



                            default:
                                if (contactcount<3)
                                {
                                    await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                                    if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                                    contactcount++;
                                }
                                else if (contactcount % 3 == 0)
                                {
                                    foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                                    {
                                        await context.PostAsync(msg);
                                        if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                    }
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if (contactcount<3)
                    {
                        await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                        if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                        contactcount++;
                    }
                    else if (contactcount % 3 == 0)
                    {
                        foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                        {
                            await context.PostAsync(msg);
                            if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                        }
                    }
                }

            }
        }

        [LuisIntent("Help From Professionals")]
        public async Task Professionalhelp(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (fetchloc == true)
            {
                FetchLocation(context);
                fetchloc = false;
            }
            var message = await activity;
            var BDLUIS = await GetEntityFromLUIS(message.Text);

            if (BDLUIS.intents.Count() > 0)
            {
                if (BDLUIS.entities.Count() > 0)
                {
                    for (int i = 0; i < BDLUIS.entities.Count(); i++)
                    {
                        switch (BDLUIS.entities[i].type)
                        {
                            case "Tax Relation::Tax Professionals":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Relation::Tax Professionals"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Relation::Tax Opinion Letters":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Relation::Tax Opinion Letters"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Relation::Tax Agency":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Relation::Tax Agency"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Relation::Enrolled Agent":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Relation::Enrolled Agent"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Relation::Tax Representative":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Relation::Tax Representative"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Relation::Collection Agency":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Relation::Collection Agency"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Relation::Tax collector":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Relation::Tax collector"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Relation::Tax Assessment":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Relation::Tax Assessment"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Relation::Lawyer":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Relation::Lawyer"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Relation::Tax Accountant":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Relation::Tax Accountant"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Firm::Professional Tax Firm":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Firm::Professional Tax Firm"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Firm::IRS Appeal":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Firm::IRS Appeal"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "IRS Tax Help::IRS Help":
                                foreach (string msg in FetchFromDB("Help From Professionals", "IRS Tax Help::IRS Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Firm::Personal Property Tax":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Firm::Personal Property Tax"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Firm::Certified Public Accountants(CPA)":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Firm::Certified Public Accountants(CPA)"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Firm::Tax Problems":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Firm::Tax Problems"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Firm::Penalty":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Firm::Penalty"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Firm::Bank Tax":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Firm::Bank Tax"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Firm::Tax Lien":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Firm::Tax Lien"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Firm::Return On Investment(ROI)":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Tax Firm::Return On Investment(ROI)"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Professional Tax Preparation Service":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Professional Tax Preparation Service"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "IRS Tax Help::Tax Lien Representation":
                                foreach (string msg in FetchFromDB("Help From Professionals", "IRS Tax Help::Tax Lien Representation"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Veterinary Tax Audit Help":
                                foreach (string msg in FetchFromDB("Help From Professionals", "Veterinary Tax Audit Help"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;



                            default:
                                if (contactcount<3)
                                {
                                    await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                                    if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                                    contactcount++;
                                }
                                else if (contactcount % 3 == 0)
                                {
                                    foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                                    {
                                        await context.PostAsync(msg);
                                        if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                    }
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if (contactcount<3)
                    {
                        await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                        if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                        contactcount++;
                    }
                    else if (contactcount % 3 == 0)
                    {
                        foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                        {
                            await context.PostAsync(msg);
                            if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                        }
                    }
                }

            }
        }
        [LuisIntent("Search TaxRelatedDefinition")]
        public async Task Taxdefhelp(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (fetchloc == true)
            {
                FetchLocation(context);
                fetchloc = false;
            }
            var message = await activity;
            var BDLUIS = await GetEntityFromLUIS(message.Text);

            if (BDLUIS.intents.Count() > 0)
            {
                if (BDLUIS.entities.Count() > 0)
                {
                    for (int i = 0; i < BDLUIS.entities.Count(); i++)
                    {
                        switch (BDLUIS.entities[i].type)
                        {
                            case "DefinitionTax::Tax Credit":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "DefinitionTax::Tax Credit"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Cost":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "Cost"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "DefinitionTax::Tax Bracket":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "DefinitionTax::Tax Bracket"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "DefinitionTax::Tax Deduction":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "DefinitionTax::Tax Deduction"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "DefinitionTax::Tax Avoidence":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "DefinitionTax::Tax Avoidence"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "DefinitionTax::Tax Expense":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "DefinitionTax::Tax Expense "))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "DefinitionTax::Tax Controversy":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "DefinitionTax::Tax Controversy"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "DefinitionTax::Tax Evasion":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "DefinitionTax::Tax Evasion"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "DefinitionTax::Social Security Tax":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "DefinitionTax::Social Security Tax"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "DefinitionTax::Estate Tax":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "DefinitionTax::Estate Tax"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "DefinitionTax::Tax Return":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "DefinitionTax::Tax Return"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Others::ATM (Automated Teller Machine)":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "Others::ATM (Automated Teller Machine)"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Others::Credit Card":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "Others::Credit Card"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Others::Expration Of Statute":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "Others::Expration Of Statute"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Others::Internal Revenue Code(IRC)":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "Others::Internal Revenue Code(IRC)"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Others::Internal Revenue Manual":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "Others::Internal Revenue Manual"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Others::Liability":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "Others::Liability"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Others::Preparer Tax Identification Number (PTIN)":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "Others::Preparer Tax Identification Number (PTIN)"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Others::Temporary Assistance for Needy Families (TANF)":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "Others::Temporary Assistance for Needy Families (TANF)"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Others::Unpaid Taxes":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "Others::Unpaid Taxes"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Others::Consumer Spending":
                                foreach (string msg in FetchFromDB("Search TaxRelatedDefinition", "Others::Consumer Spending"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;



                            default:
                                if (contactcount<3)
                                {
                                    await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                                    if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                                    contactcount++;
                                }
                                else if (contactcount % 3 == 0)
                                {
                                    foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                                    {
                                        await context.PostAsync(msg);
                                        if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                    }
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if (contactcount<3)
                    {
                        await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                        if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                        contactcount++;
                    }
                    else if (contactcount % 3 == 0)
                    {
                        foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                        {
                            await context.PostAsync(msg);
                            if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                        }
                    }
                }

            }
        }


        [LuisIntent("Search Tax")]
        public async Task taxcoll(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (fetchloc == true)
            {
                FetchLocation(context);
                fetchloc = false;
            }
            var message = await activity;
            var BDLUIS = await GetEntityFromLUIS(message.Text);

            if (BDLUIS.intents.Count() > 0)
            {
                if (BDLUIS.entities.Count() > 0)
                {
                    for (int i = 0; i < BDLUIS.entities.Count(); i++)
                    {
                        switch (BDLUIS.entities[i].type)
                        {
                            case "Tax Dept::Tax":
                                foreach (string msg in FetchFromDB("Search Tax", "Tax Dept::Tax"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Dept::IRS":
                                foreach (string msg in FetchFromDB("Search Tax", "Tax Dept::IRS"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Dept::Tax levy":
                                foreach (string msg in FetchFromDB("Search Tax", "Tax Dept::Tax levy"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Dept::Edd":
                                foreach (string msg in FetchFromDB("Search Tax", "Tax Dept::Edd"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Dept::Taxation":
                                foreach (string msg in FetchFromDB("Search Tax", "Tax Dept::Taxation"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Dept::Tax Negotiation":
                                foreach (string msg in FetchFromDB("Search Tax", "Tax Dept::Tax Negotiation"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Dept::Tax Advisor":
                                foreach (string msg in FetchFromDB("Search Tax", "Tax Dept::Tax Advisor"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Dept::Trust Fund Recovery Penalty":
                                foreach (string msg in FetchFromDB("Search Tax", "Tax Dept::Trust Fund Recovery Penalty"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Dept::Sales tax Service":
                                foreach (string msg in FetchFromDB("Search Tax", "Tax Dept::Sales tax Service"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Dept::TIPRA":
                                foreach (string msg in FetchFromDB("Search Tax", "Tax Dept::TIPRA"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Non Filer":
                                foreach (string msg in FetchFromDB("Search Tax", "Non Filer"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Audit Help::Clergy Tax Service":
                                foreach (string msg in FetchFromDB("Search Tax", "Audit Help::Clergy Tax Service"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Tax Debt Relief Services::IRS Offer in Compromise Services":
                                foreach (string msg in FetchFromDB("Search Tax", "Tax Debt Relief Services::IRS Offer in Compromise Services"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "DefinitionTax::Tax Avoidence":
                                foreach (string msg in FetchFromDB("Search Tax", "DefinitionTax::Tax Avoidence"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;




                            default:
                                if (contactcount<3)
                                {
                                    await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                                    if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                                    contactcount++;
                                }
                                else if (contactcount % 3 == 0)
                                {
                                    foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                                    {
                                        await context.PostAsync(msg);
                                        if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                    }
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if (contactcount<3)
                    {
                        await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                        if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                        contactcount++;
                    }
                    else if (contactcount % 3 == 0)
                    {
                        foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                        {
                            await context.PostAsync(msg);
                            if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                        }
                    }
                }

            }
        }


        [LuisIntent("Greeting")]
        public async Task greet(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (fetchloc == true)
            {
                FetchLocation(context);
                fetchloc = false;
            }
            var message = await activity;
            var BDLUIS = await GetEntityFromLUIS(message.Text);
            //string sbc = State.region;
            if (BDLUIS.intents.Count() > 0)
            {
                if (BDLUIS.entities.Count() > 0)
                {
                    for (int i = 0; i < BDLUIS.entities.Count(); i++)
                    {
                        switch (BDLUIS.entities[i].type)
                        {

                            case "Greet::Morning":
                                foreach (string msg in FetchFromDB("Greeting", "Greet::Morning"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Greet::Afternoon":
                                foreach (string msg in FetchFromDB("Greeting", "Greet::Afternoon"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Greet::Night":
                                foreach (string msg in FetchFromDB("Greeting", "Greet::Night"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Greet::Evening":
                                foreach (string msg in FetchFromDB("Greeting", "Greet::Evening"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Greet::Health":
                                foreach (string msg in FetchFromDB("Greeting", "Greet::Health"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Greet::Great":
                                foreach (string msg in FetchFromDB("Greeting", "Greet::Great"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Greet::ThankYou":
                                if (thankcount == 0)
                                {
                                    string text = "Thank you for using this service!!!";
                                    await context.PostAsync(text);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, text); } else { await keeplog(message.Text, text); }
                                    thankcount++;
                                }
                                else if (thankcount == 1)
                                {
                                    string text = "Thank you.";
                                    await context.PostAsync(text);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, text); } else { await keeplog(message.Text, text); }
                                    thankcount++;
                                }
                                else
                                {
                                    string text = "Thank you!!!";
                                    await context.PostAsync(text);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, text); } else { await keeplog(message.Text, text); }
                                }
                                break;
                            case "Greet::Hello":
                                foreach (string msg in FetchFromDB("Greeting", "Greet::Hello"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;

                        }
                    }
                }
                else
                {
                    if (hellocount == 0)
                    {
                        string text = "Hi, I am Tax Service advisor, You can ask me question related to Tax Services.";
                        await context.PostAsync(text);
                        if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, text); } else { await keeplog(message.Text, text); }
                        hellocount++;
                    }
                    else if (hellocount == 1)
                    {
                        string text = "Hi";
                        await context.PostAsync(text);
                        if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, text); } else { await keeplog(message.Text, text); }
                        hellocount++;
                    }
                    else
                    {
                        string text = "Typing 'Hi' or 'Hello' multiple times won't change my answer.";
                        await context.PostAsync(text);
                        if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, text); } else { await keeplog(message.Text, text); }
                    }
                }
            }

            /*------------------------*/

        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (fetchloc == true)
            {
                FetchLocation(context);
                fetchloc = false;
            }
            var message = await activity;
            foreach (string msg in FetchFromDB("Help", "searchforhelp"))
            {
                await context.PostAsync(msg);
                if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
            }           
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Identify Yourself")]
        public async Task tellaboutself(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (fetchloc == true)
            {
                FetchLocation(context);
                fetchloc = false;
            }
            var message = await activity;
            var BDLUIS = await GetEntityFromLUIS(message.Text);
            if (BDLUIS.intents.Count() > 0)
            {
                if (BDLUIS.entities.Count() > 0)
                {
                    for (int i = 0; i < BDLUIS.entities.Count(); i++)
                    {

                        switch (BDLUIS.entities[i].type)
                        {

                            case "Bot::Bot_Identity":
                                foreach (string msg in FetchFromDB("Identify Yourself", "Bot::Bot_Identity"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;

                        }
                    }
                }

            }

        }

        [LuisIntent("Frustration")]
        public async Task foulwords(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (fetchloc == true)
            {
                FetchLocation(context);
                fetchloc = false;
            }
            var message = await activity;
            if (foulcount == 0)
            {
                string text = "Are you sure you wanted to say this?";
                await context.PostAsync(text);
                if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, text); } else { await keeplog(message.Text, text); }
                foulcount++;
            }
            else if (foulcount == 1)
            {
                string text = "Please check your spellings.";
                await context.PostAsync(text);
                if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, text); } else { await keeplog(message.Text, text); }
                foulcount++;
            }
            else
            {
                string text = "Sorry !! Please note that we can only answer your questions on US Based Tax Related Services.";
                await context.PostAsync(text);
                if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, text); } else { await keeplog(message.Text, text); }
            }
        }

       
        [LuisIntent("officequery")]
        public async Task tellaboutoffice(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (fetchloc == true)
            {
                FetchLocation(context);
                fetchloc = false;
            }
            var message = await activity;
            var BDLUIS = await GetEntityFromLUIS(message.Text);
            if (BDLUIS.intents.Count() > 0)
            {
                if (BDLUIS.entities.Count() > 0)
                {
                    for (int i = 0; i < BDLUIS.entities.Count(); i++)
                    {
                       
                        switch (BDLUIS.entities[i].type)
                        {

                            case "Bot::Office_Identity":
                                foreach (string msg in FetchFromDB("officequery", "Bot::Office_Identity"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Privacy Policy":
                                foreach (string msg in FetchFromDB("officequery", "Privacy Policy"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Disclaimer":
                                foreach (string msg in FetchFromDB("officequery", "Disclaimer"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;
                            case "Spanish":
                                foreach (string msg in FetchFromDB("officequery", "Spanish"))
                                {
                                    await context.PostAsync(msg);
                                    if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                }
                                break;

                            default:
                                if (contactcount<3)
                                {
                                    await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                                    if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                                    contactcount++;
                                }
                                else if (contactcount % 3 == 0)
                                {
                                    foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                                    {
                                        await context.PostAsync(msg);
                                        if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                                    }
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if (contactcount<3)
                    {
                        await context.PostAsync("Hmmm...I didn’t quite get that. Shall we try again?");
                        if (Demo.response == "thirdeye_demo"){await keeplog_demo(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}else{await keeplog(message.Text, "Hmmm...I didn’t quite get that. Shall we try again?");}
                        contactcount++;
                    }
                    else if (contactcount % 3 == 0)
                    {
                        foreach (string msg in FetchFromDB("Taxrelated", "evalution"))
                        {
                            await context.PostAsync(msg);
                            if (Demo.response == "thirdeye_demo") { await keeplog_demo(message.Text, msg); } else { await keeplog(message.Text, msg); }
                        }
                    }
                }
            }

        }
    }
}
