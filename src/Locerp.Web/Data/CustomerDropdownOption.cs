using System.ComponentModel.DataAnnotations;

namespace Locerp.Web.Data;

public class CustomerDropdownOption
{
    public int Id { get; set; }

    public CustomerDropdownOptionKind Kind { get; set; }

    [MaxLength(80)]
    public string Name { get; set; } = "";

    [MaxLength(80)]
    public string NormalizedName { get; set; } = "";

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public enum CustomerDropdownOptionKind
{
    CustomerOrigin = 1,
    AddressType = 2
}
