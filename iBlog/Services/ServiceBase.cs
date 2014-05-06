using Framework.UnitOfWork.Interfaces;

namespace Services
{
    public class ServiceBase
    {
        protected IUnitOfWorkFactory Factory;

        public ServiceBase(IUnitOfWorkFactory factory)
        {
            this.Factory = factory;
        }
    }
}
