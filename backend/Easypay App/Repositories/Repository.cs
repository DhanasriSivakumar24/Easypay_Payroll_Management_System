using Easypay_App.Exceptions;
using Easypay_App.Interface;

namespace Easypay_App.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected static List<T> list = new List<T>();
        public async Task<T> AddValue(T entity)
        {
            list.Add(entity);
            return entity;
        }

        public async Task<T> DeleteValue(K key)
        {
            var value = await GetValueById(key);
            if (value != null)
            {
                list.Remove(value);
                return value;
            }
            throw new NotFiniteNumberException();
        }

        public async Task<IEnumerable<T>> GetAllValue()
        {
            return list;
        }

        public abstract Task<T> GetValueById(K key);

        public async Task<T> UpdateValue(K key, T entity)
        {
            var item = await GetValueById(key);
            if (item != null)
            {
                list.Remove(item);
                list.Add(entity);
                return entity;
            }
            throw new NoItemFoundException();
        }
    }
}
