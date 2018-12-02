using Invoisys.Infrastructure.CrossCutting.Identity.AccessDenied;
using Invoisys.Infrastructure.CrossCutting.Identity.Configuration;
using Invoisys.Infrastructure.CrossCutting.Identity.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Invoisys.Presentation.Web.Controllers
{
    [AccessDeniedAuthorize(Roles = "Admin")]
    public class RolesAdminController : Controller
    {
        private readonly ApplicationRoleManager _roleManager;
        private readonly ApplicationUserManager _userManager;
        public RolesAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }
        [Authorize]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var role = await _roleManager.FindByIdAsync(id);
            var users = new List<ApplicationUser>();
            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user.Id, role.Name)) users.Add(user);
            }
            ViewBag.Users = users;
            ViewBag.UserCount = users.Count;
            return View(role);
        }
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid) return View();
            var role = new IdentityRole(roleViewModel.Name);
            var roleresult = await _roleManager.CreateAsync(role);
            if (roleresult.Succeeded) return RedirectToAction("Index");
            ModelState.AddModelError("", roleresult.Errors.First());
            return View();
        }
        [Authorize]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return HttpNotFound();
            var roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };
            return View(roleModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Id")] RoleViewModel roleModel)
        {
            if (!ModelState.IsValid) return View();
            var role = await _roleManager.FindByIdAsync(roleModel.Id);
            role.Name = roleModel.Name;
            await _roleManager.UpdateAsync(role);
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return HttpNotFound();
            return View(role);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(string id, string deleteUser)
        {
            if (!ModelState.IsValid) return View();
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return HttpNotFound();
            IdentityResult result;
            if (deleteUser != null) result = await _roleManager.DeleteAsync(role);
            else result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded) return RedirectToAction("Index");
            ModelState.AddModelError("", result.Errors.First());
            return View();
        }
    }
}