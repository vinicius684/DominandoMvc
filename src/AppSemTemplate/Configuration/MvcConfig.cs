﻿using AppSemTemplate.Data;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AppSemTemplate.Configuration
{
    public static class MvcConfig
    {
        public static WebApplicationBuilder AddMvcConfiguration(this WebApplicationBuilder builder)////extension method, extendendo tipo/Classe WebApplicationBuilder, retornando um WebApplicationBuilder
        {

            builder.Configuration//configuração para adicionar um pouco mais de suporte na appsettings, de acordo com o perfil que a app vai subir
                .SetBasePath(builder.Environment.ContentRootPath)//referencia ao csproj
                .AddJsonFile("appsettings.json", true, true)//true, true - se é obrigado a ter, se tem que reiniciar a app ao fazer uma mudança na estrutura appsettings(?) - em produção por exemplo reinicia a app  (pode ou não pode n entendi              
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly(),true);//adicionando arquivos de configuração secrets


            builder.Services.AddControllersWithViews(options =>
            {//Declara o MVC j� utilizando Gobalmente o ValidateAntiforgeryToken (prote��o contra Ataque CSRF)
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            // Adicionando suporte a mudan�a de conven��o da rota das areas.
            builder.Services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Modulos_AreaComOutroNome/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Modulos_AreaComOutroNome/{2}/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });


            builder.Services.AddDatabaseDeveloperPageExceptionFilter();/*Filtro de erros do DB*/

            builder.Services.AddDbContext<ApplicationDbContext>(o =>
                o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //outras configura��es HSTS - RElacionado a HTTPS
            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("example.com");//imputadr hosts que n�o quero o HSTS
                options.ExcludedHosts.Add("www.example.com");
            });

            //populando dados do appsettings.Dev na APIConfiguration 
            builder.Services.Configure<ApiConfiguration>(
                builder.Configuration.GetSection(ApiConfiguration.ConfigName));

            return builder;
        }

        public static WebApplication UseMvcConfiguration(this  WebApplication app) {//mais generico - recebendo config de várias responsabildiades da app
            if (app.Environment.IsDevelopment())
            {

            }
            else
            {
                app.UseHsts(); //Adiciona um Hearder no Request, dizendo pro browser � obrigado a trabalhar no https / Usar HTTPS -  Uma vez implementado a aplica��o n�o vai funcionar HTTP
            }

            app.UseHttpsRedirection();//adiciona um middleware que pega quando chamar um site HTTP e muda pra HTTPS / Usar HTTPS

            app.UseStaticFiles();//Add Uso de Arquivos estaticos - wwwroot

            app.UseRouting();//Add Uso de Rotas

            app.UseAuthorization();//Uso de Authorization - Identity


            //Rota de �reas especializada
            app.MapAreaControllerRoute("AreaProdutos", "Produtos", "Produtos/{controller=Cadastro}/{action=Index}/{id?}");
            app.MapAreaControllerRoute("AreaVendas", "Vendas", "Vendas/{controller=Gestao}/{action=Index}/{id?}");

            app.MapControllerRoute(//Add Rota Default
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var serviceScope = app.Services.CreateScope())//acessando Container de Di de uma objeto Singleton
            {
                var services = serviceScope.ServiceProvider;

                var singService = services.GetRequiredService<IOperacaoSingleton>();

                Console.WriteLine("Direto da Program.cs" + singService.OperacaoId);
            }


            app.MapRazorPages();

            return app;
        }
    }
}