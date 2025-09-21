using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;

namespace ProductService.RequestHandlers;

public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, DeleteProductResponse>
{
    private readonly ProductDbContext _context;
    public DeleteProductHandler(ProductDbContext context) => _context = context;

    public async Task<DeleteProductResponse> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var response = new DeleteProductResponse();
        try
        {
            var deleteResult = await _context.Products
                .Where(x => x.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deleteResult > 0)
            {
                response.Message = "Product deleted successfully";
            }
            else
            {
                response.Message = "Product not found";
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}