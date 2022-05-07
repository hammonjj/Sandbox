using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AKSoftware.Localization.MultiLanguages;
using Blazored.LocalStorage;
using MondoDbTemplate.Client.ViewModels;

namespace MondoDbTemplate.Client {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddHttpClient<IBookViewModel, BookViewModel>
                ("MongoDbTemplate", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
                options.ProviderOptions.ResponseType = "code";
            });

            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }
    }
}
