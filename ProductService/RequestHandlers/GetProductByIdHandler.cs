using MediatR;
using ProductService.Data;
using ProductService.Models.ResponseModels;
using ProductService.Models.RequestModels;

namespace ProductService.RequestHandlers;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
{
    private readonly ProductDbContext _context;
    public GetProductByIdHandler(ProductDbContext context) => _context = context;

    public async Task<GetProductByIdResponse> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var response = new GetProductByIdResponse();

        try
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                response.Message = $"Product with ID {request.Id} not found";
                return response;
            }

            response.Data = new GetProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
            response.Message = "Success";
        }
        catch (Exception ex)
        {
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }
}
