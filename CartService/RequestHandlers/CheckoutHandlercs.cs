using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MediatR;
using CartService.Models.RequestModels;
using CartService.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;
using CartService.Data;

namespace CartService.RequestHandlers;

public class CheckoutHandler : IRequestHandler<CheckoutRequest, CheckoutResponse>
{
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly CartDbContext _context;

    public CheckoutHandler(IConfiguration config, IHttpClientFactory httpClientFactory, CartDbContext context)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _context = context;
    }

    public async Task<CheckoutResponse> Handle(CheckoutRequest request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart == null || !cart.Items.Any())
        {
            return new CheckoutResponse { Success = false, Message = "Cart is empty." };
        }

        // Logika checkout sekarang hanya menghapus keranjang.
        // Di aplikasi nyata, Anda akan membuat 'Order' dari data keranjang ini sebelum menghapusnya.
        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync(cancellationToken);

        return new CheckoutResponse
        {
            Success = true,
            Message = "Checkout successful, cart has been cleared."
        };
    }
}
