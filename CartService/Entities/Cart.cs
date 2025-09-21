using System.ComponentModel.DataAnnotations;
namespace CartService.Entities;
public class Cart
{
    [Key] public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<CartItem> Items { get; set; } = new();
}