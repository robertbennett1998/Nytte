using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Nytte.Http.Wrappers;

namespace Nytte.Http
{
    public static class Extensions
    {
        public static INytteBuilder AddHttp(this INytteBuilder builder, HttpClient client = null)
        {
            builder.Services.AddScoped(sp => client ?? new HttpClient());
            builder.Services.AddScoped<IHttpClient, EnhancedHttpClient>();
            builder.Services.AddScoped<IHttpClientWrapper, HttpClientWrapper>(); 
            builder.Services.AddTransient<IHttpContentFactory, HttpContentFactory>();
            return builder;
        }
    }
}

