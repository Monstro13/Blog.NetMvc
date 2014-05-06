using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Repositories.DTO;
using Services.Interfaces;
using iBlog.Models;

namespace iBlog.Controllers
{
    public class ManageController : Controller
    {
        /// <summary>
        /// получить все диалоги пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult GetAllDialogs()
        {
            return Json(new { Dialogs = ServiceLocator.Current.GetInstance<IPostService>().GetAllDialogs(SessionManager.GetUserId()) });
        }

        /// <summary>
        /// взять конкретный диалог
        /// </summary>
        /// <param name="userId">текущий юзер</param>
        /// <param name="companionId">его собеседник</param>
        /// <param name="newsId">новость, по поводу которой возник диалог</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult GetDialog(String userId, String companionId, String newsId)
        {
            var uid = 0;

            if (Int32.TryParse(userId, out uid))
            {
                var cid = 0;
                if (Int32.TryParse(companionId, out cid))
                {
                    var nid = 0;
                    if (Int32.TryParse(newsId, out nid))
                    {
                        if (uid == SessionManager.GetUserId())
                        {
                            return Json(String.Format("/Manage/GetDialog?companionId={0}&newsId={1}", cid, nid));
                        }
                        else return Json("/Apis/FailConfirm");
                    }
                    else return Json("/Apis/FailConfirm");
                }
                else return Json("/Apis/FailConfirm");
            }
            else return Json("/Apis/FailConfirm");
        }

        /// <summary>
        /// переход на страницу диалога между пользователями
        /// </summary>
        /// <param name="companionId"></param>
        /// <param name="newsId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult GetDialog(Int32 companionId, Int32 newsId)
        {
            var model = ServiceLocator.Current.GetInstance<IPostService>()
                                      .GetCorrespondence(SessionManager.GetUserId(), companionId, newsId);

            return model != null ? (ActionResult)View("Dialog", model) : RedirectToAction("Page404", "Apis");
        }

        /// <summary>
        /// переход в личный кабинет
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Privat(String searchPattern = "", Int32 page = 1)
        {
            SetRoleAndName();

            if (page < 0) page = 1;

            var news = ServiceLocator.Current.GetInstance<IPostService>().Get10PostsForCurrentUser(searchPattern, page, SessionManager.GetUserId());
            if (!news.Any())
            {
                page = page - 1;
                news = ServiceLocator.Current.GetInstance<IPostService>().Get10PostsForCurrentUser(searchPattern, page, SessionManager.GetUserId());
            }

            var newsModel = new MainViewModel()
            {
                News = (List<PostInfo>)news,
                CurrentPageNumder = page,
                SearchPattern = searchPattern == "" ? null : searchPattern
            };

            return View(newsModel);
        }

        /// <summary>
        /// получить игнор-лист
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult GetIgnorList()
        {
            return Json(new { Ignors = ServiceLocator.Current.GetInstance<IUserService>().GetIgnorList(SessionManager.GetUserId()) });
        }

        /// <summary>
        /// удаление из игнор-листа
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ignorUserId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult DeleteFromIgnor(Int32 userId, Int32 ignorUserId)
        {
            return Json(ServiceLocator.Current.GetInstance<IUserService>().DeleteFromIgnor(userId, ignorUserId).ToString());
        }

        /// <summary>
        /// проверить старый пароль на правильность при смене
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult ControlOldPass(String old)
        {
            return Json(ServiceLocator.Current.GetInstance<IUserService>().ControlPass(SessionManager.GetUserId(), old).ToString());
        }

        /// <summary>
        /// устаносить новый пароль пользователю
        /// </summary>
        /// <param name="old"></param>
        /// <param name="newPs"></param>
        /// <param name="confirmPs"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult SetNewPassword(String old, String newPs, String confirmPs)
        {
            return Json(ServiceLocator.Current.GetInstance<IUserService>().ChangePassword(SessionManager.GetUserId(), old, newPs, confirmPs).ToString());
        }

        /// <summary>
        /// получить все категории пользователей
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult GetAllCategories()
        {
            return Json(new { Categories = ServiceLocator.Current.GetInstance<IUserService>().GetAllCategories() });
        }

        /// <summary>
        /// изменение категории пользователя
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult ChangeCategory(Int32 catId, String code = "1")
        {
            return Json(ServiceLocator.Current.GetInstance<IUserService>().ChangeCategory(SessionManager.GetUserId(), catId, code).ToString());
        }

        /// <summary>
        /// установка прав и имен
        /// </summary>
        public void SetRoleAndName()
        {
            var userId = SessionManager.GetUserId();
            Session.Add("Role", ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(userId));
            Session.Add("UserName", ServiceLocator.Current.GetInstance<IUserService>().GetUserNameById(userId));
        }
    }
}
