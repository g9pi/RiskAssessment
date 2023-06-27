using Microsoft.AspNetCore.Http;
using System.IO;

namespace RiskAssessment.Service.IService
{
    public interface IStorageService
    {
        FileStream GetFile(string fileName);
        string SaveFile(IFormFile file);
        void DeleteFile(string fileName);

        FileStream GetImage(string fileName);
        string SaveImage(IFormFile file);
    }
}
