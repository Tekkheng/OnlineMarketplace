using CartService.Models.ResponseModels;
using MediatR;

namespace CartService.Models.RequestModels
{
    public class GetCartByUserIdRequest : IRequest<CartResponse?>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
