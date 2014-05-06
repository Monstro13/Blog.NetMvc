using System;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Practices.ServiceLocation;
using Model.Helpers;
using Model.Models.LkpEnums;
using Newtonsoft.Json;
using Repositories.DTO;
using Services.Interfaces;

namespace iBlog.Controllers
{
    public class ApisController : Controller
    {
        #region users api

        /// <summary>
        /// проверка существования эмайла
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IsEmailExists(String mail)
        {
            return Json(ServiceLocator.Current.GetInstance<IUserService>().IsEmailExists(mail.ToLower().Trim()).ToString());
        }

        /// <summary>
        /// проверка существования логина
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IsLoginExists(String login)
        {
            return Json(ServiceLocator.Current.GetInstance<IUserService>().IsLoginExists(login.ToLower().Trim()).ToString());
        }

        /// <summary>
        /// регистрация пользователя
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegisterUser(String info)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(info);

            if (userInfo != null)
            {
                var result = ServiceLocator.Current.GetInstance<IUserService>().RegisterUser(userInfo);

                if (result.Code == UserCreationCode.Ok)
                {
                    var randomNumber = new Random().Next();
                    var code = (String.Format("{0}", randomNumber)).ComputeHash();

                    Helper.SendMailWithConfirmCode(result.UserEmail.Trim(), code);
                    Session.Add("ConfirmationUserId", result.UserId);
                    Session.Add("ConfirmationCode", code);

                    return Json("/Apis/Confirmation");
                }
                else return Json(result.ToString());
            }
            else return Json("Error");
        }

        /// <summary>
        /// вход на сайт
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="remember"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SignIn(String login, String password, String remember)
        {
            var userId = ServiceLocator.Current.GetInstance<IUserService>().IsUserExists(login.ToLower().Trim(), password);

            if (userId != null)
            {
                FormsAuthentication.SetAuthCookie(userId, Boolean.Parse(remember));
                Session.Add("UserId", userId);

                return Json("Ok");
            }

            return Json("Error");
        }

        //выход с сайта
        [HttpPost]
        public JsonResult Exit()
        {
            FormsAuthentication.SignOut();
            return Json("Ok");
        }

        /// <summary>
        /// запуск страницы подтверждения регистрации
        /// </summary>
        /// <returns></returns>
        public ActionResult Confirmation()
        {
            return View();
        }

        /// <summary>
        /// запуск страницы ошибки подтверждения
        /// </summary>
        /// <returns></returns>
        public ActionResult FailConfirm()
        {
            return View();
        }

        /// <summary>
        /// запуск страницы удачного подтверждения
        /// </summary>
        /// <returns></returns>
        public ActionResult SuccessConfirm()
        {
            return View();
        }

        /// <summary>
        /// активировать пользователя для свободного входа
        /// </summary>
        /// <param name="confirmCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ActivateUser(String confirmCode)
        {
            if (confirmCode == Session["ConfirmationCode"] as string)
            {
                ServiceLocator.Current.GetInstance<IUserService>().ActivateUser(Session["ConfirmationUserId"] as string);

                FormsAuthentication.SetAuthCookie(Session["ConfirmationUserId"] as string, false);
                Session.Add("UserId", Session["ConfirmationUserId"] as string);

                return View("SuccessConfirm");
            }
            else
            {
                FormsAuthentication.SignOut();
                return View("FailConfirm");
            }
        }

        /// <summary>
        /// запуск активации пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActivateAccount(String email, String login, String password)
        {
            var userId = ServiceLocator.Current.GetInstance<IUserService>().FindUserIdByInfo(email.ToLower().Trim(), login.ToLower().Trim(), password);

            if (userId != null)
            {
                var randomNumber = new Random().Next();
                var code = (String.Format("{0}", randomNumber)).ComputeHash();

                Helper.SendMailWithConfirmCode(email.Trim(), code);
                Session.Add("ConfirmationUserId", userId.ToString());
                Session.Add("ConfirmationCode", code);

                return Json("/Apis/Confirmation");
            }
            else return Json("/Apis/FailConfirm");
        }

        /// <summary>
        /// восстановление пароля
        /// </summary>
        /// <param name="email"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecoveryPassword(String email, String login)
        {
            var newPassword = ServiceLocator.Current.GetInstance<IUserService>().SetNewPassword(email.ToLower().Trim(), login.ToLower().Trim());

            if (newPassword != null)
            {
                Helper.SendMailWithNewPassword(email.Trim(), newPassword);
                return Json("Ok");
            }
            return Json("Error");
        }

        #endregion

        /// <summary>
        /// подгрузить топ 20 тегов
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoadTags()
        {
            return Json(new { Tags = ServiceLocator.Current.GetInstance<IOptionService>().GetTop20Tags() });
        }

        /// <summary>
        /// подгрузить контакты разработчика
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoadContacts()
        {
            var dict = ServiceLocator.Current.GetInstance<IMetaInformationService>().GetInformation("Main");

            return Json(new { Contacts = dict.Keys, Values = dict.Values });
        }

        /// <summary>
        /// подгрузить рубрики 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult LoadRubrics()
        {
            return Json(new { Rubrics = ServiceLocator.Current.GetInstance<IOptionService>().GetAllRubrics() });
        }

        /// <summary>
        /// запрос на изменение рейтинга новости
        /// </summary>
        /// <param name="command"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult ChangeRating(String command, String postId)
        {
            var userId = SessionManager.GetUserId();
            var idPost = 0;

            if (Int32.TryParse(postId, out idPost))
            {
                return Json(ServiceLocator.Current.GetInstance<IPostService>().ChangeRating(idPost, userId, command).ToString());
            }
            else return Json("false");
        }

        /// <summary>
        /// изменить рейтинг комментария
        /// </summary>
        /// <param name="command"></param>
        /// <param name="kommentId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult ChangeKommentRating(String command, String kommentId)
        {
            var userId = SessionManager.GetUserId();
            var idKomment = 0;

            if (Int32.TryParse(kommentId, out idKomment))
            {
                return Json(ServiceLocator.Current.GetInstance<IPostService>().ChangeKommentRating(idKomment, userId, command).ToString());
            }
            else return Json("false");
        }

        /// <summary>
        /// запрос краткой информации для других пользователей о пользователе
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUserInfoForUser(String login)
        {
            var result = ServiceLocator.Current.GetInstance<IUserService>().GetUserInfoForUser(login.ToLower().Trim());

            if (result != null)
            {
                return Json(new
                    {
                        UserId = result.UserId,
                        Name = result.Name,
                        Sex = result.Sex,
                        Rating = result.Rating,
                        TimeOnSite = result.TimeOnSite,
                        CountPost = result.CountOfPost,
                        CountKomment = result.CountOfKomment
                    });
            }
            else return Json("Error");
        }

        /// <summary>
        /// добавить в игнор лист "черный лист"
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult AddToBlackList(String userId)
        {
            var id = 0;
            if (Int32.TryParse(userId, out id))
            {
                var currentUserId = SessionManager.GetUserId();
                return Json(ServiceLocator.Current.GetInstance<IUserService>().AddToBlackList(currentUserId, id).ToString());
            }
            return Json("Error");
        }

        /// <summary>
        /// добавление комментария к новости
        /// </summary>
        /// <param name="komment"></param>
        /// <param name="postId"></param>
        /// <param name="answerId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult AddKomment(String komment, String postId, String answerId = "-1")
        {
            var idPost = 0;
            if (Int32.TryParse(postId, out idPost))
            {
                var userid = SessionManager.GetUserId();
                var info = ServiceLocator.Current.GetInstance<IPostService>().AddKomment(userid, idPost, komment, answerId);

                return Json(new
                    {
                        Login = info.UserLogin,
                        Date = info.DateCreation,
                        Id = info.KommentarId
                    });
            }
            else return Json("Error");
        }

        /// <summary>
        /// проверить возможость изменения комментария
        /// </summary>
        /// <param name="kommentarId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangeIsPossible(String kommentarId)
        {
            var idKomment = 0;
            if (Int32.TryParse(kommentarId, out idKomment))
            {
                return Json(ServiceLocator.Current.GetInstance<IPostService>().ChangeKommentarIsPossible(idKomment, SessionManager.GetUserId()).ToString());
            }
            else return Json("false");
        }

        /// <summary>
        /// удалить комментарий, пометить как удаленный
        /// </summary>
        /// <param name="kommentarId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult DeleteKomment(String kommentarId)
        {
            var idKomment = 0;
            if (Int32.TryParse(kommentarId, out idKomment))
            {
                return Json(ServiceLocator.Current.GetInstance<IPostService>().DeleteKomment(idKomment).ToString());
            }
            else return Json("false");
        }

        /// <summary>
        /// восстановить удаленный комментарий
        /// </summary>
        /// <param name="kommentId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult RecoveryKomment(String kommentId)
        {
            var idKomment = 0;
            if (Int32.TryParse(kommentId, out idKomment))
            {
                var info = ServiceLocator.Current.GetInstance<IPostService>().RecoveryKommentar(idKomment);
                return Json(new
                    {
                        Login = info.UserLogin,
                        Date = info.DateCreation,
                        Id = info.KommentarId,
                        Message = info.Message
                    });
            }
            else return Json("false");
        }

        /// <summary>
        /// изменить содержимое коментария
        /// </summary>
        /// <param name="kommentarId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult ChangeKommentar(String kommentarId, String message)
        {
            var idKomment = 0;
            if (Int32.TryParse(kommentarId, out idKomment))
            {
                return Json(ServiceLocator.Current.GetInstance<IPostService>().ChangeKommentarText(idKomment, message).ToString());
            }
            else return Json("false");
        }

        /// <summary>
        /// добавить ответ к комметарию
        /// </summary>
        /// <param name="kommentId"></param>
        /// <param name="answerText"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult AddAnswer(String kommentId, String answerText)
        {
            var idKomment = 0;
            if (Int32.TryParse(kommentId, out idKomment))
            {
                return Json(ServiceLocator.Current.GetInstance<IPostService>().AddAnswer(idKomment, answerText, SessionManager.GetUserId()).ToString());
            }
            else return Json("false");
        }

        /// <summary>
        /// загрузить все тэги
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult LoadAllTags()
        {
            return Json(new { Tags = ServiceLocator.Current.GetInstance<IOptionService>().GetAllTags() });
        }

        /// <summary>
        /// загрузить все ссылки
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult LoadAllLinks()
        {
            return Json(new { Links = ServiceLocator.Current.GetInstance<IOptionService>().GetAllLinks() });
        }

        /// <summary>
        /// получить непрочитанные сообщения диалогов пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UnWatchedMessages()
        {
            return Json(User.Identity.IsAuthenticated ? ServiceLocator.Current.GetInstance<IOptionService>().GetUnWatchedMessages(SessionManager.GetUserId()) : 0);
        }

        /// <summary>
        /// вернуть страницу 404
        /// </summary>
        /// <returns></returns>
        public ActionResult Page404()
        {
            return View();
        }
    }
}
