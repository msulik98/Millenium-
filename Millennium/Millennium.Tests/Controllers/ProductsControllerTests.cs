using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Millennium.Controllers;
using Millennium.Models;
using Millennium.Repositories;
using Moq;

namespace Millennium.Tests.Controllers
{
	[TestClass]
	public class ProductsControllerTests
	{
		private ProductsController _unitUnderTests;

		private Mock<IRepository<Product>> _repository;

		private Mock<ILogger<ProductsController>> _logger;


		[TestInitialize]
		public void Initialize()
		{
			// It would be good to use some data factory instead of creation test data manually.

			_repository = new Mock<IRepository<Product>>();
			_logger = new Mock<ILogger<ProductsController>>();

			_unitUnderTests = new ProductsController(_repository.Object, _logger.Object);
		}

		[TestMethod]
		public async Task GetAll_Always_AllProductsReturned()
		{
			// Arrange
			_repository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product>
				{
					new Product("TestProduct", 9.99m)
				});

			// Act
			var result = await _unitUnderTests.GetAll();

			// Assert
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			var products = okResult.Value as IEnumerable<Product>;
			Assert.IsNotNull(products);
			Assert.AreEqual(1, ((List<Product>)products).Count);
		}

		[TestMethod]
		public async Task GetById_ProductExists_ProperProductReturned()
		{
			// Arrange
			var product = new Product("test", 11.9m);
			_repository.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(product);

			// Act
			var result = await _unitUnderTests.GetById(10);

			// Assert
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			var returnedProduct = okResult.Value as Product;
			Assert.IsNotNull(returnedProduct);
			Assert.AreEqual(product, returnedProduct);
		}

		[TestMethod]
		public async Task GetById_ProductDoesNotExist_NotFound()
		{
			// Arrange
			_repository.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((Product)null);

			// Act
			var result = await _unitUnderTests.GetById(10);

			// Assert
			var notFoundObjectResult = result as NotFoundResult;
			Assert.IsNotNull(notFoundObjectResult);
		}

		[TestMethod]
		public async Task Create_ValidModel_ProductCreated()
		{
			// Arrange
			var product = new Product("ProductName", 3.99m);
			_repository.Setup(r => r.CreateAsync(product)).ReturnsAsync(product);

			// Act
			var result = await _unitUnderTests.Create(product);

			// Assert
			var createdAtActionResult = result as CreatedAtActionResult;
			Assert.IsNotNull(createdAtActionResult);
			var returnedProduct = createdAtActionResult.Value as Product;
			Assert.IsNotNull(returnedProduct);
			Assert.AreEqual(product, returnedProduct);
		}

		[TestMethod]
		public async Task Create_NotValidModel_BadRequest()
		{
			// Arrange
			var product = new Product("ProductName", -3.99m);
			_unitUnderTests.ModelState.AddModelError("Price", "Price must be non-negative");
			_repository.Setup(r => r.CreateAsync(product)).ReturnsAsync(product);

			// Act
			var result = await _unitUnderTests.Create(product);

			// Assert
			var createdAtActionResult = result as BadRequestObjectResult;
			Assert.IsNotNull(createdAtActionResult);
		}
	}
}
