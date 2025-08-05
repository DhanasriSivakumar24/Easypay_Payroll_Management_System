namespace Easypay_App.Interface
{
    public interface IRepository<K,T> where T : class
    {
        public Task<T> AddValue(T entity);
        public Task<T> UpdateValue(K key,T entity);
        public Task<T> DeleteValue(K key);
        public Task<IEnumerable<T>> GetAllValue();
        public Task<T> GetValueById(K key);
    }
}
