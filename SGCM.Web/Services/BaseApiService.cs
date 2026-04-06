using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SGCM.Web.Services
{
    public class BaseApiService
    {
        protected readonly HttpClient _httpClient;
        protected readonly IConfiguration _configuration;
        protected readonly JsonSerializerOptions _jsonOptions;

        public BaseApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var baseUrl = _configuration["ApiBaseUrl"];
            if (!string.IsNullOrEmpty(baseUrl))
            {
                _httpClient.BaseAddress = new Uri(baseUrl);
            }
        }

        protected void SetAuthorizationToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        protected void ClearAuthorizationToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        protected async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase}). Content: {content}");
            }
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }

        protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data, _jsonOptions);
            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }

        protected async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data, _jsonOptions);
            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }

        protected async Task<bool> DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }

        protected async Task<HttpResponseMessage> PostAsyncWithResponse<TRequest>(string endpoint, TRequest data)
        {
            return await _httpClient.PostAsJsonAsync(endpoint, data, _jsonOptions);
        }
    }
}
