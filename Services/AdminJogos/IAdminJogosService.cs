using Fcg.Web.DTOs.Catalogo;
using Microsoft.AspNetCore.Components.Forms;

namespace Fcg.Web.Services.AdminJogos;

public interface IAdminJogosService
{
    Task<JogoResponseDto> CriarAsync(
        CriarJogoRequestDto dto,
        CancellationToken cancellationToken = default);

    Task<JogoResponseDto> AtualizarAsync(
        string id,
        AtualizarJogoRequestDto dto,
        CancellationToken cancellationToken = default);

    Task DesativarAsync(
        string id,
        CancellationToken cancellationToken = default);

    Task<string> UploadImagemAsync(
        IBrowserFile arquivo,
        CancellationToken cancellationToken = default);
}
