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
    public class RatingKommentarRepository : EntityRepository<RatingKommentar>, IRatingKommentarRepository
    {
        public RatingKommentarRepository(BlogContext context) : base(context){}
    }
}
