using AllFile;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace MyFshare
{
    /// <summary>
    /// Summary description for GetUserFile
    /// </summary>
    public class GetUserFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string userID = context.Request["userID"];
                string cnStr = "Server=DESKTOP-NLDLLMF\\CHESSIE;Database=TangThuQuan; User Id = chessie; Password = 221198;";
                using (SqlConnection cnn = new SqlConnection(cnStr))
                {
                    cnn.Open();
                    string cmd = ("select file_path, fileName from UserBackPack where id_user = @id;");
                    SqlCommand cmdGetAllFile = new SqlCommand(cmd, cnn);
                    cmdGetAllFile.Parameters.AddWithValue("@id", int.Parse(userID));
                    SqlDataReader listFile = cmdGetAllFile.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(listFile);
                    context.Response.ContentType = "text/plain";
                    if (dataTable != null)
                    {
                        List<File> ListFile = new List<File>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            File file = new AllFile.File();
                            file.loadInfo(row);
                            ListFile.Add(file);
                        }
                        string result = JsonConvert.SerializeObject(dataTable);
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
                context.Response.ContentType = "text/plain";
                context.Response.Write("false");
            }

        }

        public bool IsReusable => false;
    }
}