namespace Locerp.Web.Services;

public interface IViaCepAddressLookup
{
    Task<ViaCepAddress?> FindAddressAsync(string postalCode, CancellationToken cancellationToken = default);
}

public sealed record ViaCepAddress(
    string PostalCode,
    string Street,
    string Neighborhood,
    string City,
    string State);
