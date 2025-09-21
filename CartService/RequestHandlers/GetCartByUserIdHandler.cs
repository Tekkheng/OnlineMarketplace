using CartService.Data;
using CartService.Models.ResponseModels;
using CartService.Models.RequestModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CartService.RequestHandlers;

public class GetCartByUserIdHandler : IRequestHandler<GetCartByUserIdRequest, CartResponse?>
{
    private readonly CartDbContext _context;

    public GetCartByUserIdHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task<CartResponse?> Handle(GetCartByUserIdRequest request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart == null || !cart.Items.Any())
        {
            return null;
        }

        return new CartResponse
        {
            UserId = cart.UserId,
            Items = cart.Items.Select(item => new CartItemResponse
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Price = item.Price,
                Quantity = item.Quantity
            }).ToList(),
            TotalPrice = cart.Items.Sum(item => item.Price * item.Quantity)
        };
    }
}