using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Linq.Translations;
using cod.application.Interfaces;
using cod.domain.Alerts;
using cod.domain.Authorization;
using cod.domain.Common;
using cod.domain.Data;
using cod.domain.Features;
using cod.domain.Operation;
using Z.EntityFramework.Plus;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.persistence.Implementations
{
    public partial class Repository : IdentityDbContext<ApplicationUser>, IRepository
    {
        private readonly AuthService _authService;
        public Repository(DbContextOptions<Repository> dbContextOptions, AuthService authService) : base(dbContextOptions)
        {
            this._authService = authService;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AlertConfigurationLanguage>()
         .Property(e => e.BodyFields)
         .HasConversion(
             arr => string.Join(',', arr),
             str => str.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<Product>()
                .Property(p => p.Cost)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<ProductVendor>()
                .Property(p => p.Cost)
                .HasColumnType("decimal(18,4)");
            modelBuilder.Entity<ProductVendor>()
                .Property(p => p.UnitCost)
                .HasColumnType("decimal(18,4)");
            modelBuilder.Entity<ProductVendor>()
                .Property(p => p.Discount)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<PriceApprovalDetail>()
                .Property(p => p.OldCost)
                .HasColumnType("decimal(18,4)");
            modelBuilder.Entity<PriceApprovalDetail>()
                .Property(p => p.NewCost)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<ProductRecipe>()
                .Property(p => p.UnitsPerPackage)
                .HasColumnType("decimal(18,6)");

            modelBuilder.Entity<ProductRecipeDetail>()
                .Property(p => p.Quantity)
                .HasColumnType("decimal(18,6)");
            modelBuilder.Entity<ProductRecipeDetail>()
                .Property(p => p.ProducedUnits)
                .HasColumnType("decimal(18,6)");

            modelBuilder.Entity<ProductStock>()
                .Property(p => p.Quantity)
                .HasColumnType("decimal(18,6)");
            modelBuilder.Entity<ProductStock>()
                .HasIndex(p => p.ProductId)
                .IsUnique();

            modelBuilder.Entity<InventoryKardex>()
                .Property(p => p.Quantity)
                .HasColumnType("decimal(18,6)");
            modelBuilder.Entity<InventoryKardex>()
                .Property(p => p.Balance)
                .HasColumnType("decimal(18,6)");
            modelBuilder.Entity<InventoryKardex>()
                .Property(p => p.UnitCost)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<ProductionOrder>()
                .Property(p => p.Quantity)
                .HasColumnType("decimal(18,6)");

            modelBuilder.Entity<ProductionShift>()
                .HasIndex(p => new { p.ProductionDate, p.ShiftTypeId })
                .IsUnique();

            modelBuilder.Entity<Route>()
                .Property(r => r.DeliversRegalia)
                .HasDefaultValue(false);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        public bool DisableTenantFilter { get; set; }
        public bool CustomTenantId { get; set; }
        public int TenantId { get; set; }

        public Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> expression) where T : class, IEntity
        {
            return this.WhereAsync<T>(expression).FirstOrDefaultAsync();
        }
        public Task<T> FirstOrDefaultActive<T>(Expression<Func<T, bool>> expression) where T : class, IEntity
        {
            return this.WhereActive<T>(expression).FirstOrDefaultAsync();
        }
        public void InsertAsync<T>(T entity) where T : class, IEntity
        {
            entity.CreationTime = Clock.Now;
            entity.CreatorUser = _authService.User;
            entity.IsActive = true;

            if (this.CustomTenantId)
            {
                if (entity is IEntityTenantable entityTenantable)
                    entityTenantable.TenantId = this.TenantId;
                    entity.CreatorUser = "JOB";
            } else if (entity is IEntityTenantable entityTenantable)
                entityTenantable.TenantId = _authService.TenantId;
            this.Set<T>().Add(entity);
        }
        public void InsertWithOutDefaultValues<T>(T entity) where T : class, IEntity
        {
            entity.CreationTime = Clock.Now;
            entity.IsActive = true;
            this.Set<T>().Add(entity);
        }
        public void InsertAsync<T>(List<T> entity) where T : class, IEntity
        {
            foreach(var item in entity)
            {
                item.CreationTime = Clock.Now;
                item.CreatorUser = _authService.User;
                item.IsActive = true;

                if (this.CustomTenantId)
                {
                    if (item is IEntityTenantable entityTenantable)
                        entityTenantable.TenantId = this.TenantId;
                        item.CreatorUser = "JOB";
                }
                else if (item is IEntityTenantable entityTenantable2)
                    entityTenantable2.TenantId = _authService.TenantId;
            }
            this.Set<T>().AddRange(entity);
        }
        public void SetAuthValuesToCreate<T>(T entity) where T : class, IEntity
        {
            entity.CreationTime = Clock.Now;
            entity.CreatorUser = _authService.User;
            entity.IsActive = true;

            if (this.CustomTenantId)
            {
                if (entity is IEntityTenantable entityTenantable)
                    entityTenantable.TenantId = this.TenantId;
                    entity.CreatorUser = "JOB";
            }
            else if (entity is IEntityTenantable entityTenantable2)
                entityTenantable2.TenantId = _authService.TenantId;
        }
        public IQueryable<T> WhereAsync<T>(Expression<Func<T, bool>> expression) where T : class, IEntity
        {
            Expression<Func<T, bool>> tenantExpression = this.GetTenantFilter<T>();
            return this.Set<T>().Where(tenantExpression).Where(expression).WithTranslations();
        }
        public IQueryable<T> WhereActive<T>(Expression<Func<T, bool>> expression) where T : class, IEntity
        {
            return this.WhereAsync<T>(expression).Where(c => c.IsActive);
        }
        public IQueryable<T> AllAsync<T>() where T : class, IEntity
        {
            Expression<Func<T, bool>> tenantExpression = this.GetTenantFilter<T>();
            return this.Set<T>().Where(tenantExpression).WithTranslations();
        }
        public IQueryable<T> AllActive<T>() where T : class, IEntity
        {
            return this.AllAsync<T>().Where(c => c.IsActive);
        }
        public void DeleteSync<T>(T entity) where T : class, IEntity
        {
            this.Set<T>().Remove(entity);
        }
        public void DisableSync<T>(T entity) where T : class, IEntity
        {
            entity.DisableDateTime = Clock.Now;
            entity.DisableUser = _authService.User;
            entity.IsActive = false;
            this.Set<T>().Update(entity);
        }
        public void EnableSync<T>(T entity) where T : class, IEntity
        {
            entity.EnableDateTime = Clock.Now;
            entity.EnableUser = _authService.User;
            entity.IsActive = true;
        }
        public void UpdateSync<T>(T entity) where T : class, IEntity
        {
            entity.LastModificationTime = Clock.Now;
            entity.LastModifiedUser = _authService.User;
            this.Set<T>().Update(entity);
        }
        public async Task CommitAsync()
        {
            await SaveChangesAsync();
        }
        Task<bool> IRepository.AnyAsync<T>(Expression<Func<T, bool>> expression)
        {
            return this.WhereAsync<T>(expression).AnyAsync();
        }

        Task<bool> IRepository.AnyActive<T>(Expression<Func<T, bool>> expression)
        {
            return this.WhereActive<T>(expression).AnyAsync();
        }

        IQueryable<T> IRepository.Query<T>(Expression<Func<T, bool>> expression)
        {
            return this.WhereAsync<T>(expression).AsNoTracking();
        }
        IQueryable<T> IRepository.Query<T>()
        {
            return this.AllAsync<T>().AsNoTracking();
        }
        IQueryable<T> IRepository.QueryActive<T>(Expression<Func<T, bool>> expression)
        {
            return this.WhereActive<T>(expression).AsNoTracking();
        }
        IQueryable<T> IRepository.QueryActive<T>()
        {
            return this.AllActive<T>().AsNoTracking();
        }
        public Expression<Func<T, bool>> GetTenantFilter<T>()
        {
            Expression<Func<T, bool>> expression = x => true;
            if (this.DisableTenantFilter)
                return expression;

            if (this.CustomTenantId && Activator.CreateInstance<T>() is IEntityTenantable)
                return entity => ((IEntityTenantable)entity).TenantId == this.TenantId;

            if (Activator.CreateInstance<T>() is IEntityTenantable)
                expression = entity => ((IEntityTenantable)entity).TenantId == _authService.TenantId;
            return expression;
        }
        Task IRepository.UpdateAsync<T>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T>> newValues)
        {
            return this.Set<T>().Where(whereExpression).UpdateAsync(newValues);
        }
    }
}
