using AppSemTemplate.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppSemTemplate.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres")]
        public string? Nome { get; set; }

        [NotMapped]
        [DisplayName("Imagem do Produto")]
        public IFormFile? ImagemUpload { get; set; }//IFormFile - Representa um objeto enviado atravez de uma requisição - campo para upload de arquivo - Interessandante utilizar ViewModel(?) 

        public string? Imagem { get; set; }

        [Moeda]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal? Valor { get; set; }//validar o valor conforme nossa moeda

        public bool Processado { get; set; }
    }
}

