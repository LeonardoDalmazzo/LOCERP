using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Locerp.Web.Services;

public sealed class ViaCepAddressLookup(HttpClient httpClient, ILogger<ViaCepAddressLookup> logger) : IViaCepAddressLookup
{
    public async Task<ViaCepAddress?> FindAddressAsync(string postalCode, CancellationToken cancellationToken = default)
    {
        var digits = new string(postalCode.Where(char.IsDigit).ToArray());
        if (digits.Length != 8)
        {
            return null;
        }

        try
        {
            var response = await httpClient.GetFromJsonAsync<ViaCepResponse>($"ws/{digits}/json/", cancellationToken);
            if (response is null || response.Erro)
            {
                return null;
            }

            return new ViaCepAddress(
                digits,
                response.Logradouro ?? "",
                response.Bairro ?? "",
                response.Localidade ?? "",
                response.Uf ?? "");
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or NotSupportedException or JsonException)
        {
            logger.LogWarning(exception, "Nao foi possivel consultar o CEP {PostalCode}.", digits);
            return null;
        }
    }

    private sealed class ViaCepResponse
    {
        [JsonPropertyName("erro")]
        public bool Erro { get; set; }

        [JsonPropertyName("logradouro")]
        public string? Logradouro { get; set; }

        [JsonPropertyName("bairro")]
        public string? Bairro { get; set; }

        [JsonPropertyName("localidade")]
        public string? Localidade { get; set; }

        [JsonPropertyName("uf")]
        public string? Uf { get; set; }
    }
}
