using System.ComponentModel.DataAnnotations;

namespace WebApplication.PL.ViewModels.Account
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "New Password is Required")]
		[MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]
		[DataType(DataType.Password)]
		[Compare(nameof(NewPassword), ErrorMessage = "Confirm Password doesn't match with Password")]
		public string ConfirmPassword { get; set; }


    }
}
