using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AppSemTemplate.Extensions
{
    [HtmlTargetElement("*", Attributes = "tipo-botao, route-id")]
    public class BotaoTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public BotaoTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("tipo-botao")]
        public TipoBotao TipoBotaoSelecao { get; set; }

        [HtmlAttributeName("route-id")]
        public int RouteId { get; set; }

        private string? nomeAction;
        private string? nomeClasse;
        private string? spanIcone;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            switch (TipoBotaoSelecao)
            {
                case TipoBotao.Detalhes:
                    nomeAction = "Details";
                    nomeClasse = "btn btn-info";
                    spanIcone = "fa fa-search";
                    break;
                case TipoBotao.Editar:
                    nomeAction = "Edit";
                    nomeClasse = "btn btn-warning";
                    spanIcone = "fa fa-pencil-alt";
                    break;
                case TipoBotao.Excluir:
                    nomeAction = "Delete";
                    nomeClasse = "btn btn-danger";
                    spanIcone = "fa fa-trash";
                    break;
            }

            var controller = _contextAccessor.HttpContext?.GetRouteData().Values["controller"]?.ToString();

            output.TagName = "a";
            output.Attributes.SetAttribute("href", $"{controller}/{nomeAction}/{RouteId}");
            output.Attributes.SetAttribute("class", nomeClasse);

            var iconSpan = new TagBuilder("span");
            iconSpan.AddCssClass(spanIcone);

            output.Content.AppendHtml(iconSpan);
        }

    }

    public enum TipoBotao
    {
        Detalhes = 1,
        Editar,
        Excluir
    }
}
