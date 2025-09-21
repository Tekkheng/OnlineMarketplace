using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;

namespace ProductService.RequestHandlers;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsRequest, GetAllProductsResponse>
{
    private readonly ProductDbContext _context;
    public GetAllProductsHandler(ProductDbContext context) => _context = context;

    public async Task<GetAllProductsResponse> Handle(GetAllProductsRequest request, CancellationToken cancellationToken)
    {
        var response = new GetAllProductsResponse();

        try
        {
            var products = await _context.Products.AsNoTracking().ToListAsync(cancellationToken);

            if (!products.Any())
            {
                response.Message = "No products found";
                return response;
            }

            response.Data = products.Select(p => new GetProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            }).ToList();

            response.Message = "Success";
        }
        catch (Exception ex)
        {
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }
}
