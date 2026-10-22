using System.Text.Json;
using Microsoft.JSInterop;
using Fcg.Web.DTOs.Auth;

namespace Fcg.Web.Services.Auth;

public sealed class AuthStorage
{
    private const string StorageKey = "fcg_auth";

    private readonly IJSRuntime _jsRuntime;

    public AuthStorage(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SaveAsync(TokenResponseDto tokenResponse)
    {
        ArgumentNullException.ThrowIfNull(tokenResponse);

        var json = JsonSerializer.Serialize(tokenResponse);

        await _jsRuntime.InvokeVoidAsync(
            "localStorage.setItem",
            StorageKey,
            json);
    }

    public async Task<TokenResponseDto?> GetAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            StorageKey);

        if (string.IsNullOrWhiteSpace(json))
            return null;

        return JsonSerializer.Deserialize<TokenResponseDto>(json);
    }

    public async Task RemoveAsync()
    {
        await _jsRuntime.InvokeVoidAsync(
            "localStorage.removeItem",
            StorageKey);
    }
}
