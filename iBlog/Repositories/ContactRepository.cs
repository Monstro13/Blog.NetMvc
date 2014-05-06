﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Context;
using Model.Models;
using Repositories.Interfaces;

namespace Repositories
{
    public class ContactRepository : EntityRepository<Contact>, IContactRepository
    {
        public ContactRepository(BlogContext context) : base(context){}

        public Contact GetById(Int32 id)
        {
            return GetAll().FirstOrDefault(x => x.ContactId == id);
        }
    }
}
