using Millennium.Models;

namespace Millennium.Repositories
{
	public class InMemoryProductRepository : IRepository<Product>
	{
		private readonly List<Product> _products = new();
		private int _nextId = 1;

		public Task<IEnumerable<Product>> GetAllAsync() => Task.FromResult(_products.AsEnumerable());

		public Task<Product?> GetByIdAsync(int id) => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

		public Task<Product> CreateAsync(Product entity)
		{
			entity.AssignId(_nextId++);
			_products.Add(entity);
			return Task.FromResult(entity);
		}

		public Task<bool> UpdateAsync(Product entity)
		{
			var existing = _products.FirstOrDefault(p => p.Id == entity.Id);
			if (existing == null)
			{
				return Task.FromResult(false);
			}

			existing.Update(entity.Name, entity.Price);
			return Task.FromResult(true);
		}

		public Task<bool> DeleteAsync(int id)
		{
			var item = _products.FirstOrDefault(p => p.Id == id);
			if (item == null)
			{
				return Task.FromResult(false);
			}
			_products.Remove(item);
			return Task.FromResult(true);
		}
	}
}
