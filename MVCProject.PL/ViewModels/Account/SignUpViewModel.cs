using System.ComponentModel.DataAnnotations;

namespace MVCProject.PL.ViewModels.Account
{
	public class SignUpViewModel
	{

		[Required(ErrorMessage = "UserName is Required !")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "First Name is Required !")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last Name is Required !")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Required(ErrorMessage ="Email is Required !")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }

		[Required(ErrorMessage ="Password is Required")]
		[DataType(DataType.Password)]
        public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password),ErrorMessage = "Confirm Password dosn't match with Password ")]
		public string ConfirmPassword { get; set; }
		public bool IsAgree { get; set; }
    }
}
