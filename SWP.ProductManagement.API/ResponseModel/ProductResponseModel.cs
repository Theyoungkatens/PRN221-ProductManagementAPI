namespace SWP.ProductManagent.API.ResponseModel
{
    public class ProductResponseModel
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? UnitsInStock { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? CategoryId { get; set; }
    }
}
