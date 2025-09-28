namespace CartService.Models.ResponseModels
{
    public class CheckoutResponse : BaseResponse
    {
        public string PaymentUrl { get; set; } = string.Empty;
    }
}
