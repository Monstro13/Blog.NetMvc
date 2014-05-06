using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repositories.DTO;

namespace iBlog.Models
{
    /// <summary>
    /// главная модель передаваемая на главную страницу
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// список новостей в ленте
        /// </summary>
        public List<PostInfo> News { get; set; }

        /// <summary>
        /// текущая страница ленты
        /// </summary>
        public Int32 CurrentPageNumder { get; set; }

        /// <summary>
        /// шаблон поиска новости
        /// </summary>
        public String SearchPattern { get; set; }
    }
}