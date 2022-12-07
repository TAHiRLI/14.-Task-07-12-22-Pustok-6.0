
using Microsoft.AspNetCore.Http;
using Pustok.Models;
using System;
using System.IO;

namespace Pustok.Helpers
{
    public class FileManager
    {
        public static string Save(IFormFile file, string rootPath, string folder,int maxLength)
        {
            var fileName = file.FileName;

            string newFileName = Guid.NewGuid().ToString() + (fileName.Length > maxLength - 36 ? fileName.Substring(fileName.Length - 164) : fileName);

            string path = Path.Combine(rootPath, folder, newFileName);

            using (FileStream fs = new FileStream(path, FileMode.Create ))
            {
                file.CopyTo(fs);
            }
            return newFileName;
        }

        public static void Delete(string rootPath, string folder, string fileName)
        {
            string path = Path.Combine(rootPath, folder, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
