using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace MyFshare
{
    /// <summary>
    /// Summary description for Download
    /// </summary>
    public class Download : IHttpHandler, IRequiresSessionState
    {
        private const string FILEROOT = @"C:\Users\thoma\source\repos\MyFshare\media\";
        private const string cnStr = "Server=DESKTOP-NLDLLMF\\CHESSIE;Database=TangThuQuan; User Id = chessie; Password = 221198;";
        public void RemoveGold(string userName)
        {
            using (SqlConnection cnn = new SqlConnection(cnStr))
            {
                cnn.Open();
                string cmd = ("Update Account set gold = Account.gold - 100 where username = @username;");
                SqlCommand cmdRemoveGold = new SqlCommand(cmd, cnn);
                cmdRemoveGold.Parameters.AddWithValue("@username", userName);
                cmdRemoveGold.ExecuteNonQuery();

            }
        }
        public bool CheckGold(string userName)
        {
            bool result;
            using (SqlConnection cnn = new SqlConnection(cnStr))
            {
                cnn.Open();
                string cmd = ("Select gold from Account where username = @username;");
                SqlCommand cmdRemoveGold = new SqlCommand(cmd, cnn);
                cmdRemoveGold.Parameters.AddWithValue("@username", userName);
                int gold = (int)(cmdRemoveGold.ExecuteScalar());
                result = gold < 100 ? false : true;

            }
            return result;
        }
        public void ProcessRequest(HttpContext context)
        {
            string fileName = context.Request["fileName"];
            string filePath = FILEROOT + fileName;
            string username = (string)context.Session["username"];
            if (username == null)
            {
                context.Response.Write("false");
            }
            else
            {
                bool isGoldEnough = CheckGold(username);
                if (File.Exists(filePath) && isGoldEnough == true)
                {
                    RemoveGold(username);
                    using (Image image = Image.FromFile(filePath))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            string base64String = Convert.ToBase64String(imageBytes);
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(base64String);
                        }
                    }
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("KHÔNG TỒN TẠI file:  " + fileName);
                }
            }
        }

        public bool IsReusable => false;
    }
}
