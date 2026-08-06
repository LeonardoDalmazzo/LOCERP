using System.ComponentModel.DataAnnotations;

namespace Locerp.Web.Data;

public class Customer
{
    public int Id { get; set; }

    public CustomerType Type { get; set; } = CustomerType.Individual;

    public CustomerStatus Status { get; set; } = CustomerStatus.Active;

    [MaxLength(14)]
    public string DocumentNumber { get; set; } = "";

    [MaxLength(160)]
    public string Name { get; set; } = "";

    [MaxLength(256)]
    public string? Email { get; set; }

    [MaxLength(30)]
    public string? BusinessPhone { get; set; }

    [MaxLength(30)]
    public string? MobilePhone { get; set; }

    [MaxLength(200)]
    public string? Website { get; set; }

    [MaxLength(160)]
    public string? CompanyName { get; set; }

    [MaxLength(80)]
    public string Origin { get; set; } = CustomerOrigin.Site.ToString();

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public string SellerId { get; set; } = "";

    public ApplicationUser Seller { get; set; } = default!;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAt { get; set; }

    public ICollection<CustomerAddress> Addresses { get; set; } = [];
}

public enum CustomerType
{
    Individual = 1,
    Company = 2
}

public enum CustomerStatus
{
    Active = 1,
    Inactive = 2
}

public enum CustomerOrigin
{
    Site = 1,
    Instagram = 2,
    Referral = 3,
    Builder = 4,
    Facebook = 5,
    FormerCustomer = 6
}
