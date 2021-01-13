using System;
using Microsoft.Extensions.DependencyInjection;
using Nytte.Wrappers;

namespace Nytte
{
    public static class Extensions
    {
        public static INytteBuilder AddNytte(this IServiceCollection services)
        {
            services.AddSingleton<IJson, Json>();
            return new NytteBuilder(services);
        }
    }
}
