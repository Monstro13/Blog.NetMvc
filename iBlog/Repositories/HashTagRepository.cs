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
    public class HashTagRepository : EntityRepository<HashTag>, IHashTagRepository
    {
        public HashTagRepository(BlogContext context) : base(context){}

        public HashTag GetById(Int32 id)
        {
            return GetAll().FirstOrDefault(x => x.HashTagId == id);
        }
    }
}
