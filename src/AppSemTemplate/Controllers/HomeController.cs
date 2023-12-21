using AppSemTemplate.Configuration;
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
    }
}
