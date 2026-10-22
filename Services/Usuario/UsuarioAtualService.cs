using System.Text;
using System.Text.Json;
using Fcg.Web.Services.Auth;

namespace Fcg.Web.Services.Usuario;

public sealed class UsuarioAtualService : IUsuarioAtualService
{
    private readonly IAuthSession _authSession;

    public UsuarioAtualService(IAuthSession authSession)
    {
        _authSession = authSession;
    }

    public string? ObterId()
    {
        var token = _authSession.Token;

        if (string.IsNullOrWhiteSpace(token))
            return null;

        var partes = token.Split('.');

        if (partes.Length != 3)
            return null;

        try
        {
            var payload = partes[1]
                .Replace('-', '+')
                .Replace('_', '/');

            payload = payload.PadRight(
                payload.Length + (4 - payload.Length % 4) % 4,
                '=');

            var bytes = Convert.FromBase64String(payload);
            var json = Encoding.UTF8.GetString(bytes);

            using var documento = JsonDocument.Parse(json);

            if (documento.RootElement.TryGetProperty("sub", out var sub)
                && sub.ValueKind == JsonValueKind.String)
            {
                return sub.GetString();
            }
        }
        catch (FormatException)
        {
            return null;
        }
        catch (JsonException)
        {
            return null;
        }

        return null;
    }
}
