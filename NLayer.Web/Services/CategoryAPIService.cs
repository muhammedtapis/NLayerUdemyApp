using NLayer.Core.DTOs;

namespace NLayer.Web.Services
{
    public class CategoryAPIService
    {
        private readonly HttpClient _httpClient; //httpclient nesnesini alcak

        public CategoryAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            //GetFromJsonAsync<burada yazılan kısım api controllerdaki endpointten gelen verinin tipi.> ("burası ise controllerin ismi")
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDTO<List<CategoryDTO>>>("categories");
            return response.Data;
        }
    }
}