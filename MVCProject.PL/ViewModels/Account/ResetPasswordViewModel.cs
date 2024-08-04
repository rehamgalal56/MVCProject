using System.ComponentModel.DataAnnotations;

namespace MVCProject.PL.ViewModels.Account
{
	public class ResetPasswordViewModel
	{
        [Required(ErrorMessage = "New Password is Required")]
        [DataType(DataType.Password)]
        public string  NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password dosn't match with New Password ")]
        public string ConfirmPassword { get; set; }
    }
}
