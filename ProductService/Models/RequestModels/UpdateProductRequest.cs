using MediatR;
using ProductService.Models.ResponseModels;

namespace ProductService.Models.RequestModels
{
    public class UpdateProductRequest : IRequest<UpdateProductResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
