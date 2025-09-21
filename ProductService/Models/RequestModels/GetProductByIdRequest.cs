using MediatR;
using ProductService.Models.ResponseModels;

namespace ProductService.Models.RequestModels
{
    public class GetProductByIdRequest : IRequest<GetProductByIdResponse>
    {
        public int Id { get; set; }
    }
}
