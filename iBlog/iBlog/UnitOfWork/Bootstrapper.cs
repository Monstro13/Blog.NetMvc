using Framework.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;
using Model.Wrapper;
using Microsoft.Practices.ServiceLocation;
using Services;
using Services.Interfaces;

namespace iBlog.UnitOfWork
{
    public class Bootstrapper
    {
        public static void Init()
        {
            InitDb();
            InitDependencies();
        }

        public static void InitDb()
        {
            BlogInitWrapper.Init();
        }

        public static void InitDependencies()
        {
            var container = new UnityContainer();
            container.RegisterType<IUnitOfWorkFactory, BlogUnitOfWorkFactory>();
            container.RegisterType<IMetaInformationService, MetaInformationService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IOptionService, OptionService>();
            container.RegisterType<IPostService, PostService>();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }
    }
}
