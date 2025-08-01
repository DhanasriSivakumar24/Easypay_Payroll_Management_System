
using Easypay_App.Context;
using Easypay_App.Interfaces;
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
        public T AddValue(T entity)
        {
            _context.Add(entity);//Adds the entry to the current collection. Marks teh status of teh entry to added
            _context.SaveChanges();//Creates the insert query with the new value and executes it.
            return entity;//new object withteh identity will be provided
        }

        public T DeleteValue(K key)
        {
            var obj = GetValueById(key);//Gets teh object withteh ID
            _context.Remove(obj);//Identifies teh object within teh colelction, marks teh status to deleted
            _context.SaveChanges();//Generates the delete queryby default cascading delete
            return obj;//returns the deleted object
        }

        public abstract IEnumerable<T> GetAllValue();


        public abstract T GetValueById(K key);
        

        public T UpdateValue(K key, T entity)
        {
            var obj = GetValueById(key);
            _context.Entry<T>(obj).CurrentValues.SetValues(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
