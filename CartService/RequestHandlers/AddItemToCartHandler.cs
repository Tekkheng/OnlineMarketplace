using CartService.Data;
using CartService.Entities;
using CartService.Models.ResponseModels;
using CartService.Models.RequestModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace CartService.RequestHandlers;

public class AddItemToCartHandler : IRequestHandler<AddToCartRequest, AddItemToCartResponse>
{
    private readonly CartDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddItemToCartHandler(
        CartDbContext context,
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AddItemToCartResponse> Handle(AddToCartRequest request, CancellationToken cancellationToken)
    {
        if (request.Quantity <= 0)
        {
            return new AddItemToCartResponse
            {
                Success = false,
                Message = "Quantity must be greater than 0."
            };
        }

        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return new AddItemToCartResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        ProductInfoResponse? product = null;
        try
        {
            var productClient = _httpClientFactory.CreateClient("ProductService");
            var productResponse = await productClient.GetAsync($"/api/products/{request.ProductId}", cancellationToken);

            if (!productResponse.IsSuccessStatusCode)
            {
                return new AddItemToCartResponse
                {
                    Success = false,
                    Message = "Product not found or Product Service unavailable."
                };
            }

            var content = await productResponse.Content.ReadAsStringAsync(cancellationToken);

            var wrapper = JsonSerializer.Deserialize<ProductInfoWrapper>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            product = wrapper?.Data;
        }
        catch (Exception ex)
        {
            return new AddItemToCartResponse
            {
                Success = false,
                Message = $"An error occurred while contacting Product Service: {ex.Message}"
            };
        }

        if (product == null || product.Id <= 0)
        {
            return new AddItemToCartResponse
            {
                Success = false,
                Message = "Invalid ProductId."
            };
        }

        if (product.Stock < request.Quantity)
        {
            return new AddItemToCartResponse
            {
                Success = false,
                Message = "Insufficient product stock."
            };
        }

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem>()
            };
            _context.Carts.Add(cart);
        }

        var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (cartItem != null)
        {
            cartItem.Quantity += request.Quantity;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = request.Quantity
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new AddItemToCartResponse
        {
            Success = true,
            Message = "Item successfully added to cart."
        };
    }
}
