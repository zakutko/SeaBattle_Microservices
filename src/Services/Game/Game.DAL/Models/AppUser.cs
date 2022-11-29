using Microsoft.AspNetCore.Identity;

namespace Game.DAL.Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public bool? IsHit { get; set; }
    }
}
