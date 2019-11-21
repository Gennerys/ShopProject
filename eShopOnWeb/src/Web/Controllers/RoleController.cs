using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Controllers
{
	[ApiExplorerSettings(IgnoreApi = true)]
	[Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
	[Route("[controller]/[action]")]
	public class RoleController : Controller
	{
		private RoleManager<IdentityRole> _roleManager;
		private UserManager<ApplicationUser> _userManager;
		public RoleController(RoleManager<IdentityRole> roleManager,
			UserManager<ApplicationUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}
		public IActionResult Index() => View(_roleManager.Roles);

		public IActionResult Create() => View();
		[HttpPost]
		public async Task<IActionResult> Create([Required]string name)
		{
			if (ModelState.IsValid)
			{
				IdentityResult result
				= await _roleManager.CreateAsync(new IdentityRole(name));
				if (result.Succeeded)
				{
					TempData["message"] = $"role: {name} has been Added";

					return RedirectToAction("Index");
				}
				else
				{
					AddErrorsFromResult(result);
				}
			}
			return View(name);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			IdentityRole role = await _roleManager.FindByIdAsync(id);
			if (role != null)
			{
				IdentityResult result = await _roleManager.DeleteAsync(role);
				if (result.Succeeded)
				{
					TempData["message"] = $"role: {role.Name} has been Added";

					return RedirectToAction("Index");
				}
				else
				{
					AddErrorsFromResult(result);
				}
			}
			else
			{
				ModelState.AddModelError("", "No Role Found");
			}
			return View("Index", _roleManager.Roles);
		}

		public async Task<IActionResult> Edit(string id)
		{
			IdentityRole role = await _roleManager.FindByIdAsync(id);
			List<ApplicationUser> members = new List<ApplicationUser>();
			List<ApplicationUser> nonMembers = new List<ApplicationUser>();
			foreach (ApplicationUser user in _userManager.Users)
			{
				var list = await _userManager.IsInRoleAsync(user, role.Name)
				? members : nonMembers;
				list.Add(user);
			}
			return View(new RoleEditViewModel
			{
				Role = role,
				Members = members,
				NonMembers = nonMembers
			});
		}
		[HttpPost]
		public async Task<IActionResult> Edit(RoleModificationViewModel model)
		{
			IdentityResult result;
			if (ModelState.IsValid)
			{

				foreach (string userId in model.IdsToAdd ?? new string[] { })
				{
					ApplicationUser user = await _userManager.FindByIdAsync(userId);
					if (user != null)
					{
						result = await _userManager.AddToRoleAsync(user,
						model.RoleName);
						if (!result.Succeeded)
						{
							AddErrorsFromResult(result);
						}
						TempData["message"] = $"{user.UserName} has been added to role: {model.RoleName}";

					}
				}
				foreach (string userId in model.IdsToDelete ?? new string[] { })
				{
					ApplicationUser user = await _userManager.FindByIdAsync(userId);
					if (user != null)
					{
						result = await _userManager.RemoveFromRoleAsync(user,
						model.RoleName);
						if (!result.Succeeded)
						{
							AddErrorsFromResult(result);
						}
						TempData["message"] = $"{user.UserName} has been Added";
					}

				}

				return RedirectToAction(nameof(Index));
			}
			else
			{
				return await Edit(model.RoleId);
			}
		}
		private void AddErrorsFromResult(IdentityResult result)
		{
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
		}

	}
}
