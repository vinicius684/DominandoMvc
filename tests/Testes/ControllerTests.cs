using AppSemTemplate.Controllers;
using AppSemTemplate.Data;
using AppSemTemplate.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;
using System;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Testes
{
    public class ControllerTests
    {
        [Fact]//"Fato que tenho que comprovar"
        public void TesteController_Index_Sucesso()
        {
            //Arrange - preparar os objetos; criar as instancias; Popular as entidades; Fazer os Mocs
            var controller = new TesteController();

            //Act - Ação(executar aquilo que quer testar)
            var result = controller.Index();

            //Assert - validar se o restultado foi o esperado ou não
            Assert.IsType<PartialViewResult>(result);
        }

        [Fact]
        public void ProdutoController_Index_Sucesso()
        {
            /*Resumo - 
               criado dbContext inMemory -> populado. Agr já consigo validar o resturn -> Db Context não é nulo e retorna uma View
            
               Resto do código validar o HttpContext.Identity.User
             */

            // Arrange

            // Dbcontext Options
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Contexto
            var ctx = new ApplicationDbContext(options);//injeção de dependencia da controller

            ctx.Produto.Add(new Produto { Id = 1, Nome = "Produto 1", Valor = 10m });//validar se não é nulo o dbContext
            ctx.Produto.Add(new Produto { Id = 2, Nome = "Produto 2", Valor = 10m });
            ctx.Produto.Add(new Produto { Id = 3, Nome = "Produto 3", Valor = 10m });
            ctx.SaveChanges();



            //Validar o var user = HttpContext.User.Identity; - validação na depuração
            // Identity
            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.Setup(m => m.Name).Returns("teste@teste.com");

            var principal = new ClaimsPrincipal(mockClaimsIdentity.Object);

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.User).Returns(principal);

            var imgService = new Mock<IImageUploadService>();//?

            // Controller
            var controller = new ProdutosController(ctx, imgService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockContext.Object
                }
            };
            //fim validação User identity

            // Act         
            var result = controller.Index().Result; //.Result é o resultado da task



            // Assert
            Assert.IsType<ViewResult>(result);//validar se é uma view

        }

        [Fact]
        public void ProdutoController_CriarNovoProduto_Sucess()
        {

            // Arrange

            // Dbcontext Options
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Contexto
            var ctx = new ApplicationDbContext(options);//injeção de dependencia da controller

            //Iformfile
            var fileMock = new Mock<IFormFile>();
            var fileName = "test.jpg";
            fileMock.Setup(_ => _.FileName).Returns(fileName);

            //Img Service
            var imgService = new Mock<IImageUploadService>();

            imgService.Setup(s => s.UploadArquivo(
                new ModelStateDictionary(),
                fileMock.Object,
                It.IsAny<string>()
                )).ReturnsAsync(true);

            // Controller
            var controller = new ProdutosController(ctx, imgService.Object);

            //produto
            var produto = new Produto
            {
                Id = 1,
                ImagemUpload = fileMock.Object,
                Nome = "Teste",
                Valor = 50
            };

            // Act         
            var result = controller.CriarNovoProduto(produto).Result; //.Result é o resultado da task



            // Assert
            Assert.IsType<RedirectToActionResult>(result);//validar se é uma view
        }
    }
}