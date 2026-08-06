using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Locerp.Web.Data;

public class ApplicationUser : IdentityUser
{
    [MaxLength(120)]
    public string DisplayName { get; set; } = "";

    public bool IsActive { get; set; } = true;

    public bool MustChangePassword { get; set; }

    public DateTimeOffset? TemporaryPasswordExpiresAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

