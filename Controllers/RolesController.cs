using ElectionWeb.Data;
using ElectionWeb.Models;
using ElectionWeb.Models.ViewModels;
using ElectionWeb.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ElectionWeb.Controllers
{
	//[Authorize(Policy = Policies.ManageRoles)]
	public class RolesController : BaseController
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUsers> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public RolesController(ApplicationDbContext context, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index()
		{
			var roles = await _context.Roles.ToListAsync();
			if (User.IsInRole(Roles.INEC))
			{
				roles = roles.Where(x => x.Name != Roles.SuperAdmin).ToList();
			}
			return View(roles);
		}

		public async Task<IActionResult> Details(String id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var role = await _context.Roles
				.FirstOrDefaultAsync(m => m.Id == id);
			if (role == null)
			{
				return NotFound();
			}

			return View(role);
		}


		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var role = await _context.Roles.FindAsync(id);
			if (role == null)
			{
				return NotFound();
			}
			return View(role);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("Id,RoleName")] IdentityRole role)
		{
			if (id != role.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(role);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RoleExists(role.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(role);
		}

		// GET: Roles/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var role = await _context.Roles
				.FirstOrDefaultAsync(m => m.Id == id);
			if (role == null)
			{
				return NotFound();
			}

			return View(role);
		}

		// POST: Roles/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var role = await _context.Roles.FindAsync(id);
			_context.Roles.Remove(role);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool RoleExists(string id)
		{
			return _context.Roles.Any(e => e.Id == id);
		}

		public async Task<ActionResult> Claims(string id)
		{

			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				DisplayError("Invalid Role Passed");
				return RedirectToAction("Index");
			}
			var model = new RoleClaimsViewModel()
			{
				RoleId = id,
				Rolename = role.Name,
			};
			var roleClaims = await _roleManager.GetClaimsAsync(role);
			foreach (Claim claim in ClaimsEngine.claimsList)
			{
				var roleExist = roleClaims.Any(rc => rc.Type == claim.Type);

				RoleClaim userClaim = new RoleClaim
				{
					ClaimName = claim.Value.ToString(),
					IsSelected = roleExist ? true : false
				};
				model.Claims.Add(userClaim);
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Claims(RoleClaimsViewModel model)
		{
			if (ModelState.IsValid)
			{
				var role = await _roleManager.FindByIdAsync(model.RoleId);
				if (role == null)
				{
					DisplayError("Invalid Role Selected");
					return View(model);
				}
				var roleClaims = await _roleManager.GetClaimsAsync(role);
				foreach (var claim in roleClaims)
				{
					await _roleManager.RemoveClaimAsync(role, claim);
				}
				foreach (var claim in model.Claims)
				{
					if (claim.IsSelected)
					{
						var claimFromList = ClaimsEngine.claimsList.Where(c => c.Value == claim.ClaimName).FirstOrDefault();
						if (claimFromList != null)
						{
							await _roleManager.AddClaimAsync(role, new Claim(claimFromList.Type, claim.IsSelected.ToString()));
						}
						else
						{
							DisplayError("Claims not available");
						}
					}
				}
				DisplayMessage($"Claims for {role.Name} Updated Successfully");
				return RedirectToAction(nameof(Index));
			}
			return View(model);
		}

	}
}
