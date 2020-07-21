using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IFilesRepository
    {
        void UploadFiles(Files attachment);
        void ViewFiles(Files attachment);
        int DeleteFiles(Files attachment);
    }
}
