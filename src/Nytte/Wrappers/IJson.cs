using System.Threading.Tasks;

namespace Nytte.Wrappers
{
    public interface IJson
    {
        Task<string> SerializeAsync(object obj);

        Task<T> DeserializeAsync<T>(string json);
        
    }
}