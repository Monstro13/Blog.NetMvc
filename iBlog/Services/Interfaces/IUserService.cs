using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models.LkpEnums;
using Repositories.DTO;

namespace Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// взять роль пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        String GetUserRole(int userId);
        /// <summary>
        /// проверить занятость эмайла
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Boolean IsEmailExists(string email);
        /// <summary>
        /// проверить занятость логина
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        Boolean IsLoginExists(string login);
        /// <summary>
        /// регистрация пользователя
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ResultRegister RegisterUser(UserInfo userInfo);
        /// <summary>
        /// проверить существование пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        String IsUserExists(string login, string password);
        /// <summary>
        /// получить имя пользователя по его идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        String GetUserNameById(int userId);
        /// <summary>
        /// активировать доступ пользователю
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Boolean ActivateUser(string userId);
        /// <summary>
        /// найти идентификатор пользователя по уникальной информации
        /// </summary>
        /// <param name="email"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Int32? FindUserIdByInfo(string email, string login, string password);
        /// <summary>
        /// установить новый пароль пользователю
        /// </summary>
        /// <param name="email"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        String SetNewPassword(string email, string login);
        /// <summary>
        /// получить информацию о пользователе
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        InfoUserForUser GetUserInfoForUser(string login);
        /// <summary>
        /// добавить в черный лист
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Boolean AddToBlackList(int currentUserId, int id);
        /// <summary>
        /// получить игнор-лист пользователя
        /// </summary>
        /// <param name="getUserId"></param>
        /// <returns></returns>
        IEnumerable<IgnorInfo> GetIgnorList(int getUserId);
        /// <summary>
        /// удалить пользователя из игнор-листа
        /// </summary>
        /// <param name="userId">кто удаляет</param>
        /// <param name="ignorUserId">кого</param>
        /// <returns></returns>
        Boolean DeleteFromIgnor(int userId, int ignorUserId);
        /// <summary>
        /// проверит правильность старого пароля
        /// </summary>
        /// <param name="getUserId"></param>
        /// <param name="old"></param>
        /// <returns></returns>
        Boolean ControlPass(int getUserId, string old);
        /// <summary>
        /// изменить пароль пользователю
        /// </summary>
        /// <param name="getUserId"></param>
        /// <param name="old"></param>
        /// <param name="newPs"></param>
        /// <param name="confirmPs"></param>
        /// <returns></returns>
        ChangePasswordCode ChangePassword(int getUserId, string old, string newPs, string confirmPs);
        /// <summary>
        /// взять все категории
        /// </summary>
        /// <returns></returns>
        IEnumerable<CategoryInfo> GetAllCategories();
        /// <summary>
        /// сменить категорию пользователю
        /// </summary>
        /// <param name="getUserId"></param>
        /// <param name="catId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Boolean ChangeCategory(int getUserId, int catId, string code);
        /// <summary>
        /// взять инфу о юзерах для администрирования 
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <param name="page"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<InfoUserForAdmin> GetUsersByFilter(string searchPattern = "", int page = 1, UsersFilter filter = UsersFilter.all);
        /// <summary>
        /// Назначить админа
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        Boolean SetAdmin(int usersId);
        /// <summary>
        /// лишить прав админа
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        Boolean DeleteAdminRules(int usersId);
        /// <summary>
        /// заблокировать пользователя 
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        Boolean BlockUser(int usersId);
        /// <summary>
        /// разюлокировать пользователя
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        Boolean UnBlockUser(int usersId);
    }
}
