using CartService.Models.RequestModels;
using CartService.Models.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CartService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;
    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<CartResponse>> GetCart()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var response = await _mediator.Send(new GetCartByUserIdRequest { UserId = userId });
        return Ok(response);
    }

    [HttpPost("items")]
    public async Task<ActionResult<BaseResponse>> AddItemToCart([FromBody] AddToCartRequest request)
    {
        request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete("items/{productId}")]
    public async Task<ActionResult<BaseResponse>> RemoveItemFromCart(int productId)
    {
        var request = new RemoveItemFromCartRequest
        {
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
            ProductId = productId
        };

        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutResponse>> Checkout()
    {
        var request = new CheckoutRequest
        {
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty
        };

        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
