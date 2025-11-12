using Millennium.Models;

namespace Millennium.Repositories
{
	public interface IRepository<T>
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<Product?> GetByIdAsync(int id);
		Task<T> CreateAsync(T entity);
		Task<bool> UpdateAsync(T entity);
		Task<bool> DeleteAsync(int id);
	}
}
