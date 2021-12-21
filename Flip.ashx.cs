using System.Data.SqlClient;
using System.Web;
using System.Web.SessionState;

namespace MyFshare
{
    /// <summary>
    /// Summary description for Flip
    /// </summary>

    public class Flip : IHttpHandler, IRequiresSessionState
    {
        private const string cnStr = "Server=DESKTOP-NLDLLMF\\CHESSIE;Database=TangThuQuan; User Id = chessie; Password = 221198;";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string userName = (string)context.Session["username"];
                int amount = int.Parse(context.Request["amount"]);
                string status = (string)context.Request["status"] == "0" ? "-" : "+";
                using (SqlConnection cnn = new SqlConnection(cnStr))
                {
                    cnn.Open();
                    string cmd = ("Update Account set gold = (Account.gold " + status + " @amount) where username = @username;");
                    SqlCommand cmdRemoveGold = new SqlCommand(cmd, cnn);
                    cmdRemoveGold.Parameters.AddWithValue("@username", userName);
                    cmdRemoveGold.Parameters.AddWithValue("@amount", amount);
                    cmdRemoveGold.ExecuteNonQuery();
                    context.Response.Write("check");

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