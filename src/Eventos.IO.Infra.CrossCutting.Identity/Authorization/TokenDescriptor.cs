namespace Eventos.IO.Infra.CrossCutting.Identity.Authorization
{
    // Token(standard 2.0)
    public class TokenDescriptor
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int MinutesValid { get; set; }
    }
}