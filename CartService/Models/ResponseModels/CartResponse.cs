namespace CartService.Models.ResponseModels;

public class CartResponse
{
    public string UserId { get; set; } = string.Empty;
    public List<CartItemResponse> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }
}