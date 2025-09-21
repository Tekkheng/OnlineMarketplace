using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;

namespace ProductService.RequestHandlers.Products;

public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, UpdateProductResponse>
{
    private readonly ProductDbContext _context;
    public UpdateProductHandler(ProductDbContext context) => _context = context;

    public async Task<UpdateProductResponse> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var response = new UpdateProductResponse();

        try
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
                throw new Exception("Product not found");

            bool isDuplicateExists = await _context.Products
                .AnyAsync(x =>
                    x.Id != request.Id &&
                    x.Name.Trim().ToUpper().Equals(request.Name.Trim().ToUpper()),
                    cancellationToken);

            if (isDuplicateExists)
                throw new Exception("Product with the same name already exists");

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            response.Message = "Product updated successfully";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}