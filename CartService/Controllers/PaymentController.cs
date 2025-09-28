using CartService.Models.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CartService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("midtrans-callback")]
    public async Task<ActionResult<PaymentCallbackResponse>> MidtransCallback([FromBody] JsonElement payload)
    {
        var result = await _mediator.Send(new PaymentCallbackRequest { Payload = payload });
        return Ok(result);
    }
}
