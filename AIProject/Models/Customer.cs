using System;
using System.ComponentModel.DataAnnotations;

namespace AIProject.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(200)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(40)]
        public string? PhoneNumber { get; set; }

        [Phone]
        [StringLength(40)]
        public string? AlternatePhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(50)]
        public string? Gender { get; set; }

        [StringLength(250)]
        public string? CompanyName { get; set; }

        [StringLength(250)]
        public string? JobTitle { get; set; }

        [StringLength(100)]
        public string? Industry { get; set; }

        [Url]
        [StringLength(250)]
        public string? WebsiteUrl { get; set; }

        [StringLength(250)]
        public string? AddressLine1 { get; set; }

        [StringLength(250)]
        public string? AddressLine2 { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? StateOrProvince { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(100)]
        public string? PreferredContactMethod { get; set; }

        [StringLength(100)]
        public string? PreferredContactTime { get; set; }

        [StringLength(100)]
        public string? PreferredLanguage { get; set; }

        [StringLength(100)]
        public string? LoyaltyNumber { get; set; }

        public bool MarketingOptIn { get; set; }

        public bool IsVip { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Anniversary { get; set; }

        [StringLength(2048)]
        public string? Notes { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAtUtc { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAtUtc { get; set; }
    }
}
