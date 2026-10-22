using Fcg.Web.DTOs.Catalogo;

namespace Fcg.Web.Services.Biblioteca;

public interface IBibliotecaService
{
    Task<IReadOnlyList<JogoResponseDto>> ListarAsync(
        CancellationToken cancellationToken = default);
}
