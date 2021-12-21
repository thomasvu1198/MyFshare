using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.SessionState;

namespace MyFshare
{
    /// <summary>
    /// Summary description for login1
    /// </summary>
    internal class User
    {
        private int gold,id;
        private string username;
        private readonly string status;

        private string get_field_string(DataRow row, string field)
        {
            try
            {
                return (string)row[field];
            }
            catch
            {
                return "";
            }
        }
        private int get_field_int(DataRow row, string field)
        {

            return (int)row[field];
        }
        public void loadInfo(DataRow row)
        {
            gold = get_field_int(row, "gold");
            username = get_field_string(row, "username");
            id = get_field_int(row, "id");
        }
    }
    public class login1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["is_login"] != null)
            {
                string result = context.Session["jsonData"].ToString();
                context.Response.Write(result);
            }
            else
            {
                context.Response.ContentType = "text/plain";
                try
                {
                    string username = context.Request["username"];
                    string password = context.Request["pass"];
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
                            context.Session["password"] = password;
                            string authorizeString = ("Update Account set is_login = @value where id = @id");
                            SqlCommand authorizeCmd = new SqlCommand(authorizeString, cnn);
                            authorizeCmd.Parameters.AddWithValue("@value", 1);
                            authorizeCmd.Parameters.AddWithValue("@id", id);
                            authorizeCmd.ExecuteNonQuery();

                            string getUserGoldString = ("select username,gold,id from Account where id = @id");
                            SqlCommand getUserCmd = new SqlCommand(getUserGoldString, cnn);
                            getUserCmd.Parameters.AddWithValue("@id", id);
                            context.Session["id"] = id;
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
        }

        public bool IsReusable => false;
    }
}