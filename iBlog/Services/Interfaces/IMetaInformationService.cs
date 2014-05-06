using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Services.Interfaces
{
    public interface IMetaInformationService
    {
        /// <summary>
        /// получить контакты сайта по ид
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Dictionary<String, String> GetInformation(Int32 id);

        /// <summary>
        /// получить контакты сайта по названию
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        Dictionary<String, String> GetInformation(String title);
    }
}
