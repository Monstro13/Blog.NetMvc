using System.Data.Entity;
using Framework.UnitOfWork;
using Model.Context;
using Microsoft.Practices.Unity;
using Model.Models;
using Repositories;
using Repositories.Interfaces;

namespace iBlog.UnitOfWork
{
    public class BlogUnitOfWorkFactory : UnitOfWorkFactoryBase
    {
        public BlogUnitOfWorkFactory()
        {
            Container.RegisterType<DbContext, BlogContext>(new InjectionConstructor());
            Container.RegisterType<IUserRepository, UserRepository>();
            Container.RegisterType<IUserCategoryRepository, UserCategoryRepository>();
            Container.RegisterType<IPostRepository, PostRepository>();
            Container.RegisterType<IKommentarRepository, KommentarRepository>();
            Container.RegisterType<IAttachFileRepository, AttachFileRepository>();
            Container.RegisterType<ILinkRepository, LinkRepository>();
            Container.RegisterType<IHashTagRepository, HashTagRepository>();
            Container.RegisterType<IFileTypeRepository, FileTypeRepository>();
            Container.RegisterType<IRubricRepository, RubricRepository>();
            Container.RegisterType<IContactRepository, ContactRepository>();
            Container.RegisterType<IGroupContactRepository, GroupContactRepository>();
            Container.RegisterType<IRatingPostRepository, RatingPostRepository>();
            Container.RegisterType<IRatingKommentarRepository, RatingKommentarRepository>();
            Container.RegisterType<IBlackListRepository, BlackListRepository>();
            Container.RegisterType<IAnswerRepository, AnwerRepository>();
        }
    }
}