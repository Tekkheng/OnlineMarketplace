using System.ComponentModel.DataAnnotations;
namespace CartService.Entities;
public class Cart
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public List<CartItem> Items { get; set; } = new();
}