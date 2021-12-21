using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using AllFile;
using System.Data;
using Newtonsoft.Json; 

namespace MyFshare
{
    /// <summary>
    /// Summary description for GetAllData
    /// </summary>
    public class GetAllData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string cnStr = "Server=DESKTOP-NLDLLMF\\CHESSIE;Database=TangThuQuan; User Id = chessie; Password = 221198;";
            using (SqlConnection cnn = new SqlConnection(cnStr))
            {
                cnn.Open();
                string cmd = ("select Account.username, UserBackPack.file_path, UserBackPack.fileName from UserBackPack INNER JOIN Account ON UserBackPack.id_user = Account.id;");
                SqlCommand cmdGetAllFile = new SqlCommand(cmd, cnn);
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}