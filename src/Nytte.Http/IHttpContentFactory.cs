using System.Net.Http;

namespace Nytte.Http
{
    public interface IHttpContentFactory
    {
        HttpContent CreateJsonContent(string json);
    }
}