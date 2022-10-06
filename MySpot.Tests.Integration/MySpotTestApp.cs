using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace MySpot.Tests.Integration;

internal sealed class MySpotTestApp : WebApplicationFactory<Program>
{
    public HttpClient Client { get; }

    public MySpotTestApp(Action<IServiceCollection> services)
    {
        //tworzy dla nas dedykowanego klienta w pamieci
        Client = WithWebHostBuilder(builder => //pozwala nadpisac domyslne ustwienia
        {
            if(services is not null)
            {
                builder.ConfigureServices(services);
            }
            builder.UseEnvironment("test");
        }).CreateClient();
    }
} 
