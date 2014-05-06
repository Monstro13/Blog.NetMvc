using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.UnitOfWork.Interfaces;
using Model.Models;
using Repositories.DTO;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class OptionService : ServiceBase, IOptionService
    {
        public OptionService(IUnitOfWorkFactory factory) : base(factory) { }

        public IEnumerable<TagInfo> GetTop20Tags()
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var tagRepo = uow.GetRepository<IHashTagRepository>();

                return tagRepo.GetAll().OrderByDescending(x => x.Weigth).Take(20).Select(x => new TagInfo(x)).ToList();
            }
        }

        public IEnumerable<RubricInfo> GetAllRubrics()
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var rubricRepo = uow.GetRepository<IRubricRepository>();

                return rubricRepo.GetAll().Select(x => new RubricInfo(x)).ToList();
            }
        }

        public IEnumerable<string> GetAllTags()
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var tagRepo = uow.GetRepository<IHashTagRepository>();

                return tagRepo.GetAll().Select(x => x.Title).ToList();
            }
        }

        public IEnumerable<string> GetAllLinks()
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var linkRepo = uow.GetRepository<ILinkRepository>();

                return linkRepo.GetAll().Select(x => x.Value).ToList();
            }
        }

        public bool IsRubricExists(int rubricId)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var rubricRepo = uow.GetRepository<IRubricRepository>();

                return rubricRepo.GetAll().FirstOrDefault(x => x.RubricId == rubricId) != null;
            }
        }

        public int GetUnWatchedMessages(int getUserId)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var answerRepo = uow.GetRepository<IAnswerRepository>();

                var user = userRepo.GetAll().FirstOrDefault(x => x.UserId == getUserId);
                return user != null ? answerRepo.GetAll().Count(x => x.WhomUserId == getUserId && !x.IsWatched) : 0;
            }
        }

        public IEnumerable<ContactInfo> GetAllContacts()
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                return uow.GetRepository<IContactRepository>().GetAll().Select(x => new ContactInfo(x)).ToList();
            }
        }

        public bool SetType(int contactId, string text)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var contactRepo = uow.GetRepository<IContactRepository>();

                var contact = contactRepo.GetAll().FirstOrDefault(x => x.ContactId == contactId);
                if (contact != null)
                {
                    if (contactRepo.GetAll().Where(x => x.ContactId != contactId).FirstOrDefault(x => x.Type.ToLower().Trim() == text.Trim().ToLower()) == null)
                    {
                        try
                        {
                            contact.Type = text;
                            contactRepo.Update(contact);
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

        public bool SetValue(int contactId, string text)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var contactRepo = uow.GetRepository<IContactRepository>();

                var contact = contactRepo.GetAll().FirstOrDefault(x => x.ContactId == contactId);
                if (contact != null)
                {
                    try
                    {
                        contact.Value = text;
                        contactRepo.Update(contact);
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

        public bool DeleteContact(int id)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var contactRepo = uow.GetRepository<IContactRepository>();

                var contact = contactRepo.GetAll().FirstOrDefault(x => x.ContactId == id);
                if (contact != null)
                {
                    try
                    {
                        contactRepo.Delete(contact);
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

        public bool AddContact(string type, string value)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var contactRepo = uow.GetRepository<IContactRepository>();

                if (TypeIsNotExists(type.ToLower().Trim()))
                {
                    var contact = new Contact
                        {
                            Type = type,
                            Value = value,
                            GroupId = 1
                        };

                    try
                    {
                        contactRepo.Add(contact);
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

        public IEnumerable<CategoryInfo> GetAllCategories()
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                return uow.GetRepository<IUserCategoryRepository>().GetAll().Select(x => new CategoryInfo(x)).ToList();
            }
        }

        public bool SetTitle(int id, string text)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var categoryRepo = uow.GetRepository<IUserCategoryRepository>();

                var category = categoryRepo.GetAll().FirstOrDefault(x => x.UserCategoryId == id);
                if (category != null)
                {
                    if (categoryRepo.GetAll().Where(x => x.UserCategoryId != id).FirstOrDefault(x => x.Title.ToLower().Trim() == text.Trim().ToLower()) == null)
                    {
                        try
                        {
                            category.Title = text;
                            categoryRepo.Update(category);
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

        public bool SetCode(int id, string text)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var categoryRepo = uow.GetRepository<IUserCategoryRepository>();

                var category = categoryRepo.GetAll().FirstOrDefault(x => x.UserCategoryId == id);
                if (category != null)
                {
                    try
                    {
                        category.Code = text;
                        categoryRepo.Update(category);
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

        public bool DeleteCategory(int id)
        {
            if (id == 1) return false;
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var categoryRepo = uow.GetRepository<IUserCategoryRepository>();

                var category = categoryRepo.GetAll().FirstOrDefault(x => x.UserCategoryId == id);
                if (category != null)
                {
                    try
                    {
                        var userRepo = uow.GetRepository<IUserRepository>();
                        foreach (var user in userRepo.GetAll().Where(x => x.UserCategoryId == id))
                        {
                            user.UserCategoryId = 1;
                            userRepo.Update(user);
                        }

                        var postRepo = uow.GetRepository<IPostRepository>();
                        foreach (var post in postRepo.GetAll().Where(x=>x.UserCategoryId == id))
                        {
                            post.UserCategoryId = 1;
                            postRepo.Update(post);
                        }

                        categoryRepo.Delete(category);
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

        public bool AddCategory(string title, string code, string desc)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var categoryRepo = uow.GetRepository<IUserCategoryRepository>();

                if (TitleIsNotExists(title.ToLower().Trim()))
                {
                    var category = new UserCategory
                    {
                        Title = title,
                        Code = code,
                        Description = desc
                    };

                    try
                    {
                        categoryRepo.Add(category);
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

        public bool SetTitleRubric(int id, string text)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var rubricRepo = uow.GetRepository<IRubricRepository>();

                var rubric = rubricRepo.GetAll().FirstOrDefault(x => x.RubricId == id);
                if (rubric != null)
                {
                    if (rubricRepo.GetAll().Where(x => x.RubricId != id).FirstOrDefault(x => x.Title.ToLower().Trim() == text.Trim().ToLower()) == null)
                    {
                        try
                        {
                            rubric.Title = text;
                            rubricRepo.Update(rubric);
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

        public bool DeleteRubric(int id)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var rubricRepo = uow.GetRepository<IRubricRepository>();

                var rubric = rubricRepo.GetAll().FirstOrDefault(x => x.RubricId == id);
                if (rubric != null)
                {
                    var postRepo = uow.GetRepository<IPostRepository>();
                    if (postRepo.GetAll().Any(x => x.RubricId == id)) return false;

                    try
                    {
                        rubricRepo.Delete(rubric);
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

        public bool AddRubric(string title, string desc)
        {
            using (var uow = Factory.CreateReadWriteUnitOfWork())
            {
                var rubricRepo = uow.GetRepository<IRubricRepository>();

                if (RubricIsNotExists(title.ToLower().Trim()))
                {
                    var rubric = new Rubric
                    {
                        Title = title,
                        Description = desc
                    };

                    try
                    {
                        rubricRepo.Add(rubric);
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

        public Boolean TypeIsNotExists(String type)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                return
                    uow.GetRepository<IContactRepository>()
                       .GetAll()
                       .FirstOrDefault(x => x.Type.ToLower().Trim() == type) == null;
            }
        }

        public Boolean TitleIsNotExists(String title)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                return
                    uow.GetRepository<IUserCategoryRepository>()
                       .GetAll()
                       .FirstOrDefault(x => x.Title.ToLower().Trim() == title) == null;
            }
        }

        public Boolean RubricIsNotExists(String title)
        {
            using (var uow = Factory.CreateReadOnlyUnitOfWork())
            {
                return
                    uow.GetRepository<IRubricRepository>()
                       .GetAll()
                       .FirstOrDefault(x => x.Title.ToLower().Trim() == title) == null;
            }
        }
    }
}
