using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace Kernel.Core.Utils
{
    /// <summary>
    /// 压缩操作相关操作类，压缩格式为ZIP
    /// </summary>
    public class Compress
    {
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="targetFileName">目标文件名</param>
        /// <param name="sourceFileNames">源文件名</param>
        /// <returns>压缩包压缩是否成功</returns>
        public static bool ZipFile(string targetFileName, IEnumerable<string> sourceFileNames)
        {
            bool result = true;

            //using (ZipFile zip = new ZipFile(Encoding.Default))
            //{
            //    foreach (string filename in sourceFileNames)
            //    {
            //        ZipEntry e = zip.AddFile(filename, "");
            //    }

            //    zip.Save(targetFileName);
            //}

            using (ZipFile zip = new ZipFile(Encoding.Default))
            {
                zip.AddFiles(sourceFileNames);

                zip.Save(targetFileName);
            }

            return result;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="targetFileName">目标文件名</param>
        /// <param name="sourceDirectory">源目录</param>
        /// <returns>压缩包压缩是否成功</returns>
        public static bool DoZipFile(string targetFileName, string sourceDirectory, Dictionary<string, string> nameMapper = null)
        {
            bool result = true;

            //string[] sourceFileNames = Directory.GetFiles(sourceDirectory, ".", SearchOption.AllDirectories);
            //using (ZipFile zip = new ZipFile(Encoding.Default))
            //{
            //    foreach (string filename in sourceFileNames)
            //    {
            //        ZipEntry e = zip.AddFile(filename, "");
            //    }

            //    zip.Save(targetFileName);
            //}

            var targetPath = Path.GetDirectoryName(targetFileName);
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);

            using (ZipFile zip = new ZipFile(Encoding.Default))
            {
                zip.AddDirectory(sourceDirectory);

                //处理文件名映射
                if (nameMapper != null)
                {
                    var items = zip.Entries.ToList();
                    for (int i = 0; i < items.Count(); i++)
                    {
                        var item = items[i];
                        if (nameMapper.ContainsKey(item.FileName))
                            item.FileName = nameMapper[item.FileName];
                    }
                }

                zip.Save(targetFileName);
            }

            return result;
        }

        /// <summary>
        /// 解压zip文件
        /// </summary>
        /// <param name="targetDirectory">解压后目录</param>
        /// <param name="zipFileName">压缩包文件名</param>
        /// <returns>解压结果是否成功</returns>
        public static bool UnZipFile(string targetDirectory, string zipFileName)
        {
            bool result = true;

            using (ZipFile zip = new ZipFile(zipFileName))
            {
                zip.ExtractAll(targetDirectory);
            }

            return result;
        }

        /// <summary>
        /// 更新压缩包内容
        /// </summary>
        /// <param name="zipFileName">压缩文件名</param>
        /// <param name="sourceFileName">要更新的文件</param>
        public static void UpdateFileToZip(string zipFileName, string sourceFileName)
        {
            using (ZipFile zip = new ZipFile(zipFileName))
            {
                string fileName = Path.GetFileName(sourceFileName);
                if (zip.ContainsEntry(fileName))
                {
                    var stream = File.OpenRead(sourceFileName);
                    var z = zip.UpdateEntry(fileName, stream);
                    z.Comment = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }

                zip.Save();
            }
        }

        public static string ExtractSingleFile(string zipFileName, string fileName, string extractDirectoryPath = "")
        {
            string tempFileName = "";
            if (string.IsNullOrEmpty(extractDirectoryPath))
            {
                string temp = Environment.GetEnvironmentVariable("TEMP");
                DirectoryInfo info = new DirectoryInfo(temp);
                var tempInfo = info.CreateSubdirectory(Guid.NewGuid().ToString());
                extractDirectoryPath = tempInfo.FullName;
            }

            byte[] content = null;
            //转换stream为byte[]  
            Func<Stream, byte[]> toByteArray = (stream) =>
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                // 设置当前流的位置为流的开始   
                stream.Seek(0, SeekOrigin.Begin);
                return bytes;
            };

            using (ZipFile zip = new ZipFile(zipFileName, Encoding.Default))
            {
                if (zip.ContainsEntry(fileName))
                {
                    var entries = zip.Entries.Where(u => u.FileName.Equals(fileName));
                    if (entries == null || entries.Count() <= 0)
                    {
                        return "";
                    }

                    var entry = entries.First();

                    //将文件解压到内存流  
                    using (var stream = new MemoryStream())
                    {
                        entry.Extract(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        content = toByteArray(stream);
                    }
                }
            }

            //创建文件  
            tempFileName = Path.Combine(extractDirectoryPath, fileName);
            File.WriteAllBytes(tempFileName, content);

            return tempFileName;
        }

    }
}
