using System.Linq.Expressions;
using SecurePass.Data.Entities;

namespace SecurePass.DAL.Contracts;

public interface ICrudBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<int> Add(TEntity entity);

    Task<int> AddRange(IEnumerable<TEntity> entities);

    Task<TEntity> GetById(int entityId, string? include = null);

    Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate, string? include = null);

    Task<IEnumerable<TEntity>> GetList(string? include = null, Expression<Func<TEntity, bool>>? predicate = null);

    Task<bool> Update(TEntity entity);

    Task<int> UpdateRange(IEnumerable<TEntity> entities);

    Task<bool> Delete(int entityId);

    Task<bool> Delete(TEntity entity);

    Task<int> DeleteRange(IEnumerable<TEntity> entities);
}

