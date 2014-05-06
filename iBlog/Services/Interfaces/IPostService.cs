using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models.LkpEnums;
using Repositories.DTO;

namespace Services.Interfaces
{
    public interface IPostService
    {
        /// <summary>
        /// достать 10 постов по критериям
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <param name="page"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        IEnumerable<PostInfo> Get10Posts(String searchPattern = "", Int32 page = 1, Int32 currentUserId = 0);
        /// <summary>
        /// изменить рейтинг новости и соответственно пользователю
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        Boolean ChangeRating(int postId, int userId, string command);
        /// <summary>
        /// взять пост по ид
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        PostInfo GetPostById(int newsId);
        /// <summary>
        /// изменить рейтинг комментарию и соответсвенно пользователю
        /// </summary>
        /// <param name="idKomment"></param>
        /// <param name="userId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        Boolean ChangeKommentRating(int idKomment, int userId, string command);
        /// <summary>
        /// добавить комментарий к новости
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="idPost"></param>
        /// <param name="komment"></param>
        /// <param name="answerId"></param>
        /// <returns></returns>
        InfoKomment AddKomment(int userid, int idPost, string komment, string answerId = "-1");
        /// <summary>
        /// проверить возможность изменения содержимого комментария
        /// </summary>
        /// <param name="idKomment"></param>
        /// <param name="getUserId"></param>
        /// <returns></returns>
        Boolean ChangeKommentarIsPossible(int idKomment, int getUserId);
        /// <summary>
        /// если есть возможность то удаляется комментарий, помечается как удаленный
        /// </summary>
        /// <param name="idKomment"></param>
        /// <returns></returns>
        Boolean DeleteKomment(int idKomment);
        /// <summary>
        /// восстановить комментарий
        /// </summary>
        /// <param name="idKomment"></param>
        /// <returns></returns>
        InfoKomment RecoveryKommentar(int idKomment);
        /// <summary>
        /// изменить содержимое комментария
        /// </summary>
        /// <param name="kommentarId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Boolean ChangeKommentarText(int kommentarId, string message);
        /// <summary>
        /// добавить ответ к комментарию
        /// </summary>
        /// <param name="idKomment"></param>
        /// <param name="answerText"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Int32 AddAnswer(int idKomment, string answerText, int userId);
        /// <summary>
        /// добавить новость
        /// </summary>
        /// <param name="info"></param>
        /// <param name="getUserId"></param>
        /// <returns></returns>
        bool AddPost(PostInfoToAddPost info, int getUserId);
        /// <summary>
        /// взять все дилоги пользователя
        /// </summary>
        /// <param name="getUserId"></param>
        /// <returns></returns>
        IEnumerable<InfoDialog> GetAllDialogs(int getUserId);
        /// <summary>
        /// получить сообщения по конкретному диалогу
        /// </summary>
        /// <param name="getUserId"></param>
        /// <param name="companionId"></param>
        /// <param name="newsId"></param>
        /// <returns></returns>
        Correspondence GetCorrespondence(int getUserId, int companionId, int newsId);
        /// <summary>
        /// взять новости для личного кабинета
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <param name="page"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        IEnumerable<PostInfo> Get10PostsForCurrentUser(String searchPattern = "", Int32 page = 1, Int32 currentUserId = 0);
        /// <summary>
        /// взять новости по фильтру для администрирования 
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <param name="page"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<AdminPostInfo> GetPostsByFilter(string searchPattern = "", int page = 1, NewsFilter filter = NewsFilter.all);
        /// <summary>
        /// пометить новость как спам
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        Boolean SpamNews(int newsId);
        /// <summary>
        /// подтвердить новость
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        Boolean ConfirmNews(int newsId);
    }
}
