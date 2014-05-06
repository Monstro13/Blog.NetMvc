using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.Interfaces
{
    public interface IGroupContactRepository : IRepository<GroupContact>
    {
        GroupContact GetById(int id);

        GroupContact GetByTitle(string title);
    }
}
