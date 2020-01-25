using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.Users;
using ModernBusiness.Pages.Users.ViewModels;
using OrchardCore.Users.Services;
using OrchardCore.Users.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace ModernBusiness.Pages.Users.Pages
{
    public class RegisterModel : PageModel
    {
		[BindProperty]
		public RegisterViewModel RegisterVM { get; set; }

		ILogger _logger;

		private readonly IUserService _userService;
		private readonly SignInManager<IUser> _signInManager;

		public RegisterModel(IUserService userService, ILogger<RegisterModel> logger, SignInManager<IUser> signInManager)
		{
			_userService = userService;
			_logger = logger;
			_signInManager = signInManager;
		}

        public IActionResult OnGet(string returnUrl)
        {
			ViewData["ReturnUrl"] = returnUrl;

			return Page();
        }

		public async Task<IActionResult> OnPostAsync(string returnUrl)
		{
			ViewData["ReturnUrl"] = returnUrl;

			if (ModelState.IsValid)
			{
				returnUrl = returnUrl ?? Url.Content("~/");

				var user = await _userService.CreateUserAsync(new User
				{
					UserName = RegisterVM.Name,
					Email = RegisterVM.Email,
					RoleNames = new string[0]
				}, RegisterVM.Password, (key, message) => ModelState.AddModelError(key, message)) as User;

				if (user != null)
				{
					_logger.LogDebug("Hello{0}!!!", user.UserName);
					_logger.LogInformation(3, "User created!");

					await _signInManager.SignInAsync(user, false);

					return LocalRedirect(returnUrl);
				}
			}

			return Page();
		}
	}
}