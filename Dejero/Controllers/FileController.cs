using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Dejero.Models;
using System.Web.Http.Cors;
using AutoMapper;
using DataAccessLayer;
using DataAccessLayer.Models;


namespace Dejero.Controllers
{
    [Produces("application/json")]
    [Route("api/File")]
    //[EnableCors(origins: "http://localhost/4200", headers: "*", methods: "*")]
    //[ApiController]
    public class FileController : Controller
    {
        private IHostingEnvironment hostingEnv;
        Random random = new System.Random();

        public FileController(IHostingEnvironment _hostingEnv)
        {
            hostingEnv = _hostingEnv;
        }

        [HttpPost("Upload"), DisableRequestSizeLimit]
        public ActionResult FileUpload(IFormCollection file)
        {
            string fileName;
            string labels = "";
            string fileDesc = "";
            var files = Enumerable.Range(0, file.Files.Count).Select(i => file.Files[i]);
            foreach (var f in files)
            {
                string ext = Path.GetExtension(f.FileName);
                // Add extensions here if you need to allow more file formats
                if (String.IsNullOrEmpty(ext) ||
                 (!ext.Equals(".txt", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".pdf", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".doc", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".docx", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".xls", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".xlsm", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".png", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".jpg", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".gif", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".csv", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".kml", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".gpx", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".mov", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".mp4", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".wmv", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".flv", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".avi", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".mkv", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".webm", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".mts", StringComparison.OrdinalIgnoreCase) &&
                   !ext.Equals(".m4v", StringComparison.OrdinalIgnoreCase)))
                {
                    return BadRequest($"This file is not acceptable " + f.FileName);
                }
            }
            foreach (var f in files)
            {
                string folderName = "Upload";
                string webRootPath = hostingEnv.WebRootPath;
                string filePath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (f.Length > 0)
                {
                    if (f.Name == "undefined")
                    {
                        fileName = f.FileName;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(Path.GetExtension(f.Name)))
                            fileName = f.Name + Path.GetExtension(f.FileName);
                        else
                            fileName = f.Name;
                    }

                    string fullPath = Path.Combine(filePath, fileName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        int num = random.Next(100);
                        fileName = num + "" + fileName;
                        fullPath = Path.Combine(filePath, fileName);
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        f.CopyTo(stream);
                    }
                    var fileUploaded = new FileViewModel
                    {
                        FileName = fileName,
                        FileDesc = fileDesc,
                        Path = fullPath,
                        Labels = labels,
                        UploadedAt = DateTime.Now
                    };

                    using (var _context = new ApplicationDbContext())
                    {
                        var config = new MapperConfiguration(mc => mc.CreateMap<FileViewModel, Files>());
                        Mapper mapper = new Mapper(config);
                        Files toFile = mapper.Map<Files>(fileUploaded);

                        _context.Files.Add(toFile);
                        _context.SaveChanges();
                    }

                }
            }
            return Json("Upload Successful.");
        }

        [HttpGet("View")]
        public IActionResult ViewFiles()
        {
            using (var _context = new ApplicationDbContext())
            {
                return Ok(_context.Files.Select(x => new FileViewModel()
                {
                    Id = x.Id,
                    FileName = x.FileName,
                    FileDesc = x.FileDesc,
                    Path = x.Path,
                    Labels = x.Labels,
                    UploadedAt = x.UploadedAt
                }).ToList());
            }

        }

        [HttpGet("Delete/{FileId}")]
        public IActionResult DeleteFile(int FileId)
        {
            using (var _context = new ApplicationDbContext())
            {
                var file = _context.Files.Where(x => x.Id == FileId).FirstOrDefault();
                if (file == null)
                {
                    return BadRequest("File no longer exists");
                }
                else
                {
                    string fullPath = file.Path;

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    _context.Files.Remove(file);
                    _context.SaveChanges();

                    return Ok();
                }
            }
        }

        [HttpPost("SaveMetadata"), DisableRequestSizeLimit]
        public IActionResult SaveMetadata([FromBody] List<FileViewModel> fileList)
        {
            using (var _context = new ApplicationDbContext())
            {
                foreach (var x in fileList)
                {
                    var file = _context.Files.Find(x.Id);
                    if (file == null)
                    {
                        return BadRequest("File no longer exists");
                    }
                    else
                    {
                        var config = new MapperConfiguration(mc => mc.CreateMap<FileViewModel, Files>());
                        Mapper mapper = new Mapper(config);
                        Files toFile = mapper.Map<Files>(x);
                        _context.Files.Update(file);
                        _context.SaveChanges();                        
                    }
                }
                    
            }
            return Ok();
        }
    }
}
    