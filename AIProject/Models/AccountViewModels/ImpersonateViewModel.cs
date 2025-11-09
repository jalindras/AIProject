namespace AIProject.Models.AccountViewModels
{
    public class ImpersonateViewModel
    {
        public IList<UserSummaryViewModel> Users { get; set; } = new List<UserSummaryViewModel>();
        public string? SelectedUserId { get; set; }
        public string AdminEmail { get; set; } = string.Empty;
    }

    public class UserSummaryViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
    }
}
