using Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services
                    .AddTransient<PublicApiClient>()
                    .AddHttpClient("ApiClient", client =>
                    {
                        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    })
                    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services
                    .AddScoped(provider => provider.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

            await SetClientOptions(builder);

            if (Utility.ClientOptions != null)
            {
                builder.Services.AddOidcAuthentication(options =>
                {
                    options.ProviderOptions.Authority = Utility.ClientOptions.Authority;
                    options.ProviderOptions.ClientId = Utility.ClientOptions.ClientId;
                    options.ProviderOptions.ResponseType = "code";
                    options.ProviderOptions.RedirectUri = $"{Utility.ClientOptions.Name}/authentication/login-callback";
                });
            }

            await builder.Build().RunAsync();
        }

        private static async Task SetClientOptions(WebAssemblyHostBuilder builder)
        {
            // I do not like this Sam I Am
            using (var serviceProvider = builder.Services.BuildServiceProvider())
            {
                var uriParts = builder
                                .Services
                                .BuildServiceProvider()
                                .GetService<NavigationManager>()
                                .Uri
                                .Split('/');
                var customer = Utility.ClientOptions?.Name ?? "shire";
                if (uriParts.Length > 3 && !string.IsNullOrWhiteSpace(uriParts[3])) { customer = uriParts[3]; }

                using var httpClient = serviceProvider.GetService<PublicApiClient>();
                httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                Utility.ClientOptions = await httpClient.GetResultAsync<ClientOptions>($"/api/auth?customerName={customer}");
            }
        }
    }
}
