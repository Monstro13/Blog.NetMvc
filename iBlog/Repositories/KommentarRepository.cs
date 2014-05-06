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
    public class KommentarRepository : EntityRepository<Kommentar>, IKommentarRepository
    {
        public KommentarRepository(BlogContext context) : base(context) { }

        public Kommentar GetById(Int32 id)
        {
            return GetAll().FirstOrDefault(x => x.KommentarId == id);
        }
    }
}
