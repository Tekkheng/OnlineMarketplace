using MediatR;
using CartService.Models.ResponseModels;

namespace CartService.Models.RequestModels
{
    public class CheckoutRequest : IRequest<CheckoutResponse>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
