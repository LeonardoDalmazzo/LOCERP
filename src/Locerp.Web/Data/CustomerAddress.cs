using System.ComponentModel.DataAnnotations;

namespace Locerp.Web.Data;

public class CustomerAddress
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = default!;

    [MaxLength(80)]
    public string Type { get; set; } = CustomerAddressType.Residential.ToString();

    [MaxLength(8)]
    public string PostalCode { get; set; } = "";

    [MaxLength(160)]
    public string Street { get; set; } = "";

    [MaxLength(20)]
    public string Number { get; set; } = "";

    [MaxLength(100)]
    public string Neighborhood { get; set; } = "";

    [MaxLength(100)]
    public string City { get; set; } = "";

    [MaxLength(2)]
    public string State { get; set; } = "";

    [MaxLength(120)]
    public string? Complement { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public enum CustomerAddressType
{
    Residential = 1,
    Business = 2,
    JobSite = 3
}
