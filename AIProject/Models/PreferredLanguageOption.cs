using System.ComponentModel.DataAnnotations;

namespace AIProject.Models
{
    public class PreferredLanguageOption
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
