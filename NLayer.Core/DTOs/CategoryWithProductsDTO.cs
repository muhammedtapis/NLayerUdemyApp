namespace NLayer.Core.DTOs
{
    public class CategoryWithProductsDTO : CategoryDTO
    {
        public List<ProductDTO> Products { get; set; } //category ile beraber ona bağlı products göstermek için
    }
}