using MediatR;
using ProductService.Data;
using ProductService.Entities;
using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace ProductService.RequestHandlers;

public class CreateProductHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
{
    private readonly ProductDbContext _context;
    public CreateProductHandler(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        bool isDuplicateExists = await _context.Products
            .AnyAsync(x => x.Name.Trim().ToUpper() == request.Name.Trim().ToUpper(), cancellationToken);

        if (isDuplicateExists)
            throw new Exception("Product with the same name already exists");

        var product = new Product
        {
            Name = request.Name.Trim(),
            Price = request.Price,
            Stock = request.Stock,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateProductResponse { Message = "Product created successfully." };
    }
}
