using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.Extensions.Logging;
using Sales.Mobile.Data;
using Sales.Mobile.Services;

namespace Sales.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7125") });
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://192.168.1.143:9040") });
            builder.Services.AddSweetAlert2();
            builder.Services.AddScoped<IRequestProvider, RequestProvider>();
            builder.Services.AddScoped<ICategoriasService, CategoriasService>();

            return builder.Build();
        }
    }
}