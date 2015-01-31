using System.Collections.Generic;
using System.Linq;

namespace PBC
{
    public class RecursiveFileProcessor
    {
        public static void Main(string[] args)
        {

        //    List<PackageInfomation> allPackageInfomations = new List<PackageInfomation>();
            List<ClassInfo> classInfos = new List<ClassInfo>();
            //for home... 
            string path = @"E:\EducatioN\MSSE\thesis\project\jedit32\jedit32";
            string fileName = @"C:\Users\x-man\Documents\jedit-3.2.xlsx";
            string folderDirectory = @"C:\Users\x-man\Documents\packagecluster\";
            // for lab...
           // string path = @"G:\thesis\jedit32\jedit32";
            //string fileName = @"C:\Users\xman\Documents\jedit-3.2.xlsx";


            PackageExtractor packageExtractor=new PackageExtractor();
            PackageConstructor packageConstructor=new PackageConstructor();


            List<PackageInfomation> allPackageInfomations = packageExtractor.GetAllPackageInfomations(path);
           var codeMetricsList= packageExtractor.LoadingCodeMetricsFromFile(fileName);


           packageConstructor.MakeCluste(allPackageInfomations, codeMetricsList, folderDirectory);

          
        }

    

       
    }

    public class PackageInfomation
    {
        public string packageName { get; set; }
        public string className { get; set; }
    }

    public class ClusterInfo
    {
        public string ClusterName { get; set; }
        public int NumberOfClass { get; set; }

    }

    public class ClassInfo
    {
        public string ClassName { get; set; }
        public string PackageName { get; set; }
    }
}
