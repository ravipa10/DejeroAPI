using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class FilesRepository : Repository<Files>, IFilesRepository
    {
        public FilesRepository(ApplicationDbContext context) : base(context)
        { }

        private ApplicationDbContext appContext
        {
            get { return (ApplicationDbContext)_context; }
        }

        private Files getExistingFile(Files file)
        {
            return appContext.Files.Where(x => x.FileName.Equals(file.FileName)).FirstOrDefault();
        }

        public void UploadFiles(Files file)
        {
            Files ExistingFile = getExistingFile(file);
            if (ExistingFile != null)
            {
                ExistingFile.ModifiedAt = file.ModifiedAt;
                appContext.Files.Update(ExistingFile);
            }
            else
            {
                appContext.Files.Add(file);
            }
            appContext.SaveChanges();

        }

        public int DeleteFiles(Files file)
        {
            return 1;
        }


        public void ViewFiles(Files file)
        {
        }
    }
}
