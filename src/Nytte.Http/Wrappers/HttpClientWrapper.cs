using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nytte.Http.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content) 
            => _httpClient.PostAsync(url, content);

        public Task<HttpResponseMessage> GetAsync(string url) => _httpClient.GetAsync(url);

        public Task<HttpResponseMessage> PutAsync(string url, HttpContent content) =>
            _httpClient.PutAsync(url, content);

        public Task<HttpResponseMessage> DeleteAsync(string url) 
            => _httpClient.DeleteAsync(url);

        public HttpRequestHeaders DefaultRequestHeaders => _httpClient.DefaultRequestHeaders;

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}