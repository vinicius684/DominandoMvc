using AppSemTemplate.Configuration;
using AppSemTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AppSemTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration Configuration;//injeção
        private readonly ApiConfiguration ApiConfig;

        public HomeController(IConfiguration configuration,
            IOptions<ApiConfiguration> apiConfig) 
        {
            Configuration = configuration;
            ApiConfig = apiConfig.Value;
        }
        //

        public IActionResult Index()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");//Obtendo ambiente?

            //1 popular dadoss da appsettings.Dev na APIConfiguration
            var apiConfig = new ApiConfiguration();
            Configuration.GetSection(ApiConfiguration.ConfigName).Bind(apiConfig);

            var secret = apiConfig.UserSecret;
            //
            //2var user = Configuration[$"{ApiConfiguration.ConfigName}:UserKey"]; assim faria na mão sem usar classe(??) ourta forma de popular..

            //3Mais indicada

            //4?
            var domain = ApiConfig.Domain;

            return View();
        }

        [Route("teste")]
        public IActionResult Teste()
        {
            throw new Exception("ALGO ERRADO NÃO ESTAVA CERTO!");

            return View("Index");
        }


        [Route("erro/{id:length(3,3)}")]//id que tem que ter no minimo 3 e no maximo 3 caracteres, ou seja obrigatoriamente 3
        public IActionResult Errors(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelErro);
        }
    }
}
