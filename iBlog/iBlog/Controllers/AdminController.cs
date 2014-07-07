using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Model.Models.LkpEnums;
using Services.Interfaces;

namespace iBlog.Controllers
{
    public class AdminController : Controller
    {

        #region Contacts api

        /// <summary>
        /// получить все контакты
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult GetAllContacts()
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(new { Contacts = ServiceLocator.Current.GetInstance<IOptionService>().GetAllContacts() });
            }
            else return Json(null);
        }

        /// <summary>
        /// установить тип контакта
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult SetTypeContact(String contactId, String text)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                var id = 0;
                if (Int32.TryParse(contactId, out id))
                {
                    return Json(ServiceLocator.Current.GetInstance<IOptionService>().SetType(id, text).ToString());
                }
                else return Json("false");
            }
            else return Json("false");
        }

        /// <summary>
        /// установить значение определенному типу
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult SetValueContact(String contactId, String text)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                var id = 0;
                if (Int32.TryParse(contactId, out id))
                {
                    return Json(ServiceLocator.Current.GetInstance<IOptionService>().SetValue(id, text).ToString());
                }
                else return Json("false");
            }
            else return Json("false");
        }

        /// <summary>
        /// удаление контакта
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult DeleteContact(String contactId)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                var id = 0;
                if (Int32.TryParse(contactId, out id))
                {
                    return Json(ServiceLocator.Current.GetInstance<IOptionService>().DeleteContact(id).ToString());
                }
                else return Json("false");
            }
            else return Json("false");
        }

        /// <summary>
        /// добавление контакта
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult AddContact(String type, String value)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(ServiceLocator.Current.GetInstance<IOptionService>().AddContact(type, value).ToString());
            }
            else return Json("false");
        }

        #endregion

        #region Categories api

        /// <summary>
        /// получить все категории пользователей
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult GetAllCategories()
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(new { Categories = ServiceLocator.Current.GetInstance<IOptionService>().GetAllCategories() });
            }
            else return Json(null);
        }

        /// <summary>
        /// установить название категории
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult SetTitleCategory(String categoryId, String text)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                var id = 0;
                if (Int32.TryParse(categoryId, out id))
                {
                    return Json(ServiceLocator.Current.GetInstance<IOptionService>().SetTitle(id, text).ToString());
                }
                else return Json("false");
            }
            else return Json("false");
        }

        /// <summary>
        /// установить код категории 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult SetCodeCategory(String categoryId, String text)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                var id = 0;
                if (Int32.TryParse(categoryId, out id))
                {
                    return Json(ServiceLocator.Current.GetInstance<IOptionService>().SetCode(id, text).ToString());
                }
                else return Json("false");
            }
            else return Json("false");
        }

        /// <summary>
        /// удаление категории
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult DeleteCategory(String categoryId)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                var id = 0;
                if (Int32.TryParse(categoryId, out id))
                {
                    return Json(ServiceLocator.Current.GetInstance<IOptionService>().DeleteCategory(id).ToString());
                }
                else return Json("false");
            }
            else return Json("false");
        }

        /// <summary>
        /// добавить новую категорию
        /// </summary>
        /// <param name="title"></param>
        /// <param name="code"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult AddCategory(String title, String code, String desc)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(ServiceLocator.Current.GetInstance<IOptionService>().AddCategory(title, code, desc).ToString());
            }
            else return Json("false");
        }

        #endregion

        #region Rubrics api

        /// <summary>
        /// получить все рубрики новостей
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult GetAllRubrics()
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(new { Rubrics = ServiceLocator.Current.GetInstance<IOptionService>().GetAllRubrics() });
            }
            else return Json(null);
        }

        /// <summary>
        /// установить название рубрики
        /// </summary>
        /// <param name="rubricId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult SetTitleRubric(String rubricId, String text)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                var id = 0;
                if (Int32.TryParse(rubricId, out id))
                {
                    return Json(ServiceLocator.Current.GetInstance<IOptionService>().SetTitleRubric(id, text).ToString());
                }
                else return Json("false");
            }
            else return Json("false");
        }

        /// <summary>
        /// удалить рубрику
        /// </summary>
        /// <param name="rubricId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult DeleteRubric(String rubricId)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                var id = 0;
                if (Int32.TryParse(rubricId, out id))
                {
                    return Json(ServiceLocator.Current.GetInstance<IOptionService>().DeleteRubric(id).ToString());
                }
                else return Json("false");
            }
            else return Json("false");
        }

        /// <summary>
        /// добавить новую рубрику
        /// </summary>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult AddRubric(String title, String desc)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(ServiceLocator.Current.GetInstance<IOptionService>().AddRubric(title, desc).ToString());
            }
            else return Json("false");
        }

        #endregion

        #region News

        /// <summary>
        /// переход на страницу администрирования новостей
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult News()
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Page404", "Apis");
            }
        }

        /// <summary>
        /// получить все новости из базы используя фильтрацию
        /// </summary>
        /// <param name="searchPattern">поисковой шаблон</param>
        /// <param name="page">пагинация</param>
        /// <param name="filter">категория новости</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public JsonResult GetNews(String searchPattern = "", Int32 page = 1, NewsFilter filter = NewsFilter.all)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(new {News = ServiceLocator.Current.GetInstance<IPostService>().GetPostsByFilter(searchPattern, page, filter)});
            }
            else
            {
                return Json(null);
            }
        }

        /// <summary>
        /// объявить новость спамом
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public JsonResult SpamNews(Int32 newsId)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(ServiceLocator.Current.GetInstance<IPostService>().SpamNews(newsId).ToString());
            }
            else
            {
                return Json(null);
            }
        }

        /// <summary>
        /// подтвердить новость, что бы та попала в новостную ленту
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public JsonResult ConfirmNews(Int32 newsId)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(ServiceLocator.Current.GetInstance<IPostService>().ConfirmNews(newsId).ToString());
            }
            else
            {
                return Json(null);
            }
        }

        #endregion

        #region Users

        /// <summary>
        /// переход на страницу администрирования пользователей
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Users()
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Page404", "Apis");
            }
        }

        /// <summary>
        /// получить всех пользователей из базы используя фильтрацию
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <param name="page"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public JsonResult GetUsers(String searchPattern = "", Int32 page = 1, UsersFilter filter = UsersFilter.all)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(new { Users = ServiceLocator.Current.GetInstance<IUserService>().GetUsersByFilter(searchPattern, page, filter) });
            }
            else
            {
                return Json(null);
            }
        }

        /// <summary>
        /// дать права админа пользователю
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public JsonResult SetAdmin(Int32 usersId)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(ServiceLocator.Current.GetInstance<IUserService>().SetAdmin(usersId).ToString());
            }
            else
            {
                return Json(null);
            }
        }

        /// <summary>
        /// забрать права админа у пользователя
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public JsonResult DeleteAdminRules(Int32 usersId)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(usersId == SessionManager.GetUserId() || usersId == 1 ? null : ServiceLocator.Current.GetInstance<IUserService>().DeleteAdminRules(usersId).ToString());
            }
            else
            {
                return Json(null);
            }
        }

        /// <summary>
        /// заблокировать пользователя
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public JsonResult BlockUser(Int32 usersId)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(usersId == SessionManager.GetUserId() ? null : ServiceLocator.Current.GetInstance<IUserService>().BlockUser(usersId).ToString());
            }
            else
            {
                return Json(null);
            }
        }

        /// <summary>
        /// разблокировать пользователя
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public JsonResult UnBlockUser(Int32 usersId)
        {
            if (ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(SessionManager.GetUserId()) == "Admin")
            {
                return Json(usersId == SessionManager.GetUserId() ? null : ServiceLocator.Current.GetInstance<IUserService>().UnBlockUser(usersId).ToString());
            }
            else
            {
                return Json(null);
            }
        }

        #endregion
    }
}
