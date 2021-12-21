using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.SessionState;

namespace MyFshare
{
    /// <summary>
    /// Summary description for update
    /// </summary>

    public class update : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string username = (string)context.Session["username"];
                string password = (string)context.Session["password"];
                string cnStr = "Server=DESKTOP-NLDLLMF\\CHESSIE;Database=TangThuQuan; User Id = chessie; Password = 221198; ";
                using (SqlConnection cnn = new SqlConnection(cnStr))
                {
                    cnn.Open();
                    string cmd = ("SELECT id FROM Account WHERE username = @username AND pass = @password");
                    SqlCommand cmdFindID = new SqlCommand(cmd, cnn);
                    cmdFindID.Parameters.AddWithValue("@username", username);
                    cmdFindID.Parameters.AddWithValue("@password", password);
                    string id = (cmdFindID.ExecuteScalar() ?? "").ToString();
                    if (id != "")
                    {
                        context.Session["username"] = username;
                        string getUserGoldString = ("select username,gold,id from Account where id = @id");
                        SqlCommand getUserCmd = new SqlCommand(getUserGoldString, cnn);
                        getUserCmd.Parameters.AddWithValue("@id", id);
                        SqlDataReader userData = getUserCmd.ExecuteReader();
                        DataTable dataTable = new DataTable();
                        dataTable.Load(userData);
                        User user = new User();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            user.loadInfo(row);
                        }
                        string result = JsonConvert.SerializeObject(dataTable);
                        context.Session["is_login"] = true;
                        context.Session["jsonData"] = result;
                        //context.Session["gold"] = gold;
                        context.Response.Write(result);

                    }

                    else
                    {
                        context.Response.Write("false");
                    }
                }
            }
            catch
            {
                context.Response.Write("false");
            }

        }

        public bool IsReusable => false;
    }
}