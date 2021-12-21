using System.Data.SqlClient;
using System.Web;

namespace MyFshare
{
    /// <summary>
    /// Summary description for Authorize
    /// </summary>
    public class Authorize : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string username = context.Request["username"];
            string cnStr = "Server=DESKTOP-NLDLLMF\\CHESSIE;Database=TangThuQuan; User Id = chessie; Password = 221198; ";
            using (SqlConnection cnn = new SqlConnection(cnStr))
            {
                cnn.Open();
                string cmd = ("SELECT id FROM Account WHERE user_acc = @username");
                SqlCommand cmdFindID = new SqlCommand(cmd, cnn);
                cmdFindID.Parameters.AddWithValue("@username", username);
                string id = (cmdFindID.ExecuteScalar() ?? "").ToString();
                context.Response.ContentType = "text/plain";
                if (id != "")
                {
                    string authorizeString = ("select is_login from Account where id = @id");
                    SqlCommand authorizeCmd = new SqlCommand(authorizeString, cnn);
                    authorizeCmd.Parameters.AddWithValue("@id", id);
                    string is_login = (authorizeCmd.ExecuteScalar()).ToString(); 
                    string result = is_login == "" ? "false" : "check";
                    context.Response.Write(result);
                }
                else
                {
                    context.Response.Write("false");
                }
            }
        }

        public bool IsReusable => true;
    }
}