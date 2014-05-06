using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using Repositories.DTO;

namespace Services.Interfaces
{
    public interface IOptionService
    {
        /// <summary>
        /// выбрать 20 новостей для страницы
        /// </summary>
        /// <returns></returns>
        IEnumerable<TagInfo> GetTop20Tags();
        /// <summary>
        /// взять все рубрики новостей
        /// </summary>
        /// <returns></returns>
        IEnumerable<RubricInfo> GetAllRubrics();
        /// <summary>
        /// взять все теги
        /// </summary>
        /// <returns></returns>
        IEnumerable<String> GetAllTags();
        /// <summary>
        /// взять все ссылки
        /// </summary>
        /// <returns></returns>
        IEnumerable<String> GetAllLinks();
        /// <summary>
        /// проверить на существование рубрики
        /// </summary>
        /// <param name="rubricId"></param>
        /// <returns></returns>
        Boolean IsRubricExists(int rubricId);
        /// <summary>
        /// взять непрочитанные сообщения пользователя
        /// </summary>
        /// <param name="getUserId"></param>
        /// <returns></returns>
        Int32 GetUnWatchedMessages(int getUserId);
        /// <summary>
        /// взять все контакты сайта
        /// </summary>
        /// <returns></returns>
        IEnumerable<ContactInfo> GetAllContacts();
        /// <summary>
        /// установить тип контакта
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        Boolean SetType(int contactId, string text);
        /// <summary>
        /// установить значение контакту 
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        Boolean SetValue(int contactId, string text);
        /// <summary>
        /// удалить контакт
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Boolean DeleteContact(int id);
        /// <summary>
        /// добавить контакт
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Boolean AddContact(string type, string value);
        /// <summary>
        /// взять все категории
        /// </summary>
        /// <returns></returns>
        IEnumerable<CategoryInfo> GetAllCategories();
        /// <summary>
        /// установить название категории
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        Boolean SetTitle(int id, string text);
        /// <summary>
        /// установить код категории
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        Boolean SetCode(int id, string text);
        /// <summary>
        /// удалить категорию
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Boolean DeleteCategory(int id);
        /// <summary>
        /// добавить категорию
        /// </summary>
        /// <param name="title"></param>
        /// <param name="code"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        Boolean AddCategory(string title, string code, string desc);
        /// <summary>
        /// установить название рубрики
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        Boolean SetTitleRubric(int id, string text);
        /// <summary>
        /// удалить рубрику
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Boolean DeleteRubric(int id);
        /// <summary>
        /// добавить рубрику
        /// </summary>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        Boolean AddRubric(string title, string desc);
    }
}
