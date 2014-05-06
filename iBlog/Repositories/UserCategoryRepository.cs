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
    public class UserCategoryRepository : EntityRepository<UserCategory>, IUserCategoryRepository
    {
        public UserCategoryRepository(BlogContext context) : base(context){}

        public UserCategory GetById(Int32 id)
        {
            return GetAll().FirstOrDefault(x => x.UserCategoryId == id);
        }
    }
}
