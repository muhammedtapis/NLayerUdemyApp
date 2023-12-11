using NLayer.Core.DTOs;

namespace NLayer.Web.Services
{
    //bu sınıfın amacı WEB katmanındaki mvc projemize dataları verileri api istek yoluyla almak kendi servisini kullanmıycak apiden alcak
    public class ProductAPIService
    {
        private readonly HttpClient _httpClient; //httpclient nesnesini alcak

        public ProductAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductWithCategoryDTO>> GetProductWithCategoryAsync()
        {
            //apideki GetProductsWithCategory metodunun döndüğü tipi GetFromJsonAsync içinde belirttik.
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDTO<List<ProductWithCategoryDTO>>>("products/GetProductsWithCategory");
            //istek yapcağımız controller / metod belirttik tırnak içinde.
            return response.Data;
        }

        public async Task<ProductDTO> SaveAsync(ProductDTO newProductDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("products", newProductDTO); // post metodunda ıstek atıyoruz products controller apide içine alcağı parametreyiuolladk
            if (!response.IsSuccessStatusCode) return null;  //eğer statuscode false ise null dön

            //fALSE DEĞİLSE artık bu responsun bodysini almamız lazım bize döndüğü değer  CustomREsponseDto<ProductDTO>> bunu okuycaz.
            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDTO<ProductDTO>>();

            return responseBody.Data;  //daha sonra bu responsebody datasını dönyoruz
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            //apideki products controllerin getById metoduna istek attık bu istekten bize dönen değer CustomResponsDTO<ProductDTO>> bunun datası ProductDTo oluyor.
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDTO<ProductDTO>>($"products/{id}");

            //if (response.Errors.Any())
            //{
            //    //farklı buseiness kod hata yakalama loglma yapabilisin
            //}

            return response.Data;  //response un datası CustomResponseDto nun içine olan ProductDTO
        }

        //update de durum biraz farklı oldu apiye gönderdğimiz istekten bize true false işlem başarılı başarısız döncek
        public async Task<bool> UpdateAsync(ProductDTO productDTO)
        {
            //tırnak içinde full url yazmıyoruz onun base i program.cs AddHTTPClient da belirtildi. oraya da appsettingsden geliyo
            var response = await _httpClient.PutAsJsonAsync("products", productDTO);

            return response.IsSuccessStatusCode;
        }

        //remove da update gibi geriye değer dto falan döndürmüyoruz
        public async Task<bool> RemoveAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}