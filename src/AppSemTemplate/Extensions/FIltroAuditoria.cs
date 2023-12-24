using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppSemTemplate.Extensions
{
    public class FIltroAuditoria : IActionFilter
    {
        private readonly ILogger<FIltroAuditoria> _logger;

        public FIltroAuditoria(ILogger<FIltroAuditoria> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)//Log de algo que já acopnteceu
        {
            if (context.HttpContext.User.Identity.IsAuthenticated) 
            {
                var message = context.HttpContext.User.Identity.Name + " Acessou: " + context.HttpContext.Request.GetDisplayUrl();

                _logger.LogWarning(message);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)//Na hora que estiver executando, interferir em algum comportamento
        {
            //O que fazer
        }
    }
}
