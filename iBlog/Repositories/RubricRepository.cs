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
    public class RubricRepository : EntityRepository<Rubric>, IRubricRepository
    {
        public RubricRepository(BlogContext context) : base(context){}

        public Rubric GetById(Int32 id)
        {
            return GetAll().FirstOrDefault(x => x.RubricId == id);
        }
    }
}
