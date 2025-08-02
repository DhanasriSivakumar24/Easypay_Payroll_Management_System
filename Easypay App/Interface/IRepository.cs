namespace Easypay_App.Interface
{
    public interface IRepository<K,T> where T : class
    {
        T AddValue(T entity);
        T UpdateValue(K key,T entity);
        T DeleteValue(K key);
        IEnumerable<T> GetAllValue();
        T GetValueById(K key);

    }
}
