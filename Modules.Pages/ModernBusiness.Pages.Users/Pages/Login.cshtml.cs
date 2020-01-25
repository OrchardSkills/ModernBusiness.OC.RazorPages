using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OrchardCore.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModernBusiness.Pages.Users.ViewModels;
using Microsoft.Extensions.Logging;
using OrchardCore.Settings;
using ModernBusiness.Pages.Users.Model;
using OrchardCore.Entities;

namespace ModernBusiness.Pages.Users.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginViewModel LoginVM { get; set; }

		public bool allowResetPassword { get; set; }

        private readonly SignInManager<IUser> _signInManager;
        private readonly UserManager<IUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
		private readonly ISiteService _site;

		public LoginModel(UserManager<IUser> userManager, SignInManager<IUser> signInManager, ISiteService site, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
			_site = site;
		}

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			if (User.Identity.IsAuthenticated && returnUrl != null)
			{
				return LocalRedirect("~/notauthorized");
			}
			else if (User.Identity.IsAuthenticated)
			{
				return LocalRedirect("~/warning");
			}

			allowResetPassword = (await _site.GetSiteSettingsAsync()).As<ResetPasswordSettings>().AllowResetPassword;

            ViewData["ReturnUrl"] = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            if (ModelState.IsValid)
            {
                returnUrl = returnUrl ?? Url.Content("~/");
				
                var result = await _signInManager.PasswordSignInAsync(LoginVM.Name, LoginVM.Password, LoginVM.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {0} logged in.", LoginVM.Name);
                    return LocalRedirect(returnUrl);
                }
            }

            return Page();
        }
    }
}