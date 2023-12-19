using AppSemTemplate.Data;
using AppSemTemplate.Extensions;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(options => {//Declara o MVC j� utilizando Gobalmente o ValidateAntiforgeryToken (prote��o contra Ataque CSRF)
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

//builder.Services.AddRouting(options =>
//    options.ConstraintMap["slugify"] = typeof(RouteSlugifyParameterTransformer));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//builder.Services.AddScoped<IOperacao, Operacao>();
builder.Services.AddTransient<IOperacaoTransient, Operacao>();
builder.Services.AddScoped<IOperacaoScoped, Operacao>();
builder.Services.AddSingleton<IOperacaoSingleton, Operacao>();
builder.Services.AddSingleton<IOperacaoSingletonInstance>(new Operacao(Guid.Empty));

builder.Services.AddTransient<OperacaoService>();

builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

}
else
{
    app.UseHsts(); //Adiciona um Hearder no Request, dizendo pro browser � obrigado a trabalhar no https / Usar HTTPS -  Uma vez implementado a aplica��o n�o vai funcionar HTTP
}

app.UseHttpsRedirection();//adiciona um middleware que pega quando chamar um site HTTP e muda pra HTTPS / Usar HTTPS

app.UseStaticFiles();

app.UseRouting();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller:slugify=Home}/{action:slugify=Index}/{id?}");

//app.MapControllerRoute(//Add Rota Default para Areas - (n�o necess�rio se usar especializada)
//            name: "areas",
//            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");//-Adicionando Rota da Arena na Program

//Rota de �reas especializada
app.MapAreaControllerRoute("AreaProdutos", "Produtos", "Produtos/{controller=Cadastro}/{action=Index}/{id?}");
app.MapAreaControllerRoute("AreaVendas", "Vendas", "Vendas/{controller=Gestao}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var serviceScope = app.Services.CreateScope())//acessando Container de Di de uma objeto Singleton
{
    var services = serviceScope.ServiceProvider;

    var singService = services.GetRequiredService<IOperacaoSingleton>();

    Console.WriteLine("Direto da Program.cs" + singService.OperacaoId);
}

app.Run();

