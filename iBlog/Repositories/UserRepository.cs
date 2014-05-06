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
    public class UserRepository : EntityRepository<User>, IUserRepository
    {
        public UserRepository(BlogContext context) : base(context) { }

        public User GetById(Int32 id)
        {
            return GetAll().FirstOrDefault(x => x.UserId == id);
        }
    }
}
