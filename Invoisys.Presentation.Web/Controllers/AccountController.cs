using Invoisys.Infrastructure.CrossCutting.Identity.Configuration;
using Invoisys.Infrastructure.CrossCutting.Identity.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Invoisys.Presentation.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _userManager;
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    if (!user.ChangePassword) return RedirectToLocal(returnUrl);
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                    return RedirectToAction("SilentLogOff", new { userId = user.Id, code });
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("LOGIN", @"Conta bloqueada devido ao excesso de tentativas");
                    return View(model);
                case SignInStatus.RequiresVerification:
                    ModelState.AddModelError("LOGIN", @"Login requer verificação");
                    return View(model);
                case SignInStatus.Failure:
                    ModelState.AddModelError("LOGIN", @"Login e/ou Senha incorreto");
                    return View(model);
                default:
                    ModelState.AddModelError("LOGIN", @"Login e/ou Senha incorreto");
                    return View(model);
            }
        }
        [Authorize]
        public ActionResult ChangePassword()
        {
            var viewModel = new ChangePasswordViewModel();
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = _userManager.FindByName(User.Identity.Name);
            if (user == null) return View(model);
            if (model.OldPassword == model.NewPassword)
            {
                ModelState.AddModelError("", @"Coloque uma senha diferente da sua senha atual");
                return View(model);
            }
            var validPassword = _userManager.CheckPassword(user, model.OldPassword);
            if (!validPassword)
            {
                ModelState.AddModelError("", @"A senha atual está incorreta");
                return View(model);
            }
            _userManager.ChangePassword(user.Id, model.OldPassword, model.NewPassword);
            return View("ChangePasswordConfirmation");
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", @"E-mail não encontrado");
                return View();
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: Request.Url?.Scheme);
            await _userManager.SendEmailAsync(user.Id, "Esqueci minha senha", callbackUrl);
            return View("ForgotPasswordConfirmation");
        }
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code)
        {
            if (userId == null)
                RedirectToAction("Page500", "Error");
            var user = _userManager.FindById(userId);
            if (user == null)
                RedirectToAction("Page500", "Error");
            var viewModel = new ResetPasswordViewModel() { Email = user?.Email };
            return code == null ? View() : View(viewModel);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return View(model);
            if (user.ChangePassword) user.ChangePassword = false;
            var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                if (_userManager.Find(model.Email, model.Password) != null)
                    user.LockoutEndDateUtc = null;
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, shouldLockout: true);
                return RedirectToAction("Index", "Home", new { model.Email });
            }
            AddErrors(result);
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult SilentLogOff(string userId, string code)
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("ResetPassword", "Account", new { userId, code });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                if (returnUrl.Equals($"{Url.Content("~/")}Error/AccessDenied"))
                    return RedirectToAction("Index", "Home");
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                if (error.EndsWith("Invalid token.")) ModelState.AddModelError("", @"Token inválido");
            }
        }
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
    }
}