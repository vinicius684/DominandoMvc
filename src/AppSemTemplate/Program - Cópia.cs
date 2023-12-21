/////////////Arquivo criado com intuito de deixr guardada a classe Program antes das sua Customização / customização de algumas responsabilidades em métodos independentes

//using AppSemTemplate.Data;
//using AppSemTemplate.Extensions;
//using AppSemTemplate.Services;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Razor;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

////builder.Services.AddControllersWithViews();
//builder.Services.AddControllersWithViews(options => {//Declara o MVC j� utilizando Gobalmente o ValidateAntiforgeryToken (prote��o contra Ataque CSRF)
//    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
//});

//// Adicionando suporte a mudan�a de conven��o da rota das areas.
//builder.Services.Configure<RazorViewEngineOptions>(options =>
//{
//    options.AreaViewLocationFormats.Clear();
//    options.AreaViewLocationFormats.Add("/Modulos_AreaComOutroNome/{2}/Views/{1}/{0}.cshtml");
//    options.AreaViewLocationFormats.Add("/Modulos_AreaComOutroNome/{2}/Views/Shared/{0}.cshtml");
//    options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
//});


//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();// inje��o de dependencia do TagHelper

////inje��es de dependencia 
////builder.Services.AddScoped<IOperacao, Operacao>(); //A interface que implementa e a Classe - Toda vez que ver um IOperacao, saiba que quem a Implementa � a classe Operacao
////exemplos ciclos de vida diferentes
//builder.Services.AddTransient<IOperacaoTransient, Operacao>();
//builder.Services.AddScoped<IOperacaoScoped, Operacao>();
//builder.Services.AddSingleton<IOperacaoSingleton, Operacao>();
//builder.Services.AddSingleton<IOperacaoSingletonInstance>(new Operacao(Guid.Empty));
//builder.Services.AddTransient<OperacaoService>();


//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();/*Filtro de erros do DB*/

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddRoles<IdentityRole>()//Suporte a roles - "Niveis" de autorização
//    .AddEntityFrameworkStores<ApplicationDbContext>();/*Add Identity/Dizendo que o Identity vai consumir esse contexto*/

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("PodeExcluirPermanentemente", policy =>
//        policy.RequireRole("Admin"));

//    options.AddPolicy("VerProdutos", policy =>
//        policy.RequireClaim("Produtos", "VI"));//Adicionando Claim atraves de policy
//}
//);

//////isso tem a ver com RouteSlugFy - transformador de par�metro
////builder.Services.AddRouting(options => 
////        options.ConstraintMap["slugify"] = typeof(RouteSlugifyParameterTransformer));


////outras configura��es HSTS - RElacionado a HTTPS
//builder.Services.AddHsts(options =>
//{
//    options.Preload = true;
//    options.IncludeSubDomains = true;
//    options.MaxAge = TimeSpan.FromDays(60);
//    options.ExcludedHosts.Add("example.com");//imputadr hosts que n�o quero o HSTS
//    options.ExcludedHosts.Add("www.example.com");
//});

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{

//}
//else
//{
//    app.UseHsts(); //Adiciona um Hearder no Request, dizendo pro browser � obrigado a trabalhar no https / Usar HTTPS -  Uma vez implementado a aplica��o n�o vai funcionar HTTP
//}

//app.UseHttpsRedirection();//adiciona um middleware que pega quando chamar um site HTTP e muda pra HTTPS / Usar HTTPS

//app.UseStaticFiles();//Add Uso de Arquivos estaticos - wwwroot

//app.UseRouting();//Add Uso de Rotas

//app.UseAuthorization();//Uso de Authorization - Identity

////app.MapControllerRoute(//RotaSlugify
////    name: "default",
////    pattern: "{controller:slugify=Home}/{action:slugify=Index}/{id?}");

////app.MapControllerRoute(//Add Rota Default para Areas - (n�o necess�rio se usar especializada)
////            name: "areas",
////            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");//-Adicionando Rota da Arena na Program

////Rota de �reas especializada
//app.MapAreaControllerRoute("AreaProdutos", "Produtos", "Produtos/{controller=Cadastro}/{action=Index}/{id?}");
//app.MapAreaControllerRoute("AreaVendas", "Vendas", "Vendas/{controller=Gestao}/{action=Index}/{id?}");

//app.MapControllerRoute(//Add Rota Default
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//using (var serviceScope = app.Services.CreateScope())//acessando Container de Di de uma objeto Singleton
//{
//    var services = serviceScope.ServiceProvider;

//    var singService = services.GetRequiredService<IOperacaoSingleton>();

//    Console.WriteLine("Direto da Program.cs" + singService.OperacaoId);
//}


//app.MapRazorPages();
//app.Run();

////site.com/meus-pedidos/pedidos-cancelados