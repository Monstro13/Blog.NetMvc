using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Practices.ServiceLocation;
using Model.Helpers;
using Repositories.DTO;
using Services.Interfaces;
using iBlog.Models;

namespace iBlog.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// вывод новостей на главную страницу используя фильтрацию
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(String searchPattern = "", Int32 page = 1)
        {
            SetRoleAndName();

            if (page < 0) page = 1;

            var news = ServiceLocator.Current.GetInstance<IPostService>().Get10Posts(searchPattern, page, SessionManager.GetUserId());
            if (!news.Any())
            {
                page = page - 1;
                news = ServiceLocator.Current.GetInstance<IPostService>().Get10Posts(searchPattern, page, SessionManager.GetUserId());
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
        /// переход на страницу новости
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        public ActionResult News(String newsId)
        {
            SetRoleAndName();

            var id = 0;
            if (Int32.TryParse(newsId, out id))
            {
                var model = ServiceLocator.Current.GetInstance<IPostService>().GetPostById(id);
                if (model != null)
                    return View(model);
                else
                    return RedirectToAction("Page404", "Apis");
            }
            else return RedirectToAction("Page404", "Apis");
        }

        /// <summary>
        /// переход на страницу добавления новости
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult AddNews()
        {
            SetRoleAndName();

            return View();
        }

        /// <summary>
        /// прием данных со страницы добавления новости
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult AddNews(NewNewsViewModal news)
        {
            if (ModelState.IsValid)
            {
                var info = new PostInfoToAddPost { FilePaths = new List<string>() };

                if (ServiceLocator.Current.GetInstance<IOptionService>().IsRubricExists(news.RubricId))
                {
                    info.RubricId = news.RubricId;
                }
                else return RedirectToAction("FailConfirm", "Apis");

                if (!(news.TitlePost.Length >= 200) && !(news.TitlePost.Length <= 0))
                {
                    info.TitlePost = news.TitlePost;
                }
                else return RedirectToAction("FailConfirm", "Apis");

                if (!(news.Text.Length >= 3000) && !(news.Text.Length <= 0))
                {
                    info.Text = news.Text;
                }
                else return RedirectToAction("FailConfirm", "Apis");

                if (news.HashTags != null) info.HashTags = news.HashTags.ToList();

                if (news.Links != null) info.Links = news.Links.ToList();

                info.ForSelfCategory = news.ForSelfCategory;

                foreach (var file in news.Files)
                {
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            var fileName = new Random().Next().ToString() + new Random().Next().ToString() +
                                           (file.FileName.Contains(".png") ? ".png" : ".jpg");
                            var path = Path.Combine(Server.MapPath("~/Images/Posts"), fileName);
                            file.SaveAs(path);

                            info.FilePaths.Add("/Images/Posts/" + fileName);
                        }
                    }
                }

                return !ServiceLocator.Current.GetInstance<IPostService>().AddPost(info, SessionManager.GetUserId()) ? RedirectToAction("FailConfirm", "Apis") : RedirectToAction("SuccessConfirm", "Apis");

            }
            else return RedirectToAction("FailConfirm", "Apis");
        }

        /// <summary>
        /// устновка прав и имен
        /// </summary>
        public void SetRoleAndName()
        {
            var userId = SessionManager.GetUserId();
            Session.Add("Role", ServiceLocator.Current.GetInstance<IUserService>().GetUserRole(userId));
            Session.Add("UserName", ServiceLocator.Current.GetInstance<IUserService>().GetUserNameById(userId));
        }
    }
}
