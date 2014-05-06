namespace Framework.UnitOfWork.Interfaces
{
    public interface IReadWriteUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Commits current unit of work.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks current unit of work.
        /// </summary>
        void Rollback();
    }
}
