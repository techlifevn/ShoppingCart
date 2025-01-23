using DemoApi.Common.Result;
using DemoApi.Data.EF;
using DemoApi.Data.Entity;
using DemoApi.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllAsync();
        Task<PagedResult<ProductDTO>> GetPagingsAsync(GetPagingRequest request);
        Task<ProductDTO?> GetByIdAsync(int id);
        Task<Result<ProductDTO>> CreateAsync(ProductCreateRequest request);
        Task<Result<ProductDTO>> UpdateAsync(ProductUpdateRequest request);
        Task<Result<ProductDTO>> DeleteAsync(DeleteRequest request);
        Task<Result<int>> UpdateStatus(UpdateRequestBase request);
    }

    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<ProductDTO>> CreateAsync(ProductCreateRequest request)
        {
            string action = "Thêm sản phẩm mới";

            var obj = new Product
            {
                Name = request.Name.Trim(),
                Price = request.Price,
                Description = request.Description?.Trim(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false,
                UserId = request?.UserId
            };

            _context.Products.Add(obj);

            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Result<ProductDTO>.Success(action, new ProductDTO
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Price = obj.Price,
                    Description = obj.Description,
                    IsDeleted = false,
                });
            }
            return Result<ProductDTO>.Error(action, new ProductDTO());
        }

        public async Task<Result<ProductDTO>> DeleteAsync(DeleteRequest request)
        {
            string action = "Xóa sản phẩm";

            var product = await _context.Products.FindAsync(request.Id);

            if (product == null) return Result<ProductDTO>.Error(action, "Không tìm thấy sản phẩm");

            product.IsDeleted = true;
            product.UpdatedAt = DateTime.Now;
            product.UserId = request?.UserId;

            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return Result<ProductDTO>.Success(action, "Xóa thành công");
        }

        public async Task<List<ProductDTO>> GetAllAsync()
        {

            var products = await _context.Products.Where(x => x.IsDeleted).Select(x => new ProductDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsDeleted = x.IsDeleted,
                Price = x.Price
            }).ToListAsync();

            return products;
        }

        public async Task<ProductDTO?> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null || product.IsDeleted) return null;

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                IsDeleted = product.IsDeleted,
                Price = product.Price
            };
        }

        public async Task<PagedResult<ProductDTO>> GetPagingsAsync(GetPagingRequest request)
        {
            var query = _context.Products.Where(x => x.Name.Contains(request.Keyword.Trim()) && !x.IsDeleted);

            var products = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsDeleted = x.IsDeleted,
                    Price = x.Price
                }).OrderBy(x => x.Id).ToListAsync();

            return new PagedResult<ProductDTO>
            {
                TotalRecords = products.Count,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = products
            };
        }

        public async Task<Result<ProductDTO>> UpdateAsync(ProductUpdateRequest request)
        {
            string action = $"Cập nhật sản phẩm: {request.Id}";

            var product = await _context.Products.FindAsync(request.Id);

            if (product == null || product.IsDeleted) return Result<ProductDTO>.Error(action, "Không tìm thấy sản phẩm");

            product.Name = request.Name.Trim();
            product.Description = request?.Description;
            product.Price = request?.Price ?? 0;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return Result<ProductDTO>.Success(action, new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                IsDeleted = product.IsDeleted
            });
        }

        public async Task<Result<int>> UpdateStatus(UpdateRequestBase request)
        {
            string action = $"Cập nhật trạng thái sản phẩm: {request.Id}";

            var product = await _context.Products.FindAsync(request.Id);

            if (product == null || product.IsDeleted) return Result<int>.Error(action);

            product.IsDeleted = !product.IsDeleted;
            product.UpdatedAt = DateTime.Now;
            product.UserId = request?.UserId;

            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return Result<int>.Success(action, product.Id);
        }
    }
}
