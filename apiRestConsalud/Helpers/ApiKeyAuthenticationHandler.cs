using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>, IAuthenticationHandler
{
    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IConfiguration configuration)
        : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Obtener la API Key del encabezado de la solicitud
        var apiKey = Context.Request.Headers["ApiKey"].FirstOrDefault();

        // Validar la API Key (sustituir con tu lógica de validación real)
        var validApiKey = _configuration["ApiKey"]; // Obtener la clave de API desde la configuración

        if (string.IsNullOrEmpty(apiKey) || apiKey != validApiKey)
        {
            // Devolver un AuthenticateResult con el fallo y un mensaje personalizado
            var properties = new AuthenticationProperties();
            properties.Items["error_message"] = "Invalid API Key";

            return AuthenticateResult.Fail("Authentication failed.");
        }

        // Configurar la identidad del usuario autenticado (puedes personalizar esto)
        var claims = new[] { new Claim(ClaimTypes.Name, "api-key-user") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
