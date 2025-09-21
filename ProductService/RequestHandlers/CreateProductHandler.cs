using MediatR;
using ProductService.Data;
using ProductService.Entities;
using ProductService.Models.ResponseModels;
using ProductService.Models.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace ProductService.RequestHandlers;

public class CreateProductHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
{
    private readonly ProductDbContext _context;
    public CreateProductHandler(ProductDbContext context) => _context = context;

    public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var response = new CreateProductResponse();
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                response.Message = "Product name is required";
                return response;
            }
            if (request.Name.Length > 100)
            {
                response.Message = "Product name cannot exceed 100 characters";
                return response;
            }
            if (request.Price <= 0)
            {
                response.Message = "Price must be greater than zero";
                return response;
            }
            if (request.Stock < 0)
            {
                response.Message = "Stock cannot be negative";
                return response;
            }

            bool isDuplicateExists = await _context.Products
                .AnyAsync(x =>
                    x.Name.Trim().ToUpper().Equals(request.Name.Trim().ToUpper())
                );
            if (isDuplicateExists) throw new Exception("Product exists");

            var Product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Products.AddAsync(Product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            response.Message = "Product created successfully.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }
        return response;
    }
}