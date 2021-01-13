using System;
using Microsoft.Extensions.DependencyInjection;
using Nytte.Wrappers;

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
