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
    public class AttachFileRepository : EntityRepository<AttachFile>, IAttachFileRepository
    {
        public AttachFileRepository(BlogContext context) : base(context){}

        public AttachFile GetById(Int32 id)
        {
            return GetAll().FirstOrDefault(x => x.AttachFileId == id);
        }
    }
}
