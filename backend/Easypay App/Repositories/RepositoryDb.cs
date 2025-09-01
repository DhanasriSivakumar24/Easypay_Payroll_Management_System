using Easypay_App.Context;
using Easypay_App.Interface;
using Easypay_App.Models;

namespace Easypay_App.Repositories
{
    public abstract class RepositoryDb<K, T> : IRepository<K, T> where T : class
    {
        protected readonly PayrollContext _context;

        public RepositoryDb(PayrollContext context)
        {
            _context = context;
        }
        public async Task<T> AddValue(T entity)
        {
            _context.ChangeTracker.Clear();
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteValue(K key)
        {
            _context.ChangeTracker.Clear();
            var obj = await GetValueById(key);
            _context.Remove(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public abstract Task<IEnumerable<T>> GetAllValue();


        public abstract Task<T> GetValueById(K key);
        

        public async Task<T> UpdateValue(K key, T entity)
        {
            var obj = await GetValueById(key);
            _context.Entry<T>(obj).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
