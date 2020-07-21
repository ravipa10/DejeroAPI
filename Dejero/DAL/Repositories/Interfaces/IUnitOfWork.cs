using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IFilesRepository Files { get; }

        int SaveChanges();
    }
}
