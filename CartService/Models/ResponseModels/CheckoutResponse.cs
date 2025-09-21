namespace CartService.Models.ResponseModels
{
    public class CheckoutResponse : BaseResponse
    {
        public string RedirectUrl { get; set; } = string.Empty;
    }
}
