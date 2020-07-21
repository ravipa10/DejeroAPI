using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

        IFilesRepository _files;

        public IFilesRepository Files
        {
            get
            {
                if (_files == null)
                    _files = new FilesRepository(_context);

                return _files;
            }
        }


        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
