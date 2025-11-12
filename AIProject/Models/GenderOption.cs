using System.ComponentModel.DataAnnotations;

namespace AIProject.Models
{
    public class GenderOption
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
