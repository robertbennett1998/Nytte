using Microsoft.Extensions.DependencyInjection;

namespace Nytte
{
    public class NytteBuilder : INytteBuilder
    {
        internal NytteBuilder(IServiceCollection services)
        {
            Services = services;
        }  
        
        public IServiceCollection Services { get; }
    }
}