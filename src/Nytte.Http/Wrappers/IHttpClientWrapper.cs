using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nytte.Http.Wrappers
{
    public interface IHttpClientWrapper : IDisposable
    {
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);

        Task<HttpResponseMessage> GetAsync(string url);

        Task<HttpResponseMessage> PutAsync(string url, HttpContent content);

        Task<HttpResponseMessage> DeleteAsync(string url);
        
        HttpRequestHeaders DefaultRequestHeaders { get; }
    }
}