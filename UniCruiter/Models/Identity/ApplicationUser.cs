using Microsoft.AspNetCore.Identity;

namespace UniCruiter.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        public string CompanyName { get; set; }
    }
}
