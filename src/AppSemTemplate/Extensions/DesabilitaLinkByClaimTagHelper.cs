using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AppSemTemplate.Extensions
{
    [HtmlTargetElement("*", Attributes = "disable-by-claim-name")]
    [HtmlTargetElement("*", Attributes = "disable-by-claim-value")]
    public class DesabilitaLinkByClaimTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public DesabilitaLinkByClaimTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("disable-by-claim-name")]
        public string? IdentityClaimName { get; set; }

        [HtmlAttributeName("disable-by-claim-value")]
        public string? IdentityClaimValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            var temAcesso = CustomAuthorization.ValidarClaimsUsuario(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (temAcesso) return;

            output.Attributes.RemoveAll("href");
            output.Attributes.Add(new TagHelperAttribute("style", "cursor: not-allowed"));
            output.Attributes.Add(new TagHelperAttribute("title", "Você não tem permissão"));
        }
    }
}
