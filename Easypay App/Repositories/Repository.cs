using Easypay_App.Exceptions;
using Easypay_App.Interfaces;

namespace Easypay_App.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected static List<T> list = new List<T>();
        public T AddValue(T entity)
        {
            list.Add(entity);
            return entity;
        }

        public T DeleteValue(K key)
        {
            var value = GetValueById(key);
            if (value != null)
            {
                list.Remove(value);
                return value;
            }
            throw new NotFiniteNumberException();
        }

        public IEnumerable<T> GetAllValue()
        {
            return list;
        }

        public abstract T GetValueById(K key);

        public T UpdateValue(K key, T entity)
        {
            var item = GetValueById(key);
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
