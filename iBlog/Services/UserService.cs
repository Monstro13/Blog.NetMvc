using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.UnitOfWork.Interfaces;
using Model.Helpers;
using Model.Models;
using Model.Models.LkpEnums;
using Repositories.DTO;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class UserService : ServiceBase, IUserService
    {
        public UserService(IUnitOfWorkFactory factory) : base(factory) { }

        /// <summary>
        /// получить роль пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserRole(int userId)
        {
            Int32 id = userId;

            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();

                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == id);
                if (user != null)
                {
                    if (user.IsAdmin) return "Admin";
                    return "User";
                }

                return "Anon";
            }
        }

        /// <summary>
        /// занят ли данный эмайл
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsEmailExists(string email)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();

                var usersEmail = userRepo.GetAll().Select(x => x.Email.ToLower().Trim()).ToList();
                return usersEmail.Contains(email);
            }
        }

        /// <summary>
        /// занят ли данный логин
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool IsLoginExists(string login)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();

                var usersLogin = userRepo.GetAll().Select(x => x.Login.ToLower().Trim()).ToList();
                return usersLogin.Contains(login);
            }
        }

        /// <summary>
        /// регистрация нового пользователя
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ResultRegister RegisterUser(UserInfo userInfo)
        {
            var login = userInfo.Login;
            var email = userInfo.Email;

            if (userInfo.Password == userInfo.confirmPassword)
            {
                if (!IsEmailExists(email))
                {
                    if (!IsLoginExists(login))
                    {
                        using (var uow = Factory.CreateReadWriteUnitOfWork())
                        {
                            var userRepo = uow.GetRepository<IUserRepository>();
                            var categoryRepo = uow.GetRepository<IUserCategoryRepository>();

                            var categoryId = categoryRepo.GetAll().FirstOrDefault(x => x.Title == "general").UserCategoryId;

                            var user = new User
                                {
                                    FirstName = userInfo.FirstName,
                                    SecondName = userInfo.SecondName,
                                    Email = userInfo.Email,
                                    Login = userInfo.Login,
                                    Password = userInfo.Password.ComputeHash(),
                                    Sex = userInfo.Sex == "1",
                                    DateRegistration = DateTime.Now,
                                    UserCategoryId = categoryId,
                                    Karma = 0,
                                    IsActivate = false,
                                    IsBlocked = false,
                                    IsAdmin = false,
                                    IsDeleted = false
                                };

                            try
                            {
                                userRepo.Add(user);
                                uow.Commit();

                                User first = userRepo.GetAll().FirstOrDefault(x => x.Email == userInfo.Email && x.Login == userInfo.Login);
                                if (first != null)
                                {
                                    string retUserId = first.UserId.ToString();
                                    string retUserEmail = first.Email;

                                    return new ResultRegister { UserId = retUserId, UserEmail = retUserEmail, Code = UserCreationCode.Ok };
                                }
                                else return new ResultRegister { UserId = "0", UserEmail = "0", Code = UserCreationCode.Error };
                            }
                            catch (Exception)
                            {
                                return new ResultRegister { UserId = "0", UserEmail = "0", Code = UserCreationCode.Error };
                            }
                        }
                    }
                    else
                        return new ResultRegister { UserId = "0", UserEmail = "0", Code = UserCreationCode.LoginExists };
                }
                else
                    return new ResultRegister { UserId = "0", UserEmail = "0", Code = UserCreationCode.EmailExists };
            }
            else return new ResultRegister { UserId = "0", UserEmail = "0", Code = UserCreationCode.DiffPass };
        }

        /// <summary>
        /// проверить существование комбинации пароля и логина
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string IsUserExists(string login, string password)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                password = password.ComputeHash();

                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.Login.ToLower().Trim() == login && x.Password == password && x.IsActivate && !x.IsDeleted && !x.IsBlocked);

                return user != null ? user.UserId.ToString() : null;
            }
        }

        /// <summary>
        /// получить имя пользователя по идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserNameById(int userId)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == userId);
                if (user != null)
                    return user.Login;
                else return null;
            }
        }

        /// <summary>
        /// активировать пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ActivateUser(string userId)
        {
            var id = 0;
            if (Int32.TryParse(userId, out id))
            {
                using (var uow = Factory.CreateReadWriteUnitOfWork())
                {
                    var userRepo = uow.GetRepository<IUserRepository>();

                    var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == id);

                    try
                    {
                        if (user != null)
                        {
                            user.IsActivate = true;
                            userRepo.Update(user);
                        }
                        uow.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// узнать идентификатор пользователя по уникальной информации
        /// </summary>
        /// <param name="email"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int? FindUserIdByInfo(string email, string login, string password)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                password = password.ComputeHash();

                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.Login.ToLower().Trim() == login && x.Password == password && x.Email.ToLower().Trim() == email);

                int? userId = null;
                if (user != null)
                    userId = user.UserId;

                return userId;
            }
        }

        /// <summary>
        /// установить новый пароль пользователю
        /// </summary>
        /// <param name="email"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public string SetNewPassword(string email, string login)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();

                var user = userRepo.GetAll().FirstOrDefault(x => x.Login.ToLower().Trim() == login && x.Email.ToLower().Trim() == email);

                if (user != null)
                {
                    var newPassword = new Random().Next().ToString();

                    try
                    {
                        user.Password = newPassword.ComputeHash();
                        userRepo.Update(user);
                        uow.Commit();
                        return newPassword;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// возвращение краткой информации о пользователе другим пользователям
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public InfoUserForUser GetUserInfoForUser(string login)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();

                return new InfoUserForUser(userRepo.GetAll().FirstOrDefault(x => x.Login.ToLower().Trim() == login));
            }
        }

        /// <summary>
        /// Добавление пользователя в игнор
        /// </summary>
        /// <param name="currentUserId">кто добавляет</param>
        /// <param name="id">кого добавляет</param>
        /// <returns></returns>
        public bool AddToBlackList(int currentUserId, int id)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var blackRepo = uow.GetRepository<IBlackListRepository>();
                var blackList =
                    blackRepo.GetAll().FirstOrDefault(x => x.SubjectUserId == currentUserId && x.ObjectUserId == id);

                if (blackList == null)
                {
                    try
                    {
                        blackList = new BlackList
                            {
                                SubjectUserId = currentUserId,
                                ObjectUserId = id
                            };

                        blackRepo.Add(blackList);
                        uow.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else return false;
            }
        }

        public IEnumerable<IgnorInfo> GetIgnorList(int getUserId)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var blackRepo = uow.GetRepository<IBlackListRepository>();
                var userRepo = uow.GetRepository<IUserRepository>();

                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == getUserId);
                if (user != null)
                {
                    var list = blackRepo.GetAll().Where(x => x.SubjectUserId == getUserId).Select(x => new IgnorInfo(x)).ToList();
                    list.ForEach(x =>
                        {
                            var objectUser = userRepo.GetAll().FirstOrDefault(m => m.UserId == x.IgnorId);
                            if (objectUser != null)
                                x.Login = objectUser.Login;
                        });

                    return list;
                }
                else return new List<IgnorInfo>();
            }
        }

        public bool DeleteFromIgnor(int userId, int ignorUserId)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var blackRepo = uow.GetRepository<IBlackListRepository>();

                var paar = blackRepo.GetAll().FirstOrDefault(x => x.SubjectUserId == userId && x.ObjectUserId == ignorUserId);

                if (paar != null)
                {
                    try
                    {
                        blackRepo.Delete(paar);
                        uow.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else return false;
            }
        }

        public bool ControlPass(int getUserId, string old)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == getUserId);
                if (user != null)
                {
                    return user.Password == old.ComputeHash();
                }
                else return false;
            }
        }

        public ChangePasswordCode ChangePassword(int getUserId, string old, string newPs, string confirmPs)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == getUserId);
                if (user != null)
                {
                    if (ControlPass(getUserId, old))
                    {
                        if (newPs == confirmPs)
                        {
                            try
                            {
                                user.Password = newPs.ComputeHash();
                                userRepo.Update(user);
                                uow.Commit();
                                return ChangePasswordCode.Ok;
                            }
                            catch (Exception)
                            {
                                return ChangePasswordCode.Error;
                            }
                        }
                        else return ChangePasswordCode.newAndConfirmIsNotEquals;
                    }
                    else return ChangePasswordCode.oldIsBad;
                }
                else return ChangePasswordCode.Error;
            }
        }

        public IEnumerable<CategoryInfo> GetAllCategories()
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var userCatRepo = uow.GetRepository<IUserCategoryRepository>();
                return userCatRepo.GetAll().Select(x => new CategoryInfo(x)).ToList();
            }
        }

        public bool ChangeCategory(int getUserId, int catId, string code)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var catRepo = uow.GetRepository<IUserCategoryRepository>();

                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == getUserId);
                if (user != null)
                {
                    if (catId != 1)
                    {
                        try
                        {
                            var category = catRepo.GetAll().FirstOrDefault(x => x.UserCategoryId == catId);
                            if (category != null)
                            {
                                if (category.Code == code)
                                {
                                    user.UserCategoryId = catId;
                                    userRepo.Update(user);

                                    category.Code = new Random().Next().ToString(CultureInfo.InvariantCulture).ComputeHash();
                                    catRepo.Update(category);

                                    uow.Commit();
                                    return true;
                                }
                                else return false;
                            }
                            else return false;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        try
                        {
                            user.UserCategoryId = 1;
                            userRepo.Update(user);
                            uow.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                }
                else return false;
            }
        }

        public IEnumerable<InfoUserForAdmin> GetUsersByFilter(string searchPattern = "", int page = 1, UsersFilter filter = UsersFilter.all)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();

                var filterUsers = userRepo.GetAll();
                if (filter == UsersFilter.unConfirm)
                {
                    filterUsers = filterUsers.Where(x => !x.IsActivate && !x.IsDeleted && !x.IsBlocked);
                }
                else if (filter == UsersFilter.banned)
                {
                    filterUsers = filterUsers.Where(x => x.IsDeleted || x.IsBlocked);
                }

                var users = filterUsers.OrderByDescending(x => x.DateRegistration)
                                       .Skip((page - 1) * 8)
                                       .Take(8)
                                       .Select(x => new InfoUserForAdmin(x)).ToList();

                if (searchPattern != "")
                {
                    var pattern = searchPattern.ToLower().Trim();
                    users = users.Where(x => x.Sname.ToLower().Trim().Contains(pattern) ||
                                             x.Fname.ToLower().Trim().Contains(pattern) ||
                                             x.Login.ToLower().Trim().Contains(pattern) ||
                                             x.DateRegistration.ToLower().Trim().Contains(pattern) ||
                                             x.Email.ToLower().Trim().Contains(pattern) ||
                                             x.Sex.ToLower().Trim().Contains(pattern) ||
                                             x.Category.ToLower().Trim().Contains(pattern)).ToList();
                }

                return users;
            }
        }

        public bool SetAdmin(int usersId)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == usersId);
                if (user != null)
                {
                    if (!user.IsAdmin && !user.IsBlocked && !user.IsDeleted && user.IsActivate)
                    {
                        try
                        {
                            user.IsAdmin = true;
                            userRepo.Update(user);
                            uow.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                    else return false;
                }
                else return false;
            }
        }

        public bool DeleteAdminRules(int usersId)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == usersId);
                if (user != null)
                {
                    if (user.IsActivate && user.IsAdmin)
                    {
                        if (userRepo.GetAll().Where(x => x.UserId != usersId).Any(x => x.IsAdmin))
                        {
                            try
                            {
                                user.IsAdmin = false;
                                userRepo.Update(user);
                                uow.Commit();
                                return true;
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;

            }
        }

        public bool BlockUser(int usersId)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == usersId);
                if (user != null)
                {
                    if (user.IsActivate && !user.IsBlocked && !user.IsDeleted)
                    {
                        try
                        {
                            user.IsBlocked = true;
                            user.IsDeleted = true;
                            userRepo.Update(user);
                            uow.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                    else return false;
                }
                else return false;

            }
        }

        public bool UnBlockUser(int usersId)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == usersId);
                if (user != null)
                {
                    if (user.IsActivate && user.IsBlocked && user.IsDeleted)
                    {
                        try
                        {
                            user.IsBlocked = false;
                            user.IsDeleted = false;
                            userRepo.Update(user);
                            uow.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                    else return false;
                }
                else return false;

            }
        }
    }
}
