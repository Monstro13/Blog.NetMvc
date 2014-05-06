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
    public class GroupContactRepository : EntityRepository<GroupContact>, IGroupContactRepository
    {
        public GroupContactRepository(BlogContext context) : base(context){}

        public GroupContact GetById(Int32 id)
        {
            return GetAll().FirstOrDefault(x => x.GroupContactId == id);
        }

        public GroupContact GetByTitle(String title)
        {
            return GetAll().FirstOrDefault(x => x.Title == title);
        }
    }
}
