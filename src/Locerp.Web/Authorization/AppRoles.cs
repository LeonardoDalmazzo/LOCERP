namespace Locerp.Web.Authorization;

public static class AppRoles
{
    public const string Administrador = "Administrador";
    public const string Gerente = "Gerente";
    public const string Vendedor = "Vendedor";

    public const string Gestao = Administrador + "," + Gerente;
    public const string Operacao = Administrador + "," + Gerente + "," + Vendedor;

    public static readonly string[] All =
    [
        Administrador,
        Gerente,
        Vendedor
    ];
}
