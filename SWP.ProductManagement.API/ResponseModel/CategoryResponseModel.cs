using SWP.ProductManagement.Service.BusinessModels;

namespace SWP.ProductManagent.API.ResponseModel
{
    public class CategoryResponseModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
}
