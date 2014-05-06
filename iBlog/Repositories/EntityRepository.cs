using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Linq;
using Model.Context;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityRepository<T> : IRepository<T> where T : class
    {
        protected BlogContext Context;
        protected DbSet<T> DbSet;

        #region constructors

        /// <summary>
        /// Конструктор с установлением текущего контекста и выемкой сущностей определенного типа
        /// </summary>
        /// <param name="context"></param>
        public EntityRepository(BlogContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<T>();
        }

        #endregion

        #region public methods

        /// <summary>
        /// Возврат всего набора сущностей в контексте
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll()
        {
            return DbSet;
        }

        /// <summary>
        /// Возврат поднабора сущностей по заданному критерию поиска
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        /// <summary>
        /// Добавление новой сущности в выбранный набор
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }

        /// <summary>
        /// Обновление существующей сущности в выбранном наборе
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Удаление сущности из набора
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        #endregion       
    }
}
