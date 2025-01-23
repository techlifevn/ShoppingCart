using DemoApi.Common.Result;
using DemoApi.Model.Api;
using DemoApi.Model.Product;
using DemoApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.App.Controllers
{
    [Authorize(Roles = "Customer")]
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

            return Ok(new ApiResponse<PagedResult<ProductDTO>>
            {
                Status = true,
                Message = "Success",
                Data = products
            });
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            var result = await _productService.CreateAsync(request);

            if (!result.IsSuccessed)
            {
                return BadRequest(new ApiResponse<dynamic>
                {
                    Status = false,
                    Message = "Failure"
                });
            }

            return Ok(new ApiResponse<ProductDTO>
            {
                Status = true,
                Message = "Success",
                Data = result.ResultObj
            });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new ApiResponse<dynamic>
                {
                    Status = false,
                    Message = "Not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<ProductDTO>
            {
                Status = true,
                Message = "Success",
                Data = product
            });
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, ProductUpdateRequest request)
        {
            request.Id = id;

            var result = await _productService.UpdateAsync(request);

            if (!result.IsSuccessed)
            {
                return NotFound(new ApiResponse<ProductDTO>
                {
                    Status = result.IsSuccessed,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponse<ProductDTO>
            {
                Status = result.IsSuccessed,
                Message = result.Message,
                Data = result.IsSuccessed ? result.ResultObj : null
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id, DeleteRequest request)
        {
            request.Id = id;

            var result = await _productService.DeleteAsync(request);

            if (!result.IsSuccessed)
            {
                return NotFound(new ApiResponse<dynamic>
                {
                    Status = result.IsSuccessed,
                    Message = result.Message,
                });
            }

            return Ok(new ApiResponse<dynamic>
            {
                Status = result.IsSuccessed,
                Message = result.Message,
            });
        }
    }
}
