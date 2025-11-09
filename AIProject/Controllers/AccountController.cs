using System.Linq;
using System.Security.Claims;
using System.Text;
using AIProject.Models;
using AIProject.Models.AccountViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace AIProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} created a new account.", model.Email);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} logged in.", model.Email);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                TempData["ResetLink"] = null;
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var callbackUrl = Url.Action(
                nameof(ResetPassword),
                "Account",
                new { token = encodedToken, email = model.Email },
                protocol: Request.Scheme);

            TempData["ResetLink"] = callbackUrl;
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            var resetLink = TempData.ContainsKey("ResetLink") ? TempData["ResetLink"]?.ToString() : null;
            var viewModel = new ForgotPasswordConfirmationViewModel
            {
                ResetLink = resetLink
            };
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            string decodedToken;
            try
            {
                var decodedBytes = WebEncoders.Base64UrlDecode(token);
                decodedToken = Encoding.UTF8.GetString(decodedBytes);
            }
            catch (FormatException)
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = decodedToken
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Impersonate()
        {
            var currentUserId = _userManager.GetUserId(User);
            var model = new ImpersonateViewModel
            {
                AdminEmail = User.Identity?.Name ?? string.Empty
            };
            await PopulateImpersonationUsersAsync(model, currentUserId);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Impersonate(ImpersonateViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }

            model.AdminEmail = currentUser.Email ?? currentUser.UserName ?? string.Empty;
            await PopulateImpersonationUsersAsync(model, currentUser.Id);

            if (string.IsNullOrEmpty(model.SelectedUserId))
            {
                ModelState.AddModelError(string.Empty, "Please select a user to impersonate.");
                return View(model);
            }

            var targetUser = await _userManager.FindByIdAsync(model.SelectedUserId);
            if (targetUser == null)
            {
                ModelState.AddModelError(string.Empty, "The selected user does not exist.");
                return View(model);
            }

            await _signInManager.SignOutAsync();
            var principal = await _signInManager.CreateUserPrincipalAsync(targetUser);
            if (principal.Identity is ClaimsIdentity identity)
            {
                identity.AddClaim(new Claim("ImpersonatorId", currentUser.Id));
                identity.AddClaim(new Claim("ImpersonatorEmail", currentUser.Email ?? currentUser.UserName ?? string.Empty));
            }

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
            _logger.LogInformation("User {Admin} impersonated {Target}", currentUser.Email, targetUser.Email);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndImpersonation()
        {
            var impersonatorId = User.FindFirstValue("ImpersonatorId");
            if (string.IsNullOrEmpty(impersonatorId))
            {
                return RedirectToAction("Index", "Home");
            }

            var originalUser = await _userManager.FindByIdAsync(impersonatorId);
            await _signInManager.SignOutAsync();

            if (originalUser != null)
            {
                await _signInManager.SignInAsync(originalUser, isPersistent: false);
            }

            return RedirectToAction("Index", "Home");
        }

        private async Task PopulateImpersonationUsersAsync(ImpersonateViewModel model, string? currentUserId)
        {
            var query = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(currentUserId))
            {
                query = query.Where(u => u.Id != currentUserId);
            }

            var users = await query
                .OrderBy(u => u.Email)
                .ThenBy(u => u.UserName)
                .ToListAsync();

            model.Users = new List<UserSummaryViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Users.Add(new UserSummaryViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? user.UserName ?? string.Empty,
                    Roles = string.Join(", ", roles)
                });
            }
        }
    }
}
