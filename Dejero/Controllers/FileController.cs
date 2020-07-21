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
//using DataAccessLayer.Repositories.Interfaces;

namespace Dejero.Controllers
{
    [Produces("application/json")]
    [Route("api/File")]
    //[EnableCors(origins: "https://localhost/4200", headers: "*", methods: "*")]
    //[ApiController]
    public class FileController : Controller
    {
        private IHostingEnvironment hostingEnv;
        Random random = new System.Random();
        //private IUnitOfWork unitOfWork;
        //private readonly ApplicationDbContext _context;
        //UnitOfWork _unitOfWork, ApplicationDbContext context
        public FileController(IHostingEnvironment _hostingEnv)
        {
            //unitOfWork = _unitOfWork;
            //context = _context;
            hostingEnv = _hostingEnv;
            
        }

        [HttpPost("Upload"), DisableRequestSizeLimit]
        public ActionResult FileUpload(IFormCollection file)
        {
            string fileName;
            string labels = "";
            var files = Enumerable.Range(0, file.Files.Count).Select(i => file.Files[i]);
            foreach (var f in files)
            {
                string ext = Path.GetExtension(f.FileName);
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
                   !ext.Equals(".gpx", StringComparison.OrdinalIgnoreCase)))
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
                        fileName = fileName + "" + num;
                        fullPath = Path.Combine(filePath, fileName);
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        f.CopyTo(stream);
                    }
                    var fileUploaded = new FileViewModel
                    {
                        FileName = fullPath,
                        Labels = labels,
                        UploadedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now
                    };

                    //var config = new MapperConfiguration(mc => mc.CreateMap<FileViewModel,Files>());
                    //Mapper mapper = new Mapper(config);
                    //Files toFile = mapper.Map<Files>(fileUploaded);
                    //unitOfWork.Files.UploadFiles(toFile);
                }
            }
            return Json("Upload Successful.");
        }
    }
}