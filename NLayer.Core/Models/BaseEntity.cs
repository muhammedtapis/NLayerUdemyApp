namespace NLayer.Core.Models
{
    //abstractt yapmamızın sebebi bu baseEntityden nesne örneği oluşturulamasın yani soyut yapıyoruz interface gibi
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; } //ilk veritabanına kaydedildiğinde null gelmesi gerekiyor.
    }
}