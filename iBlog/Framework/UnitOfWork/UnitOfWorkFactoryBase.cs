using Framework.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;

namespace Framework.UnitOfWork
{
    public abstract class UnitOfWorkFactoryBase : IUnitOfWorkFactory
    {
       /// <summary>
        /// Container which created current instance of factory
        /// </summary>
        protected readonly IUnityContainer Container = new UnityContainer();

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkFactoryBase" /> class.
        /// </summary>
        protected UnitOfWorkFactoryBase()
        {
            Container.RegisterType<IReadWriteUnitOfWork, ReadWriteUnitOfWork>();
            Container.RegisterType<IReadOnlyUnitOfWork, ReadOnlyUnitOfWork>();
            Container.RegisterInstance(Container);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkFactoryBase" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        protected UnitOfWorkFactoryBase(IUnityContainer container)
        {
            Container = container;
        }


        /// <summary>
        /// Creates the read write unit of work.
        /// </summary>
        /// <returns></returns>
        public IReadWriteUnitOfWork CreateReadWriteUnitOfWork()
        {
            return Container.Resolve<IReadWriteUnitOfWork>();
        }

        /// <summary>
        /// Creates the readonly unit of work.
        /// </summary>
        /// <returns>
        ///   <see cref="IReadonlyUnitOfWork" /> instance
        /// </returns>
        public IReadOnlyUnitOfWork CreateReadOnlyUnitOfWork()
        {
            return Container.Resolve<IReadOnlyUnitOfWork>();
        }
    }

}
