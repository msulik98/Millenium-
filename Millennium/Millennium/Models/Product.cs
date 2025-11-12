using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Millennium.Models
{
	public class Product
	{
		public Product(string name, decimal price)
		{
			Name = name;
			Price = price;
		}

		public long Id { get; private set; }

		[Required]
		[StringLength(100)]
		public string Name { get; private set; }

		[Range(0, double.MaxValue)]
		public decimal Price { get; private set; }

		public void Update(string name, decimal price)
		{
			Name = name;
			Price = price;
		}

		// It should not be here in real life implementation. Id should be assigned on DB level.
		public void AssignId(long id)
		{
			Id = id;
		}
	}
}
