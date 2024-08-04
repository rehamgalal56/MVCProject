using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MVCProject.PL.Services.EmailSender;
using MVCProject.PL.ViewModels.Account;
using MVCProject.PL.ViewModels.Account;
using MVCProject_DAL.Models;
using System.Threading.Tasks;

namespace MVCProject.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configuration;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(
            IEmailSender emailSender,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager) {

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

            if (ModelState.IsValid)
            {
                var user =await _userManager.FindByNameAsync(model.UserName);
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

                    var  result = await _userManager.CreateAsync(user,model.Password);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));

                    foreach(var error in result.Errors)
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
            if(ModelState.IsValid)
            {
                var user =await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,false);

                        if (result.IsLockedOut)
                            ModelState.AddModelError(string.Empty, "Your Account is Locked !!");

                        if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index),"Home");

                        if(result.IsNotAllowed)
                               ModelState.AddModelError(string.Empty, "Your Account is not Confirmed yet!!");
					}


                }

            }
            return View(model);
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

                    var resetPasswordUrl = Url.Action("ResetPassword", "Account", new {  email = user.Email, token = resetPassowrdToken } /*,"https", "localhost:5001"*/);

                    await _emailSender.SendEmailAsync(
						from: _configuration["EmailSettings:SenderEmail"],
						recipients: model.Email,
                        subject: "Reset Password",
                        bady: resetPasswordUrl);

					// send email
					return RedirectToAction(nameof(CheckYourInbox));
				}
                ModelState.AddModelError(string.Empty, "this email is not registered !!");
            }
			return View(nameof(ForgetPassword),model);
		}


        public IActionResult CheckYourInbox()
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
                    await _userManager.ResetPasswordAsync(user, token,model.NewPassword);
                    return RedirectToAction(nameof(SignIn));

                }
                ModelState.AddModelError(string.Empty, "Url is not valid!");

            }
            return View(model);
        }
		#endregion
	}
}
