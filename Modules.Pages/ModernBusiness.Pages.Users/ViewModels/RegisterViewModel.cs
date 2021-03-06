﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModernBusiness.Pages.Users.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
	}
}
