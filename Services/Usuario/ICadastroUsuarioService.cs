using Fcg.Web.DTOs.Usuario;

namespace Fcg.Web.Services.Usuario;

public interface ICadastroUsuarioService
{
    Task CadastrarAsync(CriarUsuarioRequestDto request);
}
