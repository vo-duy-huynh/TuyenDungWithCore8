using Microsoft.AspNetCore.Identity;

namespace TuyenDungWeb.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTimeOffset> LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Discriminator { get; set; }
        public string FullName { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public Nullable<System.Guid> CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
