namespace NLayer.Core.DTOs
{
    public class ProductWithCategoryDTO : ProductDTO
    {
        public CategoryDTO Category { get; set; }
    }
}