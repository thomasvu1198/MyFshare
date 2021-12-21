using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AllFile
{
    public class File
    {
        public string filePath, userUpload;
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
        public void loadInfo(DataRow row)
        {
            filePath = get_field_string(row, "file_path");
            userUpload = get_field_string(row, "username");
            
        }
    }
}