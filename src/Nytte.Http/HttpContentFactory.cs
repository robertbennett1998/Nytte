using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Nytte.Http
{
    
    public class HttpContentFactory : IHttpContentFactory
    {
        public HttpContent CreateJsonContent(string json) =>
            new StringContent(json, Encoding.UTF8, Constants.ApplicationJson);
    }
}