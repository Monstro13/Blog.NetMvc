using Framework.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;

namespace Framework.UnitOfWork
{
    public class ReadWriteUnitOfWork : UnitOfWorkBase, IReadWriteUnitOfWork
    {
        public ReadWriteUnitOfWork(IUnityContainer container) : base(container) { }

        public void Commit() 
        {
            if (Context != null)
            {
                Context.SaveChanges();
            }
        }

        public void Rollback()
        { 

        }
    }
}
