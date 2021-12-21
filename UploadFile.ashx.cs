using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace MyFshare
{
    /// <summary>
    /// Summary description for UploadFile
    /// </summary>
    public class UploadFile : IHttpHandler, IRequiresSessionState
    {
        private const string FILEROOT = @"C:\Users\thoma\source\repos\MyFshare\media\";
        public void AddGold(HttpContext context, string file_path, string fileName)
        {
            string cnStr = "Server=DESKTOP-NLDLLMF\\CHESSIE;Database=TangThuQuan; User Id = chessie; Password = 221198;";
            string userName = (string)context.Session["username"];
            int id = int.Parse((string)context.Session["id"]);
            using (SqlConnection cnn = new SqlConnection(cnStr))
            {
                cnn.Open();
                string cmd = ("Update Account set gold = Account.gold + 100 where username = @username;");
                SqlCommand cmdRemoveGold = new SqlCommand(cmd, cnn);
                cmdRemoveGold.Parameters.AddWithValue("@username", userName);
                cmdRemoveGold.ExecuteNonQuery();

                string cmd2 = ("Insert into UserBackPack(file_path, id_user, fileName) values(@path, @id, @name);");
                SqlCommand insertData = new SqlCommand(cmd2, cnn);
                insertData.Parameters.AddWithValue("@path", file_path);
                insertData.Parameters.AddWithValue("@id", id);
                insertData.Parameters.AddWithValue("@name", fileName);
                insertData.ExecuteNonQuery();

            }
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string dirFullPath = FILEROOT;
            string[] files;
            int numFiles;
            files = System.IO.Directory.GetFiles(dirFullPath);
            numFiles = files.Length;
            numFiles = numFiles + 1;
            string str_image = "";
            if (context.Request.Files == null)
            {
                context.Response.Write("false");
            }
            else
            {
                foreach (string s in context.Request.Files)
                {
                    HttpPostedFile file = context.Request.Files[s];
                    string fileName = file.FileName;
                    string fileExtension = file.ContentType;

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        fileExtension = Path.GetExtension(fileName);
                        str_image = "[TTQ] " + numFiles.ToString() + fileExtension;
                        string pathToSave = dirFullPath + str_image;
                        AddGold(context, str_image, str_image);
                        file.SaveAs(pathToSave);
                    }
                }
                context.Response.Write("check");
            }


        }

        public bool IsReusable => false;
    }
}