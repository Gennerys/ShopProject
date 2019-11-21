using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.Web.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Controllers
{
	[ApiExplorerSettings(IgnoreApi = true)]
	[Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
	[Route("[controller]/[action]")]
	public class AdminController : Controller
	{
		private UserManager<ApplicationUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;

		public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}
		//[AllowAnonymous]
		//public IActionResult Welcome() => View();
		public ViewResult Index() => View(_userManager.Users);

		public ViewResult Create() => View();
		[HttpPost]
		public async Task<IActionResult> Create(CreateUserViewModel model)
		{
			if (ModelState.IsValid)
			{
				ApplicationUser user = new ApplicationUser
				{
					Email = model.Email
				};
				IdentityResult result
				= await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					TempData["message"] = $"{user.UserName} has been Added";

					return RedirectToAction("Index", _userManager.Users);
				}
				else
				{
					foreach (IdentityError error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			ApplicationUser user = await _userManager.FindByIdAsync(id);
			if (user != null)
			{
				IdentityResult result = await _userManager.DeleteAsync(user);
				if (result.Succeeded)
				{
					TempData["message"] = $"{user.UserName} has been deleted";

					return RedirectToAction("Index");
				}
				else
				{
					AddErrorsFromResult(result);
				}
			}
			else
			{
				ModelState.AddModelError("", "User Not Found");
			}
			return View("Index", _userManager.Users);
		}


		public async Task<IActionResult> Edit(string id)
		{
			ApplicationUser user = await _userManager.FindByIdAsync(id);
			if (user != null)
			{
				return View(user);
			}
			else
			{
				return RedirectToAction("Index", _userManager.Users);
			}
		}
		[HttpPost]
		public async Task<IActionResult> Edit(string id, string email,
		string password, string username)
		{
			ApplicationUser user = await _userManager.FindByIdAsync(id);
			if (user != null)
			{
				user.Email = email;
				user.PasswordHash = password;

				IdentityResult result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					TempData["message"] = $"{user.UserName} has been updated";

					return RedirectToAction("Index", _userManager.Users);
				}
				else
				{
					AddErrorsFromResult(result);
				}
			}

			else
			{
				ModelState.AddModelError("", "User Not Found");
			}
			return View(user);
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
