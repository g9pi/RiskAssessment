using Microsoft.AspNetCore.Http;
using System.IO;

namespace RiskAssessment.Service.IService
{
    public interface IStorageService
    {
        //FileStream GetFile(string fileName);
        FileStream GetFile(string fileName, string folder);
        string SaveFile(IFormFile file, string folder);
        string SaveFile(IFormFile file);
        void DeleteFile(string fileName);


        
        
        string SaveImage(IFormFile file);

        public FileStream GetImage(string fileName, string folder);
        
        public string SaveImage(IFormFile file, string folder);
    }
}
