using System.Net.Http;

namespace Nytte.Http
{
    public interface IHttpResponse
    {
        public HttpResponseMessage Message { get; }
        
        bool RefusedConnection { get; }
    }
    
    
    public interface IHttpResponse<out T> : IHttpResponse
    {
        public T Data { get; }
    }
}