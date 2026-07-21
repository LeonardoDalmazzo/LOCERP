namespace Locerp.Web.Authorization;

public static class AppRoles
{
    public const string Administrador = "Administrador";
    public const string Gerente = "Gerente";
    public const string Operador = "Operador";

    public const string Gestao = Administrador + "," + Gerente;
    public const string Operacao = Administrador + "," + Gerente + "," + Operador;

    public static readonly string[] All =
    [
        Administrador,
        Gerente,
        Operador
    ];
}
