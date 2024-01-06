using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppSemTemplate.Services
{
    public interface IImageUploadService
    {
        public Task<bool> UploadArquivo(ModelStateDictionary modelstate, IFormFile arquivo, string imgPrefixo);
    }
}