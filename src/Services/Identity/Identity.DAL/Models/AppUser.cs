using Microsoft.AspNetCore.Identity;

namespace Identity.DAL.Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public bool? IsHit { get; set; }
    }
}
