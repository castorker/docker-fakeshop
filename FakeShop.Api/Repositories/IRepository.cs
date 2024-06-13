namespace FakeShop.Api.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        T Get(int id);
        IEnumerable<T> GetAll();
        void SaveChanges();
    }
}
