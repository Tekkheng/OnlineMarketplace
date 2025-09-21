using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models.RequestModels;
using ProductService.Models.ResponseModels;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<GetProductResponse>> Get([FromQuery] GetAllProductsRequest request)
    {
        var response = await _mediator.Send(request);
        return response != null ? Ok(response) : NotFound();
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<GetProductResponse>> GetProductById(int id)
    {
        var response = await _mediator.Send(new GetProductByIdRequest { Id = id });
        return response != null ? Ok(response) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GetProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UpdateProductResponse>> Put([FromBody] UpdateProductRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DeleteProductResponse>> Delete([FromQuery] DeleteProductRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}