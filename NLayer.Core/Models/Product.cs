namespace NLayer.Core.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } //nullable ?
        public int Stock { get; set; }
        public decimal Price { get; set; }

        //navigation properties
        public int CategoryId { get; set; } //foreign key

        public Category Category { get; set; }  //bire çok ilişki

        public ProductFeature ProductFeature { get; set; } //bire bir ilişki
    }
}