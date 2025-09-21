using MediatR;
using ProductService.Models.ResponseModels;

namespace ProductService.Models.RequestModels
{
    public class DeleteProductRequest : IRequest<DeleteProductResponse>
    {
        public int Id { get; set; }
    }
}
