using System.ComponentModel.DataAnnotations;

namespace AIProject.Models.RoleViewModels
{
    public class RoleFormViewModel
    {
        public string? Id { get; set; }

        [Required]
        [Display(Name = "Role name")]
        public string Name { get; set; } = string.Empty;
    }
}
