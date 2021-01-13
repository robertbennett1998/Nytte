using System;
using Microsoft.Extensions.DependencyInjection;

namespace Nytte
{
    public static class Extensions
    {
        public static INytteBuilder AddNytte(this IServiceCollection services)
        {
            return new NytteBuilder(services);
        }
    }
}
