using Microsoft.AspNetCore.Identity;

namespace DatingAppAPI.Entities;

public class UserRole : IdentityUserRole<int>
{
    public User User { get; set; }
    public Role Role { get; set; }
}