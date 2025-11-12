using Microsoft.AspNetCore.Mvc;
using Millennium.Models;
using Millennium.Repositories;

namespace Millennium.Controllers
{
	[ApiController]
	[Route("api/products")]
	public class ProductsController : ControllerBase
	{
		private readonly IRepository<Product> _repository;
		private readonly ILogger<ProductsController> _logger;

		public ProductsController(IRepository<Product> repository, ILogger<ProductsController> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			_logger.LogInformation("Fetching all products");
			return Ok(await _repository.GetAllAsync());
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var product = await _repository.GetByIdAsync(id);
			if (product == null)
			{
				_logger.LogWarning("Product with requested id does not exists");
				return NotFound();
			}

			return Ok(product);
		}

		[HttpPost]
		[Route("update")]
		public async Task<IActionResult> Create([FromBody] Product product)
		{
			if (!ModelState.IsValid)
			{
				_logger.LogError("Provided model is not valid.");
				return BadRequest(ModelState);
			}

			var created = await _repository.CreateAsync(product);
			_logger.LogInformation("Product successfully created");

			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] Product product)
		{
			if (!ModelState.IsValid)
			{
				_logger.LogError("Provided model is not valid.");
				return BadRequest(ModelState);
			}

			var success = await _repository.UpdateAsync(product);
			if (!success)
			{
				_logger.LogWarning("Product with requested id does not exists");
				return NotFound();
			}

			_logger.LogInformation("Product successfully updated");
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var success = await _repository.DeleteAsync(id);
			if (!success)
			{
				_logger.LogWarning("Product with requested id does not exists");
				return NotFound();
			}

			_logger.LogInformation("Product successfully deleted");
			return NoContent();
		}
	}
}