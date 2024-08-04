﻿using System.ComponentModel.DataAnnotations;

namespace MVCProject.PL.ViewModels.Account
{
	public class SignInViewModel
	{
		[Required(ErrorMessage = "Email is Required !")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
		  
		[Required(ErrorMessage = "Password is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
