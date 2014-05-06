using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.UnitOfWork.Interfaces;
using Model.Models;
using Model.Models.LkpEnums;
using Repositories.DTO;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class PostService : ServiceBase, IPostService
    {
        public PostService(IUnitOfWorkFactory factory) : base(factory) { }

        /// <summary>
        /// взять 10 постов в соответствии с критерием
        /// </summary>
        /// <param name="searchPattern">по шаблону</param>
        /// <param name="page">для определенной страницы</param>
        /// <param name="currentUserId">для определенного пользователя, "черный список"</param>
        /// <returns></returns>
        public IEnumerable<PostInfo> Get10Posts(string searchPattern = "", int page = 1, int currentUserId = 0)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var blackRepo = uow.GetRepository<IBlackListRepository>();
                var blackList = blackRepo.GetAll().Where(x => x.SubjectUserId == currentUserId).Select(x => x.ObjectUserId).ToList();

                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == currentUserId) ?? new User { UserCategoryId = 1 };

                if (searchPattern == "")
                {
                    var postRepo = uow.GetRepository<IPostRepository>();

                    var retList = postRepo.GetAll()
                                   .Where(x => x.IsConfirm && !x.IsDeleted && !x.IsSpam && !blackList.Contains(x.UserId) && (x.UserCategoryId == user.UserCategoryId || x.UserCategoryId == 1))
                                   .OrderByDescending(x => x.DateCreation)
                                   .Skip((page - 1) * 10)
                                   .Take(10)
                                   .Select(x => new PostInfo(x))
                                   .ToList();
                    return retList;
                }
                else
                {
                    var postRepo = uow.GetRepository<IPostRepository>();

                    if (searchPattern.Contains("#"))
                    {
                        searchPattern = searchPattern.Replace("#", "");

                        var firstList = postRepo.GetAll()
                                        .Where(x => x.IsConfirm && !x.IsDeleted && !x.IsSpam && !blackList.Contains(x.UserId) && (x.UserCategoryId == user.UserCategoryId || x.UserCategoryId == 1))
                                        .OrderByDescending(x => x.DateCreation)
                                        .Skip((page - 1) * 10)
                                        .Take(10)
                                        .Select(x => new PostInfo(x))
                                        .ToList();

                        var retList = firstList.Where(x => x.HashTags.Select(h => h.Title).Contains(searchPattern)).ToList();
                        return retList;
                    }
                    else
                    {
                        var retList = postRepo.GetAll()
                                        .Where(x => x.IsConfirm && !x.IsDeleted && !x.IsSpam && x.Title.Contains(searchPattern) && !blackList.Contains(x.UserId) && (x.UserCategoryId == user.UserCategoryId || x.UserCategoryId == 1))
                                        .OrderByDescending(x => x.DateCreation)
                                        .Skip((page - 1) * 10)
                                        .Take(10)
                                        .Select(x => new PostInfo(x))
                                        .ToList();
                        return retList;
                    }
                }
            }
        }

        /// <summary>
        /// выставление рейтинга новости
        /// </summary>
        /// <param name="postId">новость для оценки</param>
        /// <param name="userId">тот, кто ставит оценку</param>
        /// <param name="command">команда</param>
        /// <returns></returns>
        public bool ChangeRating(int postId, int userId, string command)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var ratingRepo = uow.GetRepository<IRatingPostRepository>();
                var postRepo = uow.GetRepository<IPostRepository>();
                var userRepo = uow.GetRepository<IUserRepository>();

                var post = postRepo.GetAll().FirstOrDefault(x => x.PostId == postId);
                var ratingPost = ratingRepo.GetAll().FirstOrDefault(x => x.PostId == postId && x.UserId == userId);

                if (ratingPost != null)
                {
                    var ratingValue = ratingPost.Value;
                    if (command == "up")
                    {
                        switch (ratingValue)
                        {
                            case RatingCode.Like:
                                {
                                    return false;
                                }
                            case RatingCode.Neutral:
                                {
                                    ratingValue = RatingCode.Like;
                                    ratingPost.Value = ratingValue;
                                    try
                                    {
                                        ratingRepo.Update(ratingPost);

                                        post.User.Karma++;
                                        userRepo.Update(post.User);

                                        post.Rating++;
                                        postRepo.Update(post);

                                        uow.Commit();
                                        return true;
                                    }
                                    catch (Exception)
                                    {
                                        return false;
                                    }
                                }
                            case RatingCode.Dislike:
                                {
                                    ratingValue = RatingCode.Neutral;
                                    ratingPost.Value = ratingValue;
                                    try
                                    {
                                        ratingRepo.Update(ratingPost);

                                        post.User.Karma++;
                                        userRepo.Update(post.User);

                                        post.Rating++;
                                        postRepo.Update(post);

                                        uow.Commit();
                                        return true;
                                    }
                                    catch (Exception)
                                    {
                                        return false;
                                    }
                                }
                            default:
                                {
                                    return false;
                                }
                        }
                    }
                    else
                    {
                        switch (ratingValue)
                        {
                            case RatingCode.Like:
                                {
                                    ratingValue = RatingCode.Neutral;
                                    ratingPost.Value = ratingValue;
                                    try
                                    {
                                        ratingRepo.Update(ratingPost);

                                        post.User.Karma--;
                                        userRepo.Update(post.User);

                                        post.Rating--;
                                        postRepo.Update(post);

                                        uow.Commit();
                                        return true;
                                    }
                                    catch (Exception)
                                    {
                                        return false;
                                    }
                                }
                            case RatingCode.Neutral:
                                {
                                    ratingValue = RatingCode.Dislike;
                                    ratingPost.Value = ratingValue;
                                    try
                                    {
                                        ratingRepo.Update(ratingPost);

                                        post.User.Karma--;
                                        userRepo.Update(post.User);

                                        post.Rating--;
                                        postRepo.Update(post);

                                        uow.Commit();
                                        return true;
                                    }
                                    catch (Exception)
                                    {
                                        return false;
                                    }
                                }
                            case RatingCode.Dislike:
                                {
                                    return false;
                                }
                            default:
                                {
                                    return false;
                                }
                        }
                    }
                }
                else
                {
                    ratingPost = new RatingPost
                        {
                            PostId = postId,
                            UserId = userId,
                            Value = command == "up" ? RatingCode.Like : RatingCode.Dislike,
                        };

                    try
                    {
                        if (ratingPost.Value == RatingCode.Like)
                        {
                            post.Rating++;
                            post.User.Karma++;
                        }
                        else
                        {
                            post.Rating--;
                            post.User.Karma--;
                        }

                        userRepo.Update(post.User);
                        postRepo.Update(post);
                        ratingRepo.Add(ratingPost);
                        uow.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// взять инфо о посте по идентификатору
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        public PostInfo GetPostById(int newsId)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var model = uow.GetRepository<IPostRepository>().GetAll().FirstOrDefault(x => x.PostId == newsId);
                return model != null ? new PostInfo(model) : null;
            }
        }

        /// <summary>
        /// изменить рейтинг комментарию. так же как и новости. в диапазоне трех значений
        /// </summary>
        /// <param name="idKomment">ид коммента</param>
        /// <param name="userId">кто изменяет</param>
        /// <param name="command">команда</param>
        /// <returns></returns>
        public bool ChangeKommentRating(int idKomment, int userId, string command)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var ratingRepo = uow.GetRepository<IRatingKommentarRepository>();
                var kommentarRepo = uow.GetRepository<IKommentarRepository>();
                var userRepo = uow.GetRepository<IUserRepository>();

                var kommentar = kommentarRepo.GetAll().FirstOrDefault(x => x.KommentarId == idKomment);
                var ratingPost = ratingRepo.GetAll().FirstOrDefault(x => x.KommentarId == idKomment && x.UserId == userId);

                if (ratingPost != null)
                {
                    var ratingValue = ratingPost.Value;
                    if (command == "up")
                    {
                        switch (ratingValue)
                        {
                            case RatingCode.Like:
                                {
                                    return false;
                                }
                            case RatingCode.Neutral:
                                {
                                    ratingValue = RatingCode.Like;
                                    ratingPost.Value = ratingValue;
                                    try
                                    {
                                        ratingRepo.Update(ratingPost);

                                        kommentar.User.Karma++;
                                        userRepo.Update(kommentar.User);

                                        kommentar.Rating++;
                                        kommentar.IsChanged = true;
                                        kommentarRepo.Update(kommentar);

                                        uow.Commit();
                                        return true;
                                    }
                                    catch (Exception)
                                    {
                                        return false;
                                    }
                                }
                            case RatingCode.Dislike:
                                {
                                    ratingValue = RatingCode.Neutral;
                                    ratingPost.Value = ratingValue;
                                    try
                                    {
                                        ratingRepo.Update(ratingPost);

                                        kommentar.User.Karma++;
                                        userRepo.Update(kommentar.User);

                                        kommentar.Rating++;
                                        kommentar.IsChanged = true;
                                        kommentarRepo.Update(kommentar);

                                        uow.Commit();
                                        return true;
                                    }
                                    catch (Exception)
                                    {
                                        return false;
                                    }
                                }
                            default:
                                {
                                    return false;
                                }
                        }
                    }
                    else
                    {
                        switch (ratingValue)
                        {
                            case RatingCode.Like:
                                {
                                    ratingValue = RatingCode.Neutral;
                                    ratingPost.Value = ratingValue;
                                    try
                                    {
                                        ratingRepo.Update(ratingPost);

                                        kommentar.User.Karma--;
                                        userRepo.Update(kommentar.User);

                                        kommentar.Rating--;
                                        kommentar.IsChanged = true;
                                        kommentarRepo.Update(kommentar);

                                        uow.Commit();
                                        return true;
                                    }
                                    catch (Exception)
                                    {
                                        return false;
                                    }
                                }
                            case RatingCode.Neutral:
                                {
                                    ratingValue = RatingCode.Dislike;
                                    ratingPost.Value = ratingValue;
                                    try
                                    {
                                        ratingRepo.Update(ratingPost);

                                        kommentar.User.Karma--;
                                        userRepo.Update(kommentar.User);

                                        kommentar.Rating--;
                                        kommentar.IsChanged = true;
                                        kommentarRepo.Update(kommentar);

                                        uow.Commit();
                                        return true;
                                    }
                                    catch (Exception)
                                    {
                                        return false;
                                    }
                                }
                            case RatingCode.Dislike:
                                {
                                    return false;
                                }
                            default:
                                {
                                    return false;
                                }
                        }
                    }
                }
                else
                {
                    ratingPost = new RatingKommentar
                    {
                        KommentarId = idKomment,
                        UserId = userId,
                        Value = command == "up" ? RatingCode.Like : RatingCode.Dislike,
                    };

                    try
                    {
                        if (ratingPost.Value == RatingCode.Like)
                        {
                            kommentar.Rating++;
                            kommentar.User.Karma++;
                            kommentar.IsChanged = true;
                        }
                        else
                        {
                            kommentar.Rating--;
                            kommentar.User.Karma--;
                            kommentar.IsChanged = true;
                        }

                        userRepo.Update(kommentar.User);
                        kommentarRepo.Update(kommentar);
                        ratingRepo.Add(ratingPost);
                        uow.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// добавить новый комментарий
        /// </summary>
        /// <param name="userid">кто добавляет</param>
        /// <param name="idPost">к какому посту</param>
        /// <param name="komment">что добавляет</param>
        /// <param name="answerId">если это ответ на комментарий, то фиксируем</param>
        /// <returns></returns>
        public InfoKomment AddKomment(int userid, int idPost, string komment, string answerId = "-1")
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var komentRepo = uow.GetRepository<IKommentarRepository>();
                var userRepo = uow.GetRepository<IUserRepository>();
                var answerRepo = uow.GetRepository<IAnswerRepository>();

                var kommentMaxId = 0;
                kommentMaxId = komentRepo.GetAll().Any() ? komentRepo.GetAll().Select(x => x.KommentarId).Max() : 1;

                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == userid);

                var kommentar = new Kommentar
                    {
                        KommentarId = kommentMaxId + 1,
                        DateCreation = DateTime.Now,
                        IsConfirm = true,
                        Message = komment,
                        UserId = userid,
                        PostId = idPost,
                        Rating = 0,
                        IsDeleted = false,
                        IsSpam = false,
                        IsChanged = false
                    };

                var idanswer = 0;
                Int32.TryParse(answerId, out idanswer);
                if (idanswer > 0)
                {
                    var answer = answerRepo.GetAll().FirstOrDefault(x => x.Id == idanswer);
                    if (answer != null)
                    {
                        answer.KommentarId = kommentar.KommentarId;
                        answerRepo.Update(answer);
                    }
                }

                komentRepo.Add(kommentar);
                uow.Commit();

                return new InfoKomment { DateCreation = kommentar.DateCreation.ToString(), UserLogin = user.Login, KommentarId = kommentar.KommentarId };
            }
        }

        /// <summary>
        /// проверка возможности редактировать свой комментарий
        /// </summary>
        /// <param name="idKomment">ид коммента</param>
        /// <param name="getUserId">ид авторизованного пользователя</param>
        /// <returns></returns>
        public bool ChangeKommentarIsPossible(int idKomment, int getUserId)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var kommentarRepo = uow.GetRepository<IKommentarRepository>();

                var kommentar = kommentarRepo.GetAll().FirstOrDefault(x => x.KommentarId == idKomment && x.UserId == getUserId);

                if (kommentar != null)
                {
                    if (!kommentar.IsChanged)
                        return true;
                    else return false;
                }
                else return false;
            }
        }

        /// <summary>
        /// удалить комметарий, то есть пометить как удаленный
        /// </summary>
        /// <param name="idKomment"></param>
        /// <returns></returns>
        public bool DeleteKomment(int idKomment)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var kommentarRepo = uow.GetRepository<IKommentarRepository>();
                var answerRepo = uow.GetRepository<IAnswerRepository>();

                var kommentar = kommentarRepo.GetAll().FirstOrDefault(x => x.KommentarId == idKomment);
                var answer = answerRepo.GetAll().FirstOrDefault(x => x.KommentarId == idKomment);
                if (kommentar != null && !kommentar.IsDeleted && !kommentar.IsChanged)
                {
                    try
                    {
                        kommentar.IsDeleted = true;
                        kommentarRepo.Update(kommentar);

                        if (answer != null)
                        {
                            answer.IsDeleted = true;
                            answerRepo.Update(answer);
                        }

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

        /// <summary>
        /// восстановить комментарий
        /// </summary>
        /// <param name="idKomment"></param>
        /// <returns></returns>
        public InfoKomment RecoveryKommentar(int idKomment)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var kommentarRepo = uow.GetRepository<IKommentarRepository>();
                var answerRepo = uow.GetRepository<IAnswerRepository>();

                var komment = kommentarRepo.GetAll().FirstOrDefault(x => x.KommentarId == idKomment);
                var answer = answerRepo.GetAll().FirstOrDefault(x => x.KommentarId == idKomment);

                if (komment != null)
                {
                    komment.IsDeleted = false;
                    kommentarRepo.Update(komment);

                    if (answer != null)
                    {
                        answer.IsDeleted = false;
                        answerRepo.Update(answer);
                    }

                    uow.Commit();

                    return new InfoKomment(komment);

                }
                else return new InfoKomment();
            }
        }

        /// <summary>
        /// изменить содержимое комментария если тот не изменен
        /// </summary>
        /// <param name="kommentarId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ChangeKommentarText(int kommentarId, string message)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var kommentarRepo = uow.GetRepository<IKommentarRepository>();
                var answerRepo = uow.GetRepository<IAnswerRepository>();
                var kommentar = kommentarRepo.GetAll().FirstOrDefault(x => x.KommentarId == kommentarId);
                var answer = answerRepo.GetAll().FirstOrDefault(x => x.KommentarId == kommentarId);

                if (kommentar != null && !kommentar.IsDeleted && !kommentar.IsChanged)
                {
                    try
                    {
                        kommentar.Message = message;
                        kommentarRepo.Update(kommentar);

                        if (answer != null)
                        {
                            answer.Text = message;
                            answerRepo.Update(answer);
                        }

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

        /// <summary>
        /// добавить ответ к комментарию
        /// </summary>
        /// <param name="idKomment">к какому коменту идет ответ</param>
        /// <param name="answerText">что идет</param>
        /// <param name="userId">кто дает ответ</param>
        /// <returns></returns>
        public Int32 AddAnswer(int idKomment, string answerText, int userId)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var answerRepo = uow.GetRepository<IAnswerRepository>();
                var kommentarRepo = uow.GetRepository<IKommentarRepository>();

                var kommentar = kommentarRepo.GetAll().FirstOrDefault(x => x.KommentarId == idKomment);

                if (kommentar != null)
                {
                    try
                    {
                        var answerMaxId = 0;
                        answerMaxId = answerRepo.GetAll().Any() ? answerRepo.GetAll().Select(x => x.Id).Max() : 1;

                        var answer = new Answer()
                            {
                                Id = answerMaxId + 1,
                                KommentarId = -1,
                                PostId = kommentar.Post.PostId,
                                WhomUserId = kommentar.User.UserId,
                                WhoUserId = userId,
                                Source = kommentar.Message,
                                Text = answerText,
                                Date = DateTime.Now,
                                IsWatched = false,
                                IsDeleted = false
                            };

                        kommentar.IsChanged = true;
                        kommentarRepo.Update(kommentar);

                        answerRepo.Add(answer);
                        uow.Commit();
                        return answer.Id;
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
                else return -1;
            }
        }

        public bool AddPost(PostInfoToAddPost info, int getUserId)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var postRepo = uow.GetRepository<IPostRepository>();

                var fileRepo = uow.GetRepository<IAttachFileRepository>();
                var files = new List<AttachFile>();
                var fileIdMax = fileRepo.GetAll().Select(x => x.AttachFileId).Max();

                try
                {
                    foreach (var file in info.FilePaths.Select(path => new AttachFile()
                        {
                            AttachFileId = fileIdMax++,
                            FilePath = path,
                            FileTypeId = path.Contains(".png") ? 1 : 2
                        }))
                    {
                        files.Add(file);
                        fileRepo.Add(file);
                    }

                    uow.Commit();
                }
                catch (Exception)
                {
                    return false;
                }

                var tagRepo = uow.GetRepository<IHashTagRepository>();
                var tags = new List<HashTag>();
                var tagIdMax = tagRepo.GetAll().Select(x => x.HashTagId).Max();

                try
                {
                    if (info.HashTags != null)
                    {
                        foreach (var hashTag in info.HashTags)
                        {
                            var tag =
                                tagRepo.GetAll()
                                       .FirstOrDefault(x => x.Title.ToLower().Trim() == hashTag.ToLower().Trim());
                            if (tag != null)
                            {
                                tag.Weigth++;
                                tagRepo.Update(tag);
                                tags.Add(tag);
                            }
                            else
                            {
                                tag = new HashTag()
                                    {
                                        HashTagId = tagIdMax++,
                                        Weigth = 1,
                                        Title = hashTag
                                    };
                                tags.Add(tag);
                            }
                        }
                    }

                    uow.Commit();
                }
                catch (Exception)
                {
                    return false;
                }

                var linkRepo = uow.GetRepository<ILinkRepository>();
                var links = new List<Link>();
                var linkIdMax = linkRepo.GetAll().Select(x => x.LinkId).Max();

                try
                {
                    if (info.Links != null)
                    {
                        links.AddRange(
                            info.Links.Select(
                                linkItem =>
                                (Link)
                                (linkRepo.GetAll()
                                         .FirstOrDefault(x => x.Value.ToLower().Trim() == linkItem.ToLower().Trim()) ??
                                 new Link()
                                     {
                                         LinkId = linkIdMax++,
                                         Value = linkItem
                                     })));
                    }
                    uow.Commit();
                }
                catch (Exception)
                {
                    return false;
                }

                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == getUserId);

                if (user != null)
                {
                    try
                    {
                        var post = new Post()
                            {
                                PostId = postRepo.GetAll().Select(x => x.PostId).Max() + 1,
                                DateCreation = DateTime.Now,
                                RubricId = info.RubricId,
                                Title = info.TitlePost,
                                Message = info.Text,
                                Files = files,
                                HashTags = tags,
                                Links = links,
                                UserId = getUserId,
                                UserCategoryId = info.ForSelfCategory ? user.UserCategoryId : 1,
                                Rating = 0,
                                IsConfirm = false,
                                IsDeleted = false,
                                IsSpam = false
                            };

                        postRepo.Add(post);
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

        public IEnumerable<InfoDialog> GetAllDialogs(int getUserId)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == getUserId);
                if (user != null)
                {
                    var answerRepo = uow.GetRepository<IAnswerRepository>();
                    var dialogs =
                        answerRepo.GetAll()
                                  .Where(x => x.WhoUserId == getUserId || x.WhomUserId == getUserId)
                                  .Select(x => new InfoDialog(x, getUserId))
                                  .ToList();

                    var postRepo = uow.GetRepository<IPostRepository>();
                    foreach (var infoDialog in dialogs)
                    {
                        var title = postRepo.GetAll().FirstOrDefault(x => x.PostId == infoDialog.NewsId).Title;
                        var login = userRepo.GetAll().FirstOrDefault(x => x.UserId == infoDialog.CompanionId).Login;
                        infoDialog.NewsTitle = title;
                        infoDialog.LoginCompanion = login;
                    }

                    var retDialogs = new List<InfoDialog>();
                    foreach (var infoDialog in dialogs)
                    {
                        var us1 = infoDialog.CurrentUserId;
                        var us2 = infoDialog.CompanionId;
                        var ps = infoDialog.NewsId;

                        if (retDialogs.Count(x => x.CurrentUserId == us1 && x.CompanionId == us2 && x.NewsId == ps) > 0) continue;
                        retDialogs.Add(infoDialog);
                    }

                    foreach (var infoDialog in retDialogs)
                    {
                        if (dialogs.FirstOrDefault(
                                x =>
                                x.CurrentUserId == infoDialog.CurrentUserId && x.CompanionId == infoDialog.CompanionId &&
                                x.NewsId == infoDialog.NewsId && x.IsWatched == false) != null)
                            infoDialog.IsWatched = false;
                    }

                    return retDialogs;
                }
                else return null;
            }
        }

        public Correspondence GetCorrespondence(int getUserId, int companionId, int newsId)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var answerRepo = uow.GetRepository<IAnswerRepository>();

                var messages = answerRepo.GetAll().Where(x => (x.WhoUserId == getUserId && x.WhomUserId == companionId && x.PostId == newsId)
                    || (x.WhomUserId == getUserId && x.WhoUserId == companionId && x.PostId == newsId)).OrderBy(x => x.Date).ToList();

                try
                {
                    messages.ForEach(x =>
                        {
                            x.IsWatched = true;
                            answerRepo.Update(x);
                        });
                    uow.Commit();
                }
                catch (Exception)
                {
                    return null;
                }

                var correspondence = new Correspondence();
                var postRepo = uow.GetRepository<IPostRepository>();
                var firstOrDefault = postRepo.GetAll().FirstOrDefault(x => x.PostId == newsId);
                if (firstOrDefault != null)
                {
                    var title = firstOrDefault.Title;
                    correspondence.PostId = newsId;
                    correspondence.PostTitle = title;

                    var kommentRepo = uow.GetRepository<IKommentarRepository>();
                    var msgs = messages.Select(message => new PostKommentar(kommentRepo.GetAll().FirstOrDefault(x => x.KommentarId == message.KommentarId))).ToList();

                    correspondence.Messages = msgs;
                    return correspondence;
                }
                else return null;
            }
        }

        public IEnumerable<PostInfo> Get10PostsForCurrentUser(string searchPattern = "", int page = 1, int currentUserId = 0)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == currentUserId);
                if (user != null)
                {
                    if (searchPattern == "")
                    {
                        var postRepo = uow.GetRepository<IPostRepository>();

                        var retList = postRepo.GetAll()
                                       .Where(x => x.IsConfirm && !x.IsDeleted && !x.IsSpam && x.UserId == currentUserId)
                                       .OrderByDescending(x => x.DateCreation)
                                       .Skip((page - 1) * 10)
                                       .Take(10)
                                       .Select(x => new PostInfo(x))
                                       .ToList();
                        return retList;
                    }
                    else
                    {
                        var postRepo = uow.GetRepository<IPostRepository>();

                        if (searchPattern.Contains("#"))
                        {
                            searchPattern = searchPattern.Replace("#", "");

                            var firstList = postRepo.GetAll()
                                            .Where(x => x.IsConfirm && !x.IsDeleted && !x.IsSpam && x.UserId == currentUserId)
                                            .OrderByDescending(x => x.DateCreation)
                                            .Skip((page - 1) * 10)
                                            .Take(10)
                                            .Select(x => new PostInfo(x))
                                            .ToList();

                            var retList = firstList.Where(x => x.HashTags.Select(h => h.Title).Contains(searchPattern)).ToList();
                            return retList;
                        }
                        else
                        {
                            var retList = postRepo.GetAll()
                                            .Where(x => x.IsConfirm && !x.IsDeleted && !x.IsSpam && x.Title.Contains(searchPattern) && x.UserId == currentUserId)
                                            .OrderByDescending(x => x.DateCreation)
                                            .Skip((page - 1) * 10)
                                            .Take(10)
                                            .Select(x => new PostInfo(x))
                                            .ToList();
                            return retList;
                        }
                    }
                }
                else return new List<PostInfo>();
            }
        }

        public IEnumerable<AdminPostInfo> GetPostsByFilter(string searchPattern = "", int page = 1, NewsFilter filter = NewsFilter.all)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var postRepo = uow.GetRepository<IPostRepository>();

                var filterPosts = postRepo.GetAll();
                if (filter == NewsFilter.unConfirm)
                {
                    filterPosts = filterPosts.Where(x => !x.IsConfirm && !x.IsDeleted && !x.IsSpam);
                }
                else if (filter == NewsFilter.banned)
                {
                    filterPosts = filterPosts.Where(x => x.IsDeleted || x.IsSpam);
                }

                var posts = filterPosts.OrderByDescending(x => x.DateCreation)
                                       .Skip((page - 1)*8)
                                       .Take(8)
                                       .Select(x => new AdminPostInfo(x)).ToList();

                if (searchPattern != "")
                {
                    var pattern = searchPattern.ToLower().Trim();
                    posts = posts.Where(x => x.UserLogin.ToLower().Trim().Contains(pattern) ||
                                             x.PostTitle.ToLower().Trim().Contains(pattern) ||
                                             x.Rubric.ToLower().Trim().Contains(pattern) ||
                                             x.DateCreation.ToLower().Trim().Contains(pattern)).ToList();
                }

                return posts;
            }
        }

        public bool SpamNews(int newsId)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var postRepo = uow.GetRepository<IPostRepository>();
                var post = postRepo.GetAll().FirstOrDefault(x => x.PostId == newsId);
                if (post != null)
                {
                    if (!post.IsSpam)
                    {
                        try
                        {
                            post.IsSpam = true;
                            post.IsDeleted = true;
                            postRepo.Update(post);
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

        public bool ConfirmNews(int newsId)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var postRepo = uow.GetRepository<IPostRepository>();
                var post = postRepo.GetAll().FirstOrDefault(x => x.PostId == newsId);
                if (post != null)
                {
                    if (!post.IsSpam && !post.IsDeleted && !post.IsConfirm)
                    {
                        try
                        {
                            post.IsConfirm = true;
                            postRepo.Update(post);
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
