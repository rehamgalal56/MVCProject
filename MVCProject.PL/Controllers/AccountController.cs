using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MVCProject.PL.Services.EmailSender;
using MVCProject.PL.Services.SmsSenderByTwilio;
using MVCProject.PL.ViewModels.Account;
using MVCProject.PL.ViewModels.Account;
using MVCProject_DAL.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly IEmailSender _emailSender;
		private readonly ISmsSender _smsSender;
		private readonly IConfiguration _configuration;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(
			IEmailSender emailSender,
			ISmsSender smsSender,
			IConfiguration configuration,
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{

			_emailSender = emailSender;
			_smsSender = smsSender;
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

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(model.UserName);
				if (user is null)
				{
					user = new ApplicationUser()
					{
						FName = model.FirstName,
						LName = model.LastName,
						UserName = model.UserName,
						Email = model.Email,
						IsAgree = model.IsAgree,

					};

					var result = await _userManager.CreateAsync(user, model.Password);

					if (result.Succeeded)
						return RedirectToAction(nameof(SignIn));

					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);

				}
				ModelState.AddModelError(string.Empty, "this username is already in use for aonther account !");


			}
			return View(model);
		}
		#endregion

		#region Sign In
		public IActionResult SignIn()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var flag = await _userManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

						if (result.IsLockedOut)
							ModelState.AddModelError(string.Empty, "Your Account is Locked !!");

						if (result.Succeeded)
							return RedirectToAction(nameof(HomeController.Index), "Home");

						if (result.IsNotAllowed)
							ModelState.AddModelError(string.Empty, "Your Account is not Confirmed yet!!");
					}


				}

			}
			return View(model);
		}

		// Google Login
		public IActionResult GoogleLogin()
		{
			var prop = new AuthenticationProperties
			{
				RedirectUri = Url.Action("GoogleResponse")
			};
			return Challenge(prop, GoogleDefaults.AuthenticationScheme);
		}
		//Google Response
		public async Task<IActionResult> GoogleResponse()
		{
			var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
			var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(
				claim => new
				{
					claim.Issuer,
					claim.OriginalIssuer,
					claim.Type,
					claim.Value
				});

			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		#endregion

		#region Sign Out
		public async new Task<IActionResult> SignOut()
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
					var resetPassowrdToken = await _userManager.GeneratePasswordResetTokenAsync(user);    // UNIQUE token for this user

					var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token = resetPassowrdToken }, "https", "localhost:44303");

					#region Send Email
					#region By .Net Core
					//  await _emailSender.SendEmailAsync(
					// 	_configuration["EmailSettings:SenderEmail"],
					//	     ents: model.Email,
					//       subject: "Reset Password",
					//       bady: resetPasswordUrl);
					#endregion

					#region By Mimekit
					_emailSender.SendMail(
					   from: _configuration["EmailSettings:SenderEmail"],
					   recipients: model.Email,
					   subject: "Reset Password",
					   bady: resetPasswordUrl);
					#endregion

					#endregion

					return RedirectToAction(nameof(CheckYourInbox));
				}
				ModelState.AddModelError(string.Empty, "this email is not registered !!");
			}
			return View(nameof(ForgetPassword), model);
		}
		public IActionResult CheckYourInbox()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendResetPasswordSms(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var resetPassowrdToken = await _userManager.GeneratePasswordResetTokenAsync(user);    // UNIQUE token for this user

					var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token = resetPassowrdToken }, "https", "localhost:44303");


					var result = _smsSender.SendSms(user.PhoneNumber, resetPasswordUrl);
					if (string.IsNullOrEmpty(result.ErrorMessage))
					{

						return RedirectToAction(nameof(CheckYourMessages));
					}
					ModelState.AddModelError(string.Empty, "Error in sending sms !!");

					//return Ok(result);
				}
				ModelState.AddModelError(string.Empty, "this email is not registered !!");
			}
			return View(nameof(ForgetPassword), model);
		}

		public IActionResult CheckYourMessages()
		{
			return View();
		}
		#endregion

		#region Reset Password
		[HttpGet]
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["Email"] = email;
			TempData["Token"] = token;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var email = TempData["Email"] as string;
				var token = TempData["Token"] as string;

				var user = await _userManager.FindByEmailAsync(email);

				if (user is not null)
				{
					await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
					return RedirectToAction(nameof(SignIn));

				}
				ModelState.AddModelError(string.Empty, "Url is not valid!");

			}
			return View(model);
		}
		#endregion
	}
}
