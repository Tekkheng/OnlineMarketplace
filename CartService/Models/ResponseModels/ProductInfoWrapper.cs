namespace CartService.Models.ResponseModels
{
    public class ProductInfoWrapper
    {
        public ProductInfoResponse? Data { get; set; }
        public string? Message { get; set; }
    }

    public class ProductInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
