using AppSemTemplate.Data;
using AppSemTemplate.Models;
using Microsoft.AspNetCore.Identity;

namespace AppSemTemplate.Configuration
{
    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)//garanta que os dados foram populados
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;//recuperação de uma dependencia do container, coontainer do Asp.net
            await EnsureSeedData(services);//passa o Metodo com o services
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();//obter o escopo
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();//com base no escopo, resgata do container de DI o IWebHostEnvironment, para pegar iformações do Environment

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();//resgatando a instancia do DBContext

            if (env.IsDevelopment() || env.IsEnvironment("Docker"))
            {
                await context.Database.EnsureCreatedAsync();//garanta que com base no contexto dos mapeamentos nas migration a base exista
                await EnsureSeedProducts(context);//criar os registros na base
            }
        }

        private static async Task EnsureSeedProducts(ApplicationDbContext context)
        {
            if (context.Produto.Any())//se já tem algo return; senão popule o banco
                return;

            await context.Produto.AddAsync(new Produto() { Nome = "Livro CSS", Imagem = "CSS.jpg", Valor = 50, Processado = false });
            await context.Produto.AddAsync(new Produto() { Nome = "Livro jQuery", Imagem = "JQuery.jpg", Valor = 150, Processado = false });
            await context.Produto.AddAsync(new Produto() { Nome = "Livro HTML", Imagem = "HTML.jpg", Valor = 90, Processado = false });
            await context.Produto.AddAsync(new Produto() { Nome = "Livro Razor", Imagem = "Razor.jpg", Valor = 50, Processado = false });

            await context.SaveChangesAsync();

            if (context.Users.Any())
                return;

            await context.Users.AddAsync(new IdentityUser()//se estiver em branco, criando usuário
            {
                Id = "ef0e0af2-1ba4-4519-b322-32dcb7041567",
                UserName = "teste@teste.com",
                NormalizedUserName = "TESTE@TESTE.COM",
                Email = "teste@teste.com",
                NormalizedEmail = "TESTE@TESTE.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEElLokEYWXScCl0LUbh96Thu6CBkwEZDaQQ0+8/D/bYsfBxYQB/NfRIsCcUk03+Wxg==",
                SecurityStamp = "R75GPOQ7C4IP7HI3CDIJ2WS3ELTP6KBV",
                ConcurrencyStamp = "0926d8fc-0b41-4837-9edb-6c1978c34066",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0
            });

            await context.SaveChangesAsync();

            if (context.UserClaims.Any())
                return;

            await context.UserClaims.AddAsync(new IdentityUserClaim<string>()//associando user a claim
            {
                UserId = "ef0e0af2-1ba4-4519-b322-32dcb7041567",
                ClaimType = "Produtos",
                ClaimValue = "VI,ED,AD,EX"
            });

            await context.SaveChangesAsync();
        }
    }
}
