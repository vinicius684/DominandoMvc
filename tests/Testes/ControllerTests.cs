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

namespace Testes
{
    public class ControllerTests
    {
        [Fact]//"Fato que tenho que comprovar"
        public void TesteController_Index_Sucesso()
        {
            //Arrange - preparar os objetos; criar as instancias; Popular as entidades; Fazer os Mocs
            var controller = new TesteController();

            //Act - A��o(executar aquilo que quer testar)
            var result = controller.Index();

            //Assert - validar se o restultado foi o esperado ou n�o
            Assert.IsType<PartialViewResult>(result);
        }

        [Fact]
        public void ProdutoController_Index_Sucesso()
        {
            /*Resumo - 
               criado dbCOntext inMemory - 
               populado. Agr j� consigo validar o resturn - 
               Db Context n�o � nulo e retorna uma View
            
                Resto do c�digo validar o HttpContext.Identity
             */

            // Arrange

            // Dbcontext Options
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Contexto
            var ctx = new ApplicationDbContext(options);//inje��o de dependencia da controller

            ctx.Produto.Add(new Produto { Id = 1, Nome = "Produto 1", Valor = 10m });//validar se n�o � nulo o dbContext
            ctx.Produto.Add(new Produto { Id = 2, Nome = "Produto 2", Valor = 10m });
            ctx.Produto.Add(new Produto { Id = 3, Nome = "Produto 3", Valor = 10m });
            ctx.SaveChanges();



           //Validar o var user = HttpContext.User.Identity; - valida��o na depura��o
            // Identity
            var mockClaimsIdentity = new Mock<ClaimsIdentity>(); 
            mockClaimsIdentity.Setup(m => m.Name).Returns("teste@teste.com"); 

            var principal = new ClaimsPrincipal(mockClaimsIdentity.Object);

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.User).Returns(principal);

            // Controller
            var controller = new ProdutosController(ctx)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockContext.Object
                }
            };
            //fim valida��o User identity

            // Act         
            var result = controller.Index().Result; //.Result � o resultado da task



            // Assert
            Assert.IsType<ViewResult>(result);//validar se � uma view






        }

    }
}