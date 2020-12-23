using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessCore.DataAccess.Infrastructure
{
    /// <summary>
    /// Interface for all non-async functionality.  
    /// <p>
    /// Includes:
    /// <see cref="IReadonlyRepository{TEntity}"/>
    /// <see cref="IEditableRepository{TEntity}"/>
    /// <see cref="IDeletableRepository{TEntity}"/>
    /// </p>
    /// </summary>
    /// <typeparam name="TEntity">The entity the repository is based on.</typeparam>
    public interface IRepository<TEntity> : IReadonlyRepository<TEntity>, IEditableRepository<TEntity>, IDeletableRepository<TEntity>
        where TEntity : class
    { }

    /// <summary>
    /// Interface for all async functionality.  
    /// <p>
    /// Includes:
    /// <see cref="IReadonlyRepositoryAsync{TEntity}"/>
    /// <see cref="IEditableRepositoryAsync{TEntity}"/>
    /// <see cref="IDeletableRepositoryAsync{TEntity}"/>
    /// </p>
    /// </summary>
    /// <typeparam name="TEntity">The entity the repository is based on.</typeparam>
    public interface IRepositoryAsync<TEntity> : IReadonlyRepositoryAsync<TEntity>, IEditableRepositoryAsync<TEntity>, IDeletableRepositoryAsync<TEntity>
        where TEntity : class
    { }

    /// <summary>
    /// Interface for all non-async read functionality.
    /// </summary>
    /// <typeparam name="TEntity">The entity the repository is based on.</typeparam>
    public interface IReadonlyRepository<TEntity> : IDisposable
        where TEntity : class
    {
        /// <summary>
        /// Return all <see cref="TEntity"/>, including provided joins.
        /// </summary>
        /// <param name="includes">Navigation items.</param>
        /// <returns><see cref="TEntity"/> result set.</returns>
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Return all <see cref="TEntity"/>, including provided joins, with the provided predicate.
        /// </summary>
        /// <param name="predicate">Filter for <see cref="TEntity"/> result set.</param>
        /// <param name="includes">Navigation items.</param>
        /// <returns><see cref="TEntity"/> result set.</returns>
        IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Return <see cref="TEntity"/> specified by the provided predicate.
        /// </summary>
        /// <param name="predicate">Filter for <see cref="TEntity"/></param>
        /// <param name="includes">Navigation items.</param>
        /// <returns><see cref="TEntity"/></returns>
        TEntity FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Returns <see cref="TEntity"/> specified by the primary key.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns><see cref="TEntity"/></returns>
        TEntity GetById(object id);

        /// <summary>
        /// Return <see cref="TResult"/>, based on provided selector and included joins.
        /// </summary>
        /// <typeparam name="TResult">Return type.</typeparam>
        /// <param name="selector">Filter for <see cref="TEntity"/></param>
        /// <param name="prop">Property to return.</param>
        /// <param name="includes">Navigation items.</param>
        /// <returns><see cref="TResult"/>.</returns>
        TResult GetValue<TResult>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TResult>> prop, params Expression<Func<TEntity, object>>[] includes);
    }

    /// <summary>
    /// Interface for all async read functionality.
    /// </summary>
    /// <typeparam name="TEntity">The entity the repository is based on.</typeparam>
    public interface IReadonlyRepositoryAsync<TEntity> : IDisposable
        where TEntity : class
    {
        /// <summary>
        /// Return all <see cref="TEntity"/>, including provided joins.
        /// </summary>
        /// <param name="includes">Navigation items.</param>
        /// <returns>Async <see cref="TEntity"/> result set.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Return all <see cref="TEntity"/>, including provided joins, with the provided predicate.
        /// </summary>
        /// <param name="predicate">Filter for <see cref="TEntity"/> result set.</param>
        /// <param name="includes">Navigation items.</param>
        /// <returns>Async <see cref="TEntity"/> result set.</returns>
        Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Return <see cref="TEntity"/> specified by the provided predicate.
        /// </summary>
        /// <param name="predicate">Filter for <see cref="TEntity"/></param>
        /// <param name="includes">Navigation items.</param>
        /// <returns>Async <see cref="TEntity"/></returns>
        Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Returns <see cref="TEntity"/> specified by the primary key.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns>Async <see cref="TEntity"/></returns>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// Return <see cref="TResult"/>, based on provided selector and included joins.
        /// </summary>
        /// <typeparam name="TResult">Return type.</typeparam>
        /// <param name="selector">Filter for <see cref="TEntity"/></param>
        /// <param name="prop">Property to return.</param>
        /// <param name="includes">Navigation items.</param>
        /// <returns>Async <see cref="TResult"/>.</returns>
        Task<TResult> GetValueAsync<TResult>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TResult>> prop, params Expression<Func<TEntity, object>>[] includes);
    }

    /// <summary>
    /// Interface for all non-async ediablte functionality.
    /// </summary>
    /// <typeparam name="TEntity">The entity the repository is based on.</typeparam>
    public interface IEditableRepository<TEntity> : ISaveableRepository
        where TEntity : class
    {
        /// <summary>
        /// Create <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> to create.</param>
        /// <returns>Added <see cref="TEntity"/>.</returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Update <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> to update.</param>
        void Update(TEntity entity);
    }

    /// <summary>
    /// Interface for all async ediablte functionality.
    /// </summary>
    /// <typeparam name="TEntity">The entity the repository is based on.</typeparam>
    public interface IEditableRepositoryAsync<TEntity> : ISaveableRepository
        where TEntity : class
    {
        /// <summary>
        /// Create <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> to create.</param>
        /// <returns>Asymc added <see cref="TEntity"/>.</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Update <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> to update.</param>
        Task UpdateAsync(TEntity entity);
    }

    /// <summary>
    /// Interface for non-async delete functionality.
    /// </summary>
    /// <typeparam name="TEntity">The entity the repository is based on.</typeparam>
    public interface IDeletableRepository<in TEntity> : ISaveableRepository
        where TEntity : class
    {
        /// <summary>
        /// Delete <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> to delete.</param>
        void Delete(TEntity entity);
    }

    /// <summary>
    /// Interface for async delete functionality.
    /// </summary>
    /// <typeparam name="TEntity">The entity the repository is based on.</typeparam>
    public interface IDeletableRepositoryAsync<in TEntity> : ISaveableRepository
        where TEntity : class
    {
        /// <summary>
        /// Delete <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> to delete.</param>
        Task DeleteAsync(TEntity entity);
    }

    /// <summary>
    /// Interface for saving data before the final save.
    /// </summary>
    public interface ISaveableRepository : IDisposable
    {
        /// <summary>
        /// Save all context changes.
        /// </summary>
        /// <returns>Affected row count.</returns>
        int SaveChanges();

        /// <summary>
        /// Discard all context changes.
        /// </summary>
        void DiscardChanges();
    }
}
