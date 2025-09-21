namespace ProductService.Models.ResponseModels
{
    public class GetAllProductsResponse
    {
        public List<GetProductResponse> Data { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }

    public class GetProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
