using DemoApi.Common.Result;
using System.ComponentModel.DataAnnotations;

namespace DemoApi.Model.Product
{
    public class ProductUpdateRequest : UpdateRequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
        public decimal Price { get; set; }

        public string? Description { get; set; }
    }
}
