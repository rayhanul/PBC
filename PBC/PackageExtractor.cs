using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
namespace PBC
{
    public class PackageExtractor
    {
        public List<PackageInfomation> GetAllPackageInfomations(string path)
        {
            List<PackageInfomation> allPackageInfomations = new List<PackageInfomation>();
            string[] JavaAllFileContainer = Directory.GetFiles(path, "*.java",
                                         SearchOption.AllDirectories);

            foreach (string fileName in JavaAllFileContainer)
            {

                string FileText = System.IO.File.ReadAllText(fileName);

                string pattern = @"package .*\b";

                Match match = Regex.Match(FileText, pattern);

                if (match.Success)
                {
                    var package = new PackageInfomation
                    {
                        className = GetFileName(fileName),
                        packageName = GetPackageName(match.Value)
                    };
                    allPackageInfomations.Add(package);
                }
            }
            return allPackageInfomations;
        }

        private string GetFileName(string fileName)
        {
            string[] splited = fileName.Split('\\');
            string name = string.Empty;
            foreach (var it in splited)
            {
                if (it.EndsWith(".java"))
                {
                    name = it;
                }
            }
            name = name.Substring(0, name.IndexOf(".", StringComparison.Ordinal));
            return name;
        }

        private string GetPackageName(string packageName)
        {

            packageName = packageName.Replace("package", string.Empty);
            return packageName.Trim();
        }
        private List<object> GetListByDataTable(DataTable dt)
        {

            var reult = (from rw in dt.AsEnumerable()
                         select new
                         {
                             Name = Convert.ToString(rw["Name"]),
                             ID = Convert.ToInt32(rw["ID"])
                         }).ToList();

            return reult.ConvertAll<object>(o => (object)o);
        }
        public List<object[]> LoadingCodeMetricsFromFile(string fileName)
        {

            var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text\""; ;
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                var cmd2 = new OleDbCommand("SELECT * FROM [" + "jedit-3.2" + "]", conn);

                var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM [" + sheets.Rows[0]["TABLE_NAME"].ToString() + "] ";

                    var adapter = new OleDbDataAdapter(cmd);
                    var ds = new DataSet();
                    adapter.Fill(ds);

                    var myData = ds.Tables[0].AsEnumerable().Select(r => r.ItemArray).ToList();
                    return myData;
                }
            }

        }
    }
}
