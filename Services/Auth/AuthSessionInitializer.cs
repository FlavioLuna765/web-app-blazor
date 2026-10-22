namespace Fcg.Web.Services.Auth;

public sealed class AuthSessionInitializer
{
    private readonly AuthStorage _authStorage;
    private readonly IAuthSession _authSession;

    public AuthSessionInitializer(
        AuthStorage authStorage,
        IAuthSession authSession)
    {
        _authStorage = authStorage;
        _authSession = authSession;
    }

    public async Task InitializeAsync()
    {
        var tokenResponse = await _authStorage.GetAsync();

        if (tokenResponse is null)
            return;

        if (tokenResponse.Expiracao <= DateTime.UtcNow)
        {
            await _authStorage.RemoveAsync();
            _authSession.Clear();
            return;
        }

        _authSession.SetSession(tokenResponse);
    }
}
