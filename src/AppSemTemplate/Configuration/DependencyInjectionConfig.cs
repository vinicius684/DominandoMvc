using AppSemTemplate.Data;
using AppSemTemplate.Extensions;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace AppSemTemplate.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();// inje��o de dependencia do TagHelper

            //exemplos ciclos de vida diferentes
            builder.Services.AddTransient<IOperacaoTransient, Operacao>();
            builder.Services.AddScoped<IOperacaoScoped, Operacao>();
            builder.Services.AddSingleton<IOperacaoSingleton, Operacao>();
            builder.Services.AddSingleton<IOperacaoSingletonInstance>(new Operacao(Guid.Empty));

            builder.Services.AddTransient<OperacaoService>();

            builder.Services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>(); //Registrando Provider para Data Annotation customizado(Moeda)

            return builder;
        }
    }
}
