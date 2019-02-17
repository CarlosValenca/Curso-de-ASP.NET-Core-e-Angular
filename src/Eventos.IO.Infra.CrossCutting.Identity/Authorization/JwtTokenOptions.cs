using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace Eventos.IO.Infra.CrossCutting.Identity.Authorization
{
    public class JwtTokenOptions
    {
        // Emissor do Token
        public string Issuer { get; set; }
        // Assunto do Token
        public string Subject { get; set; }
        // Para qual site este Token é válido
        public string Audience { get; set; }
        // Não usar o Token antes de
        public DateTime NotBefore { get; set; } = DateTime.UtcNow;
        // Token emitido em
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        // Até quando este Token é válido
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromHours(5);
        // Data de expiração do Token
        public DateTime Expiration => IssuedAt.Add(ValidFor);
        // Token Generator via Guid
        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());
        // Credenciais do Token
        public SigningCredentials SigningCredentials { get; set; }
    }
}
