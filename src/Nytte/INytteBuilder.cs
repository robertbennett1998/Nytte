using Microsoft.Extensions.DependencyInjection;

namespace Nytte
{
    public interface INytteBuilder
    {
        IServiceCollection Services { get; }
    }
}