using System;

namespace Framework.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for current unit of work.
        /// </summary>
        /// <typeparam name="TRepository">The type of the repository.</typeparam>
        /// <returns>
        /// Repository instance
        /// </returns>
        TRepository GetRepository<TRepository>() where TRepository : class;
    }
}
