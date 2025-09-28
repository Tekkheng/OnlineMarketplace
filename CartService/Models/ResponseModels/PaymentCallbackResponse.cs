namespace CartService.Models.ResponseModels;

public class PaymentCallbackResponse: BaseResponse
{
    public string? OrderId { get; set; }
    public string? Status { get; set; }
}
