using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        internal MainContext context;
        internal DbSet<TEntity> dbSet;

        public BaseRepository(MainContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
            this.dbSet.Load();
        }

        public virtual IQueryable<TEntity> All
        {
            get
            {
                return dbSet;
            }
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity Insert(TEntity entity)
        {
            return dbSet.Add(entity).Entity;
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            try
            {
                dbSet.Attach(entityToUpdate);
            }
            catch { }
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public void SetStateModified(TEntity entity)
        {
            context.Entry<TEntity>(entity).State = EntityState.Modified;
        }

    }
}
