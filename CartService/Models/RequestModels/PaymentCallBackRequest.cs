using MediatR;
using CartService.Models.ResponseModels;
using System.Text.Json;

public class PaymentCallbackRequest : IRequest<PaymentCallbackResponse>
{
    public JsonElement Payload { get; set; }
}