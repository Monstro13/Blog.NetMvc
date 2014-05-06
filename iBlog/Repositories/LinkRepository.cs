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
    public class LinkRepository : EntityRepository<Link>, ILinkRepository
    {
        public LinkRepository(BlogContext context) : base(context){}

        public Link GetById(Int32 id)
        {
            return GetAll().FirstOrDefault(x => x.LinkId == id);
        }
    }
}
