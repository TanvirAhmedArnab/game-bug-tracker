using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GameBugTracker.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace GameBugTracker.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly DemoAdminOptions _demoAdminOptions;

        public LoginModel(IOptions<DemoAdminOptions> demoAdminOptionsAccessor)
        {
            _demoAdminOptions = demoAdminOptionsAccessor.Value;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        [BindProperty]
        public string ReturnUrl { get; set; } = "/";

        public bool IsAdminConfigured =>
            !string.IsNullOrWhiteSpace(_demoAdminOptions.UserName) &&
            !string.IsNullOrWhiteSpace(_demoAdminOptions.Password);

        public IActionResult OnGet(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return LocalRedirect(GetSafeReturnUrl(returnUrl));
            }

            ReturnUrl = GetSafeReturnUrl(returnUrl);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ReturnUrl = GetSafeReturnUrl(ReturnUrl);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!IsAdminConfigured)
            {
                ModelState.AddModelError(string.Empty, "Owner sign-in is unavailable until DemoAdmin:Password is configured.");
                return Page();
            }

            if (!string.Equals(Input.UserName, _demoAdminOptions.UserName, StringComparison.Ordinal) ||
                !PasswordsMatch(Input.Password, _demoAdminOptions.Password))
            {
                ModelState.AddModelError(string.Empty, "The username or password was incorrect.");
                return Page();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, _demoAdminOptions.UserName),
                new(ClaimTypes.Role, "Owner")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = false
                });

            return LocalRedirect(ReturnUrl);
        }

        private string GetSafeReturnUrl(string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }

            return Url.Page("/Index") ?? "/";
        }

        private static bool PasswordsMatch(string providedPassword, string configuredPassword)
        {
            var providedHash = SHA256.HashData(Encoding.UTF8.GetBytes(providedPassword));
            var configuredHash = SHA256.HashData(Encoding.UTF8.GetBytes(configuredPassword));

            return CryptographicOperations.FixedTimeEquals(providedHash, configuredHash);
        }

        public class LoginInputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; } = "";

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";
        }
    }
}
