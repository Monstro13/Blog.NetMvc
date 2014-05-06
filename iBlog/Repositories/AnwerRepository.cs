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
    public class AnwerRepository : EntityRepository<Answer>, IAnswerRepository
    {
        public AnwerRepository(BlogContext context) : base(context){}
    }
}
