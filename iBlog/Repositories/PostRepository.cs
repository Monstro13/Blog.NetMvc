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
    public class PostRepository : EntityRepository<Post>, IPostRepository
    {
        public PostRepository(BlogContext context) : base(context) { }

        public Post GetById(Int32 id)
        {
            return GetAll().FirstOrDefault(x => x.PostId == id);
        }
    }
}
