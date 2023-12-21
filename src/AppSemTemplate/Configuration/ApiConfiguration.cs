namespace AppSemTemplate.Configuration
{
    public class ApiConfiguration //Possuir as propriedades daquilo que quero mapear - Propriedades da api, no caso
    {
        public const string ConfigName = "ApiConfiguration";

        public string? Domain { get; set; }
        public string? UserKey { get; set; }
        public string? UserSecret { get; set; }
    }
}
