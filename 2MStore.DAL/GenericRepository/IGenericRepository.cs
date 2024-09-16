using Microsoft.EntityFrameworkCore.Storage;

namespace _2MStore.DAL.Generic_Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        void Commit();
        void RollBack();

        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();

        //  Create- UPdata = Delete => ONE -MANY
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(ICollection<T> entities);
    }
}
