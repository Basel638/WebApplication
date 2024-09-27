using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WebApplication.DAL.Models;
using WebApplication.PL.Services.EmailSender;
using WebApplication.PL.ViewModels.Account;

namespace WebApplication.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configuration;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(IEmailSender emailSender,IConfiguration configuration,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
			_emailSender = emailSender;
			_configuration = configuration;
			_userManager = userManager;
			_signInManager = signInManager;
		}

        #region Sign Up

        [HttpGet]
		public IActionResult SignUp()
		{
			return View();
		}





		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{



			if(ModelState.IsValid)
			{
				var user =await _userManager.FindByNameAsync(model.UserName);

				if (user == null)
				{
					user = new ApplicationUser()
					{
						FName = model.FirstName,
						LName = model.LastName,
						UserName = model.UserName, 
						Email = model.Email,
						IsAgree = model.IsAgree,
					};

					var result = await _userManager.CreateAsync(user,model.Password);

					if (result.Succeeded) 
						return RedirectToAction(nameof(SignIn));


					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);
				
				}
				ModelState.AddModelError(string.Empty, "this username is already in use for another account");
			}

	
			return View(model);

		}

		#endregion


		#region SignIn

		public IActionResult SignIn()
		{
			return View();	
		}



		[HttpPost]

		public async Task<IActionResult> SignIn(SignInViewModel model)
		{


			if(ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);	

				if(user != null)
				{
					var flag = await _userManager.CheckPasswordAsync(user, model.Password);
					
					if(flag)
					{
						var result = await _signInManager.PasswordSignInAsync(user,model.Password,model.RemeberMe,false);

						if (result.IsLockedOut)
							ModelState.AddModelError(string.Empty, "Your Account is Locked!!");

						if (result.Succeeded)
							return RedirectToAction(nameof(HomeController.Index), "Home");
						
						if (result.IsNotAllowed)
							ModelState.AddModelError(string.Empty, "Your Account is not confirmed yet!!");

					}

				}

				ModelState.AddModelError(string.Empty, "Invalid Login");
			}
			return View(model);
		}

		#endregion



		#region Sign Out


		public async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}

		#endregion



		#region Forget Password
		public IActionResult ForgetPassword()
		{
			return View();
		}


		[HttpPost]

		public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);

				if (user is not null)
				{

					var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user); // UNIQUE Token for this user

					var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token = resetPasswordToken });
					await _emailSender.SendAsync(from: _configuration["EmailSettings:SenderEmail"],
												recipients: model.Email,
												subject: "Reset Your Password",
												body: resetPasswordUrl);

					return RedirectToAction(nameof(CheckYourInbox));
				}

				ModelState.AddModelError(string.Empty, "There is No Account with this Email!!");
			}
			return View(model);
		}

		public IActionResult CheckYourInbox()
		{
			return View();
		}

		public IActionResult ResetPassword(string email, string token)
		{
			TempData["Email"] = email;
			TempData["token"] = token;
			return View();
		}

		[HttpPost]
		public	async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)

		{
            if ((ModelState.IsValid))
            {
				var email = TempData["Email"] as string;
				var token = TempData["token"] as string;

				var user= await _userManager.FindByEmailAsync(email);

				if(user is not null)
				{
					await _userManager.ResetPasswordAsync(user, token,model.NewPassword);
					return RedirectToAction(nameof(SignIn));

				}
				ModelState.AddModelError(string.Empty,"Url is not valid!");
			}
			return View(model);
        }


		#endregion
	}
}
