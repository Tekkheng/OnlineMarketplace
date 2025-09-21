using MediatR;
using ProductService.Models.ResponseModels;

namespace ProductService.Models.RequestModels
{
    public class GetAllProductsRequest : IRequest<GetAllProductsResponse> { }
}
