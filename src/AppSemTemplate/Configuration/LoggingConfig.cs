using AppSemTemplate.Data;
using Elmah.Io.AspNetCore;
using Elmah.Io.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace AppSemTemplate.Configuration
{
    public static class LogginConfig
    {
        public static WebApplicationBuilder AddElmahConfiguration(this WebApplicationBuilder builder) 
        {
            //config pro elmah pegar exception ou erro
            builder.Services.Configure<ElmahIoOptions>(builder.Configuration.GetSection("ElmahIo"));//configurando elmah con chaves da appsettings
            builder.Services.AddElmahIo();

            //Configurando Elmah para pegar outros loggins além de erros
            builder.Logging.Services.Configure<ElmahIoProviderOptions>(builder.Configuration.GetSection("ElmahIo"));
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddElmahIo();

            builder.Logging.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);//filtrando loggs de Warning pra cima.

            return builder;
        }
    }
}
