using Microsoft.AspNetCore.Localization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace AppSemTemplate.Configuration
{
    public static class GlobalizationConfig
    {
        //public static WebApplication UseGlobalizationConfig(this WebApplication app)//"Forçar que a aplicação tenha uma cultura"
        //{
        //    var defaultCulture = new CultureInfo("pt-BR");

        //    var localizationOptions = new RequestLocalizationOptions //configs necessárias para que a app se adapte à cultura
        //    {
        //        DefaultRequestCulture = new RequestCulture(defaultCulture),
        //        SupportedCultures = new List<CultureInfo> {defaultCulture},
        //        SupportedUICultures = new List<CultureInfo> { defaultCulture}

        //    };

        //    app.UseRequestLocalization(localizationOptions);//add um middlewareque automaticamente vai setar a cultura a cultura informada para os requests

        //    return app;
        //}

        public static WebApplication UseGlobalizationConfig(this WebApplication app)
        {
            var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            return app;
        }

        public static WebApplicationBuilder AddGlobalizationConfig(this WebApplicationBuilder builder)//suporte a mais de um cultura
        {
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en-US", "pt-BR" };
                options.SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });

            return builder;
        }

    }
}
