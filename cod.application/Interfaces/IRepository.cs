using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using cod.domain.Common;

namespace cod.application.Interfaces
{
    public interface IRepository
    {
        bool DisableTenantFilter { get; set; }
        bool CustomTenantId { get; set; }
        int TenantId { get; set; }
        Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> expression) where T : class, IEntity;
        Task<T> FirstOrDefaultActive<T>(Expression<Func<T, bool>> expression) where T : class, IEntity;
        void InsertAsync<T>(T entity) where T : class, IEntity;
        void InsertWithOutDefaultValues<T>(T entity) where T : class, IEntity;
        void InsertAsync<T>(List<T> entity) where T : class, IEntity;
        IQueryable<T> WhereAsync<T>(Expression<Func<T, bool>> expression) where T : class, IEntity;
        IQueryable<T> WhereActive<T>(Expression<Func<T, bool>> expression) where T : class, IEntity;
        IQueryable<T> AllAsync<T>() where T : class, IEntity;
        IQueryable<T> AllActive<T>() where T : class, IEntity;
        IQueryable<T> Query<T>(Expression<Func<T, bool>> expression) where T : class, IEntity;
        IQueryable<T> Query<T>() where T : class, IEntity;
        IQueryable<T> QueryActive<T>(Expression<Func<T, bool>> expression) where T : class, IEntity;
        IQueryable<T> QueryActive<T>() where T : class, IEntity;
        void DeleteSync<T>(T entity) where T : class, IEntity;
        void DisableSync<T>(T entity) where T : class, IEntity;
        void EnableSync<T>(T entity) where T : class, IEntity;
        void UpdateSync<T>(T entity) where T : class, IEntity;
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> expression) where T : class, IEntity;
        Task<bool> AnyActive<T>(Expression<Func<T, bool>> expression) where T : class, IEntity;
        Task UpdateAsync<T>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T>> newValues) where T : class, IEntity;
        Task CommitAsync();
        void SetAuthValuesToCreate<T>(T entity) where T : class, IEntity;
    }
}
