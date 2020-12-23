using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessCore.DataAccess.Repository
{
    public abstract class RepositoryBase<TContext, TEntity> : IDisposable
        where TContext : DbContext
        where TEntity : class
    {
        protected TContext Context { get; private set; }
        protected DbSet<TEntity> DbSet => Context.Set<TEntity>();

        protected RepositoryBase(TContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Return all <see cref="TEntity"/>, including provided joins.
        /// </summary>
        /// <param name="includes">Navigation items.</param>
        /// <returns><see cref="TEntity"/> result set.</returns>
        public virtual IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.AsEnumerable();
        }

        /// <summary>
        /// Return all <see cref="TEntity"/>, including provided joins.
        /// </summary>
        /// <param name="includes">Navigation items.</param>
        /// <returns>Async <see cref="TEntity"/> result set.</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Return all <see cref="TEntity"/>, including provided joins, with the provided predicate.
        /// </summary>
        /// <param name="predicate">Filter for <see cref="TEntity"/> result set.</param>
        /// <param name="includes">Navigation items.</param>
        /// <returns><see cref="TEntity"/> result set.</returns>
        public virtual IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet.Where(predicate);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.AsEnumerable();
        }

        /// <summary>
        /// Return all <see cref="TEntity"/>, including provided joins, with the provided predicate.
        /// </summary>
        /// <param name="predicate">Filter for <see cref="TEntity"/> result set.</param>
        /// <param name="includes">Navigation items.</param>
        /// <returns>Async <see cref="TEntity"/> result set.</returns>
        public virtual async Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet.Where(predicate);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Return <see cref="TEntity"/> specified by the provided predicate.
        /// </summary>
        /// <param name="predicate">Filter for <see cref="TEntity"/></param>
        /// <param name="includes">Navigation items.</param>
        /// <returns><see cref="TEntity"/></returns>
        public virtual TEntity FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Return <see cref="TEntity"/> specified by the provided predicate.
        /// </summary>
        /// <param name="predicate">Filter for <see cref="TEntity"/></param>
        /// <param name="includes">Navigation items.</param>
        /// <returns>Async <see cref="TEntity"/></returns>
        public virtual async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Returns <see cref="TEntity"/> specified by the primary key.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns><see cref="TEntity"/></returns>
        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Returns <see cref="TEntity"/> specified by the primary key.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns>Async <see cref="TEntity"/></returns>
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await Task.Run(() => GetById(id));
        }

        /// <summary>
        /// Return <see cref="TResult"/>, based on provided selector and included joins.
        /// </summary>
        /// <typeparam name="TResult">Return type.</typeparam>
        /// <param name="selector">Filter for <see cref="TEntity"/></param>
        /// <param name="prop">Property to return.</param>
        /// <param name="includes">Navigation items.</param>
        /// <returns><see cref="TResult"/>.</returns>
        public TResult GetValue<TResult>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TResult>> prop, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = DbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            TResult result = query.Where(selector).Select(prop).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Return <see cref="TResult"/>, based on provided selector and included joins.
        /// </summary>
        /// <typeparam name="TResult">Return type.</typeparam>
        /// <param name="selector">Filter for <see cref="TEntity"/></param>
        /// <param name="prop">Property to return.</param>
        /// <param name="includes">Navigation items.</param>
        /// <returns>Async <see cref="TResult"/>.</returns>
        public Task<TResult> GetValueAsync<TResult>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TResult>> prop, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = DbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            Task<TResult> result = query.Where(selector).Select(prop).FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Create <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> to create.</param>
        /// <returns>Added <see cref="TEntity"/>.</returns>
        public virtual TEntity Add(TEntity entity)
        {
            EntityEntry<TEntity> addedEntity = DbSet.Add(entity);

            return addedEntity.Entity;
        }

        /// <summary>
        /// Create <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> to create.</param>
        /// <returns>Asymc added <see cref="TEntity"/>.</returns>
        public virtual Task<TEntity> AddAsync(TEntity entity)
        {
            return Task.Run(() => Add(entity));
        }

        /// <summary>
        /// Update <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> to update.</param>
        public virtual void Update(TEntity entity)
        {
            EntityEntry<TEntity> entry = Context.Entry(entity);
            entry.State = EntityState.Modified;
        }

        /// <summary>
        /// Update <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> to update.</param>
        public virtual Task UpdateAsync(TEntity entity)
        {
            return Task.Run(() => Update(entity));
        }

        /// <summary>
        /// Delete <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> to delete.</param>
        public virtual void Delete(TEntity entity)
        {
            EntityEntry<TEntity> entry = Context.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        /// <summary>
        /// Delete <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> to delete.</param>
        public virtual Task DeleteAsync(TEntity entity)
        {
            return Task.Run(() => Delete(entity));
        }

        /// <summary>
        /// Save all context changes.
        /// </summary>
        /// <returns>Affected row count.</returns>
        public int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    throw;
                }

                if (ex.InnerException.InnerException != null)
                {
                    throw ex.InnerException.InnerException;
                }

                throw ex.InnerException;
            }
        }

        public void DiscardChanges()
        {
            IEnumerable<EntityEntry> enteries = Context.ChangeTracker.Entries()
                .Where(x => x.Entity != null
                            && x.State != EntityState.Unchanged)
                .ToList();
            foreach (var entry in enteries)
            {
                entry.State = EntityState.Detached;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// When disposing, if the context has changes, save the context.
        /// </summary>
        /// <param name="disposing">Actively dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (Context == null)
            {
                return;
            }

            if (Context.ChangeTracker.HasChanges())
            {
                SaveChanges();
            }

            foreach (var entry in Context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }

            Context.Dispose();
            Context = null;
        }
    }
}
