using System;
using System.Data.Entity;
using Microsoft.Practices.Unity;

namespace Framework.UnitOfWork
{
    public abstract class UnitOfWorkBase : IDisposable
    {
        /// <summary>
        /// Gets the current entity framework context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        protected DbContext Context { get; private set; }

        /// <summary>
        /// Container which was used to create current instance
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// <c>true</c> if unit was disposed
        /// </summary>
        private Boolean _disposed;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="container">The container.</param>
        protected UnitOfWorkBase(IUnityContainer container)
        {
            _container = container.CreateChildContainer();

            if (container.IsRegistered<DbContext>())
            {
                Context = container.Resolve<DbContext>();
                _container.RegisterInstance(Context);
                _container.RegisterInstance(Context.GetType(), Context);
            }
        }

        /// <summary>
        /// Gets the repository for current unit of work.
        /// </summary>
        /// <typeparam name="TRepository">The type of the repository.</typeparam>
        /// <returns>
        /// Repository instance
        /// </returns>
        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return _container.Resolve<TRepository>();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (!_disposed)
            {
                if (Context != null)
                {
                    Context.Dispose();
                }

                _disposed = true;
            }
        }
    }
    
}
