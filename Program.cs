using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Fcg.Web;
using Fcg.Web.Services.Auth;
using Fcg.Web.Services.Catalogo;
using Fcg.Web.Services.Checkout;
using Fcg.Web.Services.Carrinho;
using Fcg.Web.Services.Usuario;
using Fcg.Web.Services.Biblioteca;
using Fcg.Web.Services.Perfil;
using Fcg.Web.Services.Avaliacoes;
using Fcg.Web.Services.AdminJogos;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri("http://localhost:8081/")
    });

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthSession, AuthSession>();
builder.Services.AddScoped<AuthStorage>();
builder.Services.AddScoped<AuthSessionInitializer>();

builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();
builder.Services.AddScoped<ICadastroUsuarioService, CadastroUsuarioService>();
builder.Services.AddScoped<IUsuarioAtualService, UsuarioAtualService>();

builder.Services.AddScoped<JwtAuthorizationHandler>();

builder.Services.AddScoped<IPerfilService>(sp =>
{
    var authSession = sp.GetRequiredService<IAuthSession>();

    var handler = new JwtAuthorizationHandler(authSession)
    {
        InnerHandler = new HttpClientHandler()
    };

    var httpClient = new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:8081/")
    };

    return new PerfilService(httpClient);
});

builder.Services.AddScoped<ICatalogoService>(sp =>
{
    var authSession = sp.GetRequiredService<IAuthSession>();

    var handler = new JwtAuthorizationHandler(authSession)
    {
        InnerHandler = new HttpClientHandler()
    };

    var httpClient = new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:8082/")
    };

    return new CatalogoService(httpClient);
});

builder.Services.AddScoped<IAvaliacaoService>(sp =>
{
    var authSession = sp.GetRequiredService<IAuthSession>();

    var handler = new JwtAuthorizationHandler(authSession)
    {
        InnerHandler = new HttpClientHandler()
    };

    var httpClient = new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:8082/")
    };

    return new AvaliacaoService(httpClient);
});

builder.Services.AddScoped<ICheckoutService>(sp =>
{
    var authSession = sp.GetRequiredService<IAuthSession>();

    // Cliente autenticado para os endpoints de catálogo e pedidos.
    var handler = new JwtAuthorizationHandler(authSession)
    {
        InnerHandler = new HttpClientHandler()
    };

    var catalogClient = new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:8082/")
    };

    // Cliente separado para a API de pagamentos.
    // O PaymentsController atual não exige autenticação por JWT.
    var paymentsClient = new HttpClient
    {
        BaseAddress = new Uri("http://localhost:8083/")
    };

    var usuarioAtualService =
        sp.GetRequiredService<IUsuarioAtualService>();

    return new CheckoutService(
        catalogClient,
        paymentsClient,
        usuarioAtualService);
});
builder.Services.AddScoped<IBibliotecaService>(sp =>
{
    var authSession = sp.GetRequiredService<IAuthSession>();

    var handler = new JwtAuthorizationHandler(authSession)
    {
        InnerHandler = new HttpClientHandler()
    };

    var httpClient = new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:8082/")
    };

    return new BibliotecaService(httpClient);
});


builder.Services.AddScoped<IAdminJogosService>(sp =>
{
    var authSession = sp.GetRequiredService<IAuthSession>();

    var handler = new JwtAuthorizationHandler(authSession)
    {
        InnerHandler = new HttpClientHandler()
    };

    var httpClient = new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:8082/")
    };

    return new AdminJogosService(httpClient);
});
var host = builder.Build();

var authSessionInitializer =
    host.Services.GetRequiredService<AuthSessionInitializer>();

await authSessionInitializer.InitializeAsync();

await host.RunAsync();
