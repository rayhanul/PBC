
using LinqToExcel;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using OfficeOpenXml;
namespace PBC
{
    public class PackageConstructor
    {
        public PackageConstructor()
        {
        }


        private List<ClusterInfo> GetUniquePackageLists(List<PackageInfomation> allPackageInfomations)
        {
            var numberOfPackages = allPackageInfomations.Select(a => a.packageName).Distinct().ToList();


            var numberOfClassesInPackage = new List<ClusterInfo>();

            foreach (var numberOfPackage in numberOfPackages)
            {
                int count = allPackageInfomations.Count(ap => ap.packageName.Equals(numberOfPackage));

                var item = new ClusterInfo()
                {
                    ClusterName = numberOfPackage,
                    NumberOfClass = count
                };

                numberOfClassesInPackage.Add(item);
            }

            return numberOfClassesInPackage;
        }

        public void MakeCluste(List<PackageInfomation> allPackageInfomations, List<object[]> objectList, string path)
        {

            var packageList = GetUniquePackageLists(allPackageInfomations);
            var item = from objList in objectList select objList;
            item = item.ToList();


            foreach (var package in packageList)
            {
                List<object[]> classInfoList = new List<object[]>();
                foreach (var classwithPackageInfo in allPackageInfomations)
                {
                    if (package.ClusterName.Trim() == classwithPackageInfo.packageName.Trim())
                    {
                        string fileNameWithPackageInfo = string.Format("{0}.{1}", classwithPackageInfo.packageName, classwithPackageInfo.className);

                        foreach (var obj in item)
                        {

                            string fileNameFromList = obj[2].ToString();

                            if (fileNameFromList.Trim() == fileNameWithPackageInfo.Trim())
                            {
                                classInfoList.Add(obj);
                                break;
                            }
                        }
                    }
                }

                if (classInfoList.Count > 0)
                {
                    classInfoList.Insert(0, objectList[0]);
                    WriteToExcelFile(path, package.ClusterName, classInfoList);
                }
            }
        }


        public List<object[]> GetObjectList(string name, List<object[]> objectList)
        {


            return null;
        }

        public void WriteToExcelFile(string filePath, string fileName, List<object[]> data)
        {
            try
            {
                filePath = string.Format("{0}{1}.{2}", filePath, fileName, "xlsx");

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                FileInfo newFile = new FileInfo(filePath);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {

                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("JEdit-3.2");

                    for (int j = 0; j < data.Count; j++)
                    {
                        var item = data[j];
                        int i = 1;
                        foreach (var inner in item)
                        {

                            worksheet.Cell(j + 2, i).Value = inner.ToString();
                            i++;
                        }

                    }
                    xlPackage.Save();
                }
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}
