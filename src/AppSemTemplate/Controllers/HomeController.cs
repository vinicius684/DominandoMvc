﻿using AppSemTemplate.Configuration;
using AppSemTemplate.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace AppSemTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration Configuration;//injeção
        private readonly ApiConfiguration ApiConfig;
        private readonly ILogger<HomeController> Logger;
        private readonly IStringLocalizer<HomeController> _localizer;//injeção p usar o localzier

        public HomeController(IConfiguration configuration,
                                IOptions<ApiConfiguration> apiConfig, 
                                ILogger<HomeController> logger,
                                IStringLocalizer<HomeController> localizer) 
        {
            Configuration = configuration;
            ApiConfig = apiConfig.Value;
            Logger = logger;
            _localizer = localizer;
        }

        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, NoStore =false )]//segundos, location, 
        public IActionResult Index()
        {
            //Mensagem dos tipos de log
            Logger.LogInformation("Information");
            Logger.LogCritical("Critical");
            Logger.LogWarning("Warning");
            Logger.LogError("Error");

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

            ViewData["Message"] = _localizer["Seja bem vindo!"];

            ViewData["Horario"] = DateTime.Now;// para ver ex do cache

            //acessando dados do cookie por meio da controller
            if (Request.Cookies.TryGetValue("MeuCookie", out string? cookieValue))//request que está dentro da classe Controller - metodo TryGet: tenta pegar os dados ou trata falha - parâmetros: Nome do cookie, out string saída_do_método)
            {
                ViewData["MeuCookie"] = cookieValue;
            }

            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)//criar um cookie onde vai guardar a cultura e definir a expiração desse cookie em 1 ano
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,//nome
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),//valor do cookie
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }//duração
            );

            return LocalRedirect(returnUrl);
        }

        [Route("cookies")]
        public IActionResult Cookie()
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1)//duração
            };

            Response.Cookies.Append("MeuCookie", "Dados do Cookie", cookieOptions);//mesmos parâmetros do Append do SetLanguage, porém passados de forma diferente 

            return View();
        }

        [Route("teste")]
        public IActionResult Teste()//view preparada para dar o máximo de info possível para o dev - tratamento de erros ambiente desenvolvimento(config na MvcCOnfig)
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
