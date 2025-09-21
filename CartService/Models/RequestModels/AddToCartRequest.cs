using CartService.Models.ResponseModels;
using MediatR;
namespace CartService.Models.RequestModels;

public class AddToCartRequest : IRequest<AddItemToCartResponse>
{
    public string UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}