namespace Framework.UnitOfWork.Interfaces
{
    public interface IUnitOfWorkFactory
    {

        /// <summary>
        /// Creates the read write unit of work.
        /// </summary>
        /// <returns></returns>
        IReadWriteUnitOfWork CreateReadWriteUnitOfWork();

        /// <summary>
        /// Creates the readonly unit of work.
        /// </summary>
        /// <returns></returns>
        IReadOnlyUnitOfWork CreateReadOnlyUnitOfWork();
    }
}
