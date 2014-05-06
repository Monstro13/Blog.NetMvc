using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Context;
using Model.Models;
using Repositories.Interfaces;

namespace Repositories
{
    public class FileTypeRepository : EntityRepository<FileType>, IFileTypeRepository
    {
        public FileTypeRepository(BlogContext context) : base(context){}

        public FileType GetById(Int32 id)
        {
            return GetAll().FirstOrDefault(x => x.FileTypeId == id);
        }
    }
}
