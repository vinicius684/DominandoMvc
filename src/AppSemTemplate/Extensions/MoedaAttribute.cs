using AppSemTemplate.Configuration;
using AppSemTemplate.Controllers;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AppSemTemplate.Extensions
{
    public class MoedaAttribute : ValidationAttribute
    {
        //private readonly IStringLocalizer<MoedaAttribute> _localizer;//injeção p usar o localzier

        //public MoedaAttribute(IStringLocalizer<HomeController> localizer)
        //{
        //    _localizer = (IStringLocalizer<MoedaAttribute>?)localizer;
        //}

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)//validação da moeda no back
        {
            try
            {
                var moeda = Convert.ToDecimal(value, new CultureInfo("pt-BR"));
            }
            catch (Exception)
            {
                return new ValidationResult("Moeda em formato inválido");
            }

            return ValidationResult.Success;
        }
    }

    public class MoedaAttributeAdapter : AttributeAdapterBase<MoedaAttribute> 
    {

        public MoedaAttributeAdapter(MoedaAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {

        }

        public override void AddValidation(ClientModelValidationContext context) //validação no front
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-moeda", GetErrorMessage(context));//construida por nos
            MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(context));
        }
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return "Moeda em formato inválido";
        }
    }

    public class MoedaValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider //fazer com que funcione a validação do front
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();
        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer) //IStringLocalizer - string a ser informada com base na cultura
        {
            if (attribute is MoedaAttribute moedaAttribute)
            {
                return new MoedaAttributeAdapter(moedaAttribute, stringLocalizer);
            }

            return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }


}
