using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Helpers
{
    public interface IBaseRepository
    {
        DbContext dbContext { get; set; }
        int SaveChanges();

        IEnumerable<TEntity> GetList<TEntity>()
            where TEntity : class;

        IEnumerable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class;

        Task<IEnumerable<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class;

        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class;

        Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class;

        TEntity Insert<TEntity>(TEntity entity)
            where TEntity : class;

        TEntity Update<TEntity>(TEntity entity)
            where TEntity : class;

        TEntity Delete<TEntity>(TEntity entity)
            where TEntity : class;
    }

    public class BaseRepository : IBaseRepository
    {
        private DbContext _db;

        public DbContext dbContext
        {
            get { return _db; }
            set { _db = value; }
        }

        public int SaveChanges()
        {
            if (_db == null)
                throw new Exception("_dbContext");
            return _db.SaveChanges();
        }

        public IEnumerable<TEntity> GetList<TEntity>()
            where TEntity : class
        {
            return _db.Set<TEntity>().ToList();

        }

     
        public IEnumerable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {

            return _db.Set<TEntity>().Where(predicate).ToList();

        }
        public async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {

            return await _db.Set<TEntity>().Where(predicate).ToListAsync();

        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return _db.Set<TEntity>().Where(predicate).FirstOrDefault();

        }

        public async Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
    where TEntity : class
        {
            return await _db.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();
        }

        public TEntity Insert<TEntity>(TEntity entity)
            where TEntity : class
        {
            var result = _db.Set<TEntity>().Add(entity);
            return result;
        }


        public TEntity Update<TEntity>(TEntity entity)
            where TEntity : class
        {
           _db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                return entity;
          
        }


        public TEntity Delete<TEntity>(TEntity entity)
            where TEntity : class
        {

            var result = _db.Set<TEntity>().Remove(entity);
            return result;

        }

    }
}