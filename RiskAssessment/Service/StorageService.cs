using Microsoft.AspNetCore.Http;
using RiskAssessment.Models.Data;
using RiskAssessment.Service.IService;
using System;
using System.IO;

namespace RiskAssessment.Service
{
    public class StorageService : IStorageService
    {
        public StorageService()
        {

        }


        public FileStream GetFile(string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), RelativePath.File, fileName);
            var stream = new FileStream(path, FileMode.Open);
            return stream;
        }
        public string SaveFile(IFormFile file)
        {
            string fileName = "file_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), RelativePath.File);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);
            using (var stream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }

        public void DeleteFile(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), RelativePath.File, fileName);
            File.Delete(path);
        }

        public FileStream GetImage(string fileName)
        {
            return GetFile(fileName);
        }

        public string SaveImage(IFormFile file)
        {
            string fileName = "img_" + DateTime.Now.ToString("yyyyMMddHHmmss_ffffff") + Path.GetExtension(file.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), RelativePath.File);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);
            using (var stream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }
    }
}
