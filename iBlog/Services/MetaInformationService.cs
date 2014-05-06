using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.UnitOfWork.Interfaces;
using Model.Models;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class MetaInformationService : ServiceBase, IMetaInformationService
    {
        public MetaInformationService(IUnitOfWorkFactory factory) : base(factory){}

        /// <summary>
        /// получение контактов сайта по ид
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dictionary<String, String> GetInformation(int id)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var contacts = uow.GetRepository<IGroupContactRepository>().GetById(id).Contacts;
                var types = contacts.Select(x => x.Type).ToList();
                var values = contacts.Select(x => x.Value).ToList();

                var dict = new Dictionary<String, String>();

                for (int i = 0; i < types.Count; i++)
                {
                    dict.Add(types[i], values[i]);
                }

                return dict;
            }
        }

        /// <summary>
        /// получение контактов сайта по названию
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Dictionary<String, String> GetInformation(string title)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var contacts = uow.GetRepository<IGroupContactRepository>().GetByTitle(title).Contacts;
                var types = contacts.Select(x => x.Type).ToList();
                var values = contacts.Select(x => x.Value).ToList();

                var dict = new Dictionary<String, String>();

                for (int i = 0; i < types.Count; i++)
                {
                    if (dict.Keys.Contains(types[i])) continue;
                    dict.Add(types[i], values[i]);
                }

                return dict;
            }
        }
    }
}
