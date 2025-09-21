namespace ProductService.Models.ResponseModels
{
    public class GetProductByIdResponse
    {
        public GetProductResponse? Data { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
