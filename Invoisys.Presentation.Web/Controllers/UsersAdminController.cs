using Invoisys.Infrastructure.CrossCutting.Identity.Configuration;
using Invoisys.Infrastructure.CrossCutting.Identity.Model;
using Invoisys.Presentation.Web.Filters;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Invoisys.Presentation.Web.Controllers
{
    [AccessDeniedAuthorize(Roles = "Admin")]
    public class UsersAdminController : Controller
    {
        private readonly ApplicationRoleManager _roleManager;
        private readonly ApplicationUserManager _userManager;
        public UsersAdminController(ApplicationRoleManager roleManager, ApplicationUserManager userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }
        [Authorize]
        public ActionResult Create()
        {
            var user = new RegisterViewModel
            {
                RolesList = _roleManager.Roles.ToList().Select(x => new DropDownListItem()
                {
                    Text = x.Name,
                    Value = x.Name
                })
            };
            return View(user);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model, params string[] selectedRole)
        {
            var role = selectedRole;
            model.RolesList = _roleManager.Roles.ToList().Select(x => new DropDownListItem() { Selected = role.Contains(x.Name), Text = x.Name, Value = x.Name });
            if (!ModelState.IsValid) return View(model);
            model.Password = _userManager.PasswordDefault();
            var user = new ApplicationUser { ChangePassword = true, EmailConfirmed = true, Name = model.Name.Trim(), UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: Request.Url?.Scheme);
                await _userManager.SendEmailAsync(user.Id, "Usuário Cadastrado", callbackUrl);
                selectedRole = selectedRole ?? new string[] { };
                await _userManager.AddToRolesAsync(user.Id, selectedRole.ToArray());
                return RedirectToAction("Index");
            }
            AddErrors(result);
            return View(model);
        }
        [Authorize]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = await _userManager.FindByIdAsync(id);
            ViewBag.RoleNames = await _userManager.GetRolesAsync(user.Id);
            return View(user);
        }
        [Authorize]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return HttpNotFound();
            var userRoles = await _userManager.GetRolesAsync(user.Id);
            return View(new UsersRoleViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RolesList = _roleManager.Roles.ToList().Select(x => new DropDownListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PhoneNumber,Email,Name,Id")] UsersRoleViewModel editUser, params string[] selectedRole)
        {
            var userRoles = await _userManager.GetRolesAsync(editUser.Id);
            editUser.RolesList = _roleManager.Roles.ToList().Select(x => new DropDownListItem()
            {
                Selected = userRoles.Contains(x.Name),
                Text = x.Name,
                Value = x.Name
            });
            if (!ModelState.IsValid) return View(editUser);
            var user = await _userManager.FindByIdAsync(editUser.Id);
            if (user == null)
                return HttpNotFound();
            user.Name = editUser.Name.Trim();
            user.UserName = editUser.Email;
            user.Email = editUser.Email;
            user.PhoneNumber = editUser.PhoneNumber;
            selectedRole = selectedRole ?? new string[] { };
            var result = await _userManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray());
            if (!result.Succeeded)
            {
                editUser.RolesList = _roleManager.Roles.ToList().Select(x => new DropDownListItem() { Selected = selectedRole.Contains(x.Name), Text = x.Name, Value = x.Name });
                AddErrors(result);
                return View(editUser);
            }
            result = await _userManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray());
            if (!result.Succeeded) return View(editUser);
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return HttpNotFound();
            return View(user);
        }
        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (!ModelState.IsValid) return View();
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return HttpNotFound();
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return RedirectToAction("Index");
            ModelState.AddModelError("", result.Errors.First());
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> ResetToken(string id)
        {
            var user = _userManager.FindById(id);
            if (user == null || !user.ChangePassword) return Json("Não é possível resetar o Token, o usuário já confirmou sua conta e alterou a senha");
            var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: Request.Url?.Scheme);
            await _userManager.SendEmailAsync(user.Id, "Usuário Cadastrado", callbackUrl);
            return Json("Token resetado com sucesso, um novo e-mail foi enviado para o usuário");
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                if (error.EndsWith("is already taken."))
                {
                    if (error.StartsWith("Name")) ModelState.Clear();
                    else ModelState.AddModelError("", @"Este e-mail já está cadastrado, por favor informe outro");
                }
                else ModelState.AddModelError("", error);
            }
        }
    }
}