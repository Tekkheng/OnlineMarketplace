using CartService.Models.ResponseModels;
using MediatR;

namespace CartService.Models.RequestModels
{
    public class RemoveItemFromCartRequest : IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }
    }

}
