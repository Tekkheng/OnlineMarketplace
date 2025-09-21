using CartService.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CartService.RequestHandlers;

public class RemoveItemFromCartRequest : IRequest<bool>
{
    public string UserId { get; set; } = string.Empty;
    public int ProductId { get; set; }
}

public class RemoveItemFromCartHandler : IRequestHandler<RemoveItemFromCartRequest, bool>
{
    private readonly CartDbContext _context;

    public RemoveItemFromCartHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RemoveItemFromCartRequest request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);
        if (cart == null) return false;

        var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (itemToRemove == null) return false;

        cart.Items.Remove(itemToRemove);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}