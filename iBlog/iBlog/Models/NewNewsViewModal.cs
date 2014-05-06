using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iBlog.Models
{
    /// <summary>
    /// модель собираемая со страницы добавления новости
    /// </summary>
    public class NewNewsViewModal
    {
        /// <summary>
        /// прикрепленные файлы
        /// </summary>
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        /// <summary>
        /// ид рубрики, под которую попадает новость
        /// </summary>
        [Required]
        public Int32 RubricId { get; set; }

        /// <summary>
        /// заголовок новости
        /// </summary>
        [Required]
        public String TitlePost { get; set; }

        /// <summary>
        /// текст новости
        /// </summary>
        [Required]
        public String Text { get; set; }

        /// <summary>
        /// список хэштегов
        /// </summary>
        public IEnumerable<String> HashTags { get; set; }

        /// <summary>
        /// список ссылок используемых при составлении новости
        /// </summary>
        public IEnumerable<String> Links { get; set; }

        /// <summary>
        /// настройка видимости новости
        /// </summary>
        public Boolean ForSelfCategory { get; set; }
    }
}