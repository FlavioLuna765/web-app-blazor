using Fcg.Web.DTOs.Usuario;
using Microsoft.AspNetCore.Components.Forms;

namespace Fcg.Web.Services.Perfil;

public interface IPerfilService
{
    Task<UsuarioResponseDto> ObterAsync(
        string usuarioId,
        CancellationToken cancellationToken = default);

    Task<UsuarioResponseDto> AtualizarAsync(
        string usuarioId,
        AtualizarUsuarioRequestDto request,
        CancellationToken cancellationToken = default);

    Task<UsuarioResponseDto> AtualizarFotoAsync(
        string usuarioId,
        IBrowserFile arquivo,
        CancellationToken cancellationToken = default);
}
