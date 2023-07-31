using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiskAssessment.Models.Admin;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using static RiskAssessment.Models.Common;

namespace RiskAssessment.Controllers
{
    public class AdminController : Controller
    {
        public const string Name = "Admin";
        public const string ActionBackupAssets = nameof(BackupAssets);
        public const string ActionDownloadAssets = nameof(DownloadAssets);
        public IActionResult Index()
        {
            return View();
        }
        private List<string> DefaultRootPath()
        {
            return new List<string> { AbsolutePath.Flows, AbsolutePath.UserImage  };
        }
        private bool IsDefaultRootPath(string rootPath)
        {
            if (!DefaultRootPath().Contains(rootPath))
            {
                return false;
            }
            return true;
        }
        [HttpGet]
        [Route("Backup")]
        public IActionResult BackupAssets()
        {
            List<BackupModel> model = new List<BackupModel>();
            foreach (var rootPath in DefaultRootPath())
            {
                BackupModel backup = new BackupModel();
                backup.Name = rootPath;
                backup.Path = rootPath;
                try
                {
                    string[] allpath = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
                    foreach (var path in allpath)
                    {
                        backup.Size += new FileInfo(path).Length;
                    }
                }
                catch { }
                model.Add(backup);
            }
            return View(model);
        }

        [HttpPost]
        [Route("Backup")]
        public IActionResult BackupAssets([FromForm] IFormFile file, string rootPath)
        {
            try
            {
                if (!IsDefaultRootPath(rootPath))
                {
                    throw new Exception("RootPath is out of range");
                }

                if (file == null)
                {
                    return Json(new { code = 400, message = "Zipped file is required." });
                }

                var stream = file.OpenReadStream();
                var archive = new ZipArchive(stream);

                #region Clear all file in asset directory
                try
                {
                    string[] allpath = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
                    foreach (string path in allpath)
                    {
                        System.IO.File.Delete(path);
                    }
                    foreach (string dir in Directory.GetDirectories(rootPath))
                    {
                        System.IO.Directory.Delete(dir);
                    }
                }
                catch
                {

                }
                #endregion

                archive.ExtractToDirectory(Path.Combine(Directory.GetCurrentDirectory()), true);


                return Json(new { code = 200, message = "Assets have been restored." });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("Download")]
        public IActionResult DownloadAssets(string rootPath)
        {
            try
            {
                if (!IsDefaultRootPath(rootPath))
                {
                    throw new Exception("rootPath is out of range.");
                }
                string zipName = $"{rootPath}_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.zip";
                string[] allpath = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
                var tempoutput = "myzip.zip";
                try
                {
                    System.IO.File.Delete(tempoutput);
                }
                catch { }
                using (ZipOutputStream outputStream = new ZipOutputStream(new FileStream(tempoutput, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
                {
                    outputStream.SetLevel(9);
                    byte[] buffer = new byte[4096];
                    foreach (var path in allpath)
                    {
                        ZipEntry entry = new ZipEntry(path);
                        entry.IsUnicodeText = true;
                        outputStream.PutNextEntry(entry);

                        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                                outputStream.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                    outputStream.Finish();
                    outputStream.Flush();
                    outputStream.Close();
                }
                byte[] fileResult = System.IO.File.ReadAllBytes(tempoutput);
                try
                {
                    if (System.IO.File.Exists(tempoutput))
                    {
                        System.IO.File.Delete(tempoutput);
                    }
                }
                catch (Exception)
                {

                }
                if (fileResult == null)
                {
                    throw new Exception("Nothing found");
                }
                return File(fileResult, "application/zip", zipName);
            }
            catch (Exception ex)
            {
                ViewBag.Code = 500;
                ViewBag.Message = ex.Message;
                return View("MyError");
            }

        }
    }
}
