# Nytte
A library of utilities helpful for projects.

> Please checkout the *staging* branch for the *alpha* releases.

## Getting Started
To get started with any of the packages find them on Nuget.

A simple way to see these in action is to checkout the [samples](https://github.com/mumby0168/Nytte/samples/Nytte.Web.Sample)

Taking the Nytte.Http package as an example use ```dotnet add package Nytte.Http --version 0.4.7-alpha``` to add the package to your project.

Then simply add Nyte and it's extra packages using the builder.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddNytte()
        .AddHttp();
}
```

This then allows you to use the services that Nytte provides an example of this is shown in the example code below:
```c#
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
```