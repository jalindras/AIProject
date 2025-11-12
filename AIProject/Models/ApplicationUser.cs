using System;
using Microsoft.AspNetCore.Identity;

namespace AIProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; } = new DateTime(2000, 1, 1);

        public string Department { get; set; } = string.Empty;
    }
}
