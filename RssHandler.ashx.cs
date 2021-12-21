using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.SessionState;
using System.Xml;
namespace MyFshare
{
    /// <summary>
    /// Summary description for RssHandler
    /// </summary>
    internal class RSSItem
    {
        public string sum, title;
    }
    public class RssHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string response = "";
            if (context.Cache["response"] == null)
            {
                string cnStr = "Server=DESKTOP-NLDLLMF\\CHESSIE;Database=TangThuQuan; User Id = chessie; Password = 221198;";
                using (SqlConnection cnn = new SqlConnection(cnStr))
                {
                    cnn.Open();

                    string cmd = ("select link from MyRSS");
                    SqlCommand cmdGetLink = new SqlCommand(cmd, cnn);
                    SqlDataReader listLink = cmdGetLink.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(listLink);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string url = (string)row["link"];
                        XmlReader reader = XmlReader.Create(url);
                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        reader.Close();
                        string subject = "", summary = "";
                        List<RSSItem> listItem = new List<RSSItem>();
                        foreach (SyndicationItem item in feed.Items)
                        {
                            RSSItem rssItem = new RSSItem();
                            subject = subject + item.Title.Text + "\n";
                            summary = summary + item.Summary.Text + "\n";
                            rssItem.sum = item.Summary.Text;
                            rssItem.title = item.Title.Text;
                            listItem.Add(rssItem);

                        }
                        response = response + JsonConvert.SerializeObject(listItem);
                        context.Cache.Insert("response", response, null,
                        DateTime.MaxValue, TimeSpan.FromMinutes(15));

                    }
                }
            }
            else
            {
                response = context.Cache["response"].ToString();
            }
            context.Response.Write(response);
        }

        public bool IsReusable => false;
    }
}