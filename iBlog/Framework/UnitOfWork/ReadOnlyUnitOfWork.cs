using Framework.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;

namespace Framework.UnitOfWork
{
    public class ReadOnlyUnitOfWork : UnitOfWorkBase, IReadOnlyUnitOfWork
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadonlyUnitOfWork" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ReadOnlyUnitOfWork(IUnityContainer container) : base(container) { }
    }
}
