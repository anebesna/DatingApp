using Microsoft.AspNetCore.Identity;

namespace DatingAppAPI.Entities;

public class Role : IdentityRole<int>
{
    public ICollection<UserRole> UserRoles { get; set; }
}