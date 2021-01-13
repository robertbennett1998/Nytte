using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nytte.Http;

namespace Nytte.Web.Sample
{
    public record AJoke(int Id, string Type, string Setup, string Punchline);
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNytte()
                .AddHttp();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var scope = app.ApplicationServices.CreateScope();
                    var httpClient = scope.ServiceProvider.GetRequiredService<IHttpClient>();
                    
                    var url = "https://official-joke-api.appspot.com/random_joke";
                    var result = await httpClient.GetAsync<AJoke>(url);
                    if (result.RefusedConnection)
                    {
                        await context.Response.WriteAsync(
                            $"uh oh the joke service appears to be offline using url: {url}");
                    }
                    else if (result.Data is { })
                    {
                        var joke = result.Data;
                        await context.Response.WriteAsync($"{joke.Setup} ... \n{joke.Punchline}");
                    }
                    else
                    {
                        await context.Response.WriteAsync($"Uh oh request failed with status code: {result.Message.StatusCode}");   
                    }
                    
                    scope.Dispose();
                });
            });
        }
    }
}