using System.Net.Http.Headers;

namespace Fcg.Web.Services.Auth;

public sealed class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly IAuthSession _authSession;

    public JwtAuthorizationHandler(IAuthSession authSession)
    {
        _authSession = authSession;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"[JWT] Token existe: {!string.IsNullOrWhiteSpace(_authSession.Token)}");

        if (!string.IsNullOrWhiteSpace(_authSession.Token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _authSession.Token);

            Console.WriteLine("[JWT] Authorization Bearer adicionado.");
        }
        else
        {
            Console.WriteLine("[JWT] ATENÇÃO: token não encontrado.");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
