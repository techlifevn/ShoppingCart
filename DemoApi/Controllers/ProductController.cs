using DemoApi.Common.Result;
using DemoApi.Model.Product;
using DemoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("")]
        public async Task<IActionResult> List(GetPagingRequest request)
        {
            var products = await _productService.GetPagingsAsync(request);
            return Ok(products);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            return Ok(await _productService.CreateAsync(request));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(ProductUpdateRequest request)
        {
            return Ok(await _productService.UpdateAsync(request));
        }
    }
}
