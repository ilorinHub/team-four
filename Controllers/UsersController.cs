using ElectionWeb.Data;
using ElectionWeb.Models;
using ElectionWeb.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElectionWeb.Controllers
{
    public class UsersController : BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUsers> _userManager;


        public UsersController(ApplicationDbContext context, UserManager<ApplicationUsers> userManager)
        {
            _context= context;
            _userManager= userManager; 
        }
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            var appUsers = users.Select(x => new UserView
			{
				UserName = x.UserName,
				Email = x.Email,
				Id = x.Id
			}).ToList();
			var userRoles = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();
			foreach (var user in appUsers)
            {
                var role = userRoles.Where(x => x.UserId == user.Id).FirstOrDefault();
                if(role == null)
                {
                    user.Role = "Unassigned";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(r => r.Id == role.RoleId).Name;
                }
            }
           
               
            return View(appUsers);
        }
   
    
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            if(user == null) {
                DisplayError("User Not Found");
                return View("Index");
            }
            var viewModel = new UserEditViewModel
            {
                Id = id,
                Email = user.Email,
                Name = user.UserName,
                NIN = user.NIN,
            };
            var userRoles = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();
            var role = userRoles.Where(x => x.UserId == user.Id).FirstOrDefault();
            if (role == null)
            {
                viewModel.Role = "Unassigned";
            }
            else
            {
                viewModel.Role = roles.FirstOrDefault(r => r.Id == role.RoleId).Id;
            }
            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Name", viewModel.Role);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.ApplicationUsers.Find(model.Id);
                var userRoles = _context.UserRoles.ToList();
                var roles = _context.Roles.ToList();
                var userRole = userRoles.FirstOrDefault(u => u.UserId == model.Id);
                if (userRole != null)
                {
                    var previousRole = roles.Where(u => u.Id == userRole.RoleId).Select(e => e.Name).FirstOrDefault();
                    // Remove Old Role
                    await _userManager.RemoveFromRoleAsync(user, previousRole);
                }
                // Add new Role 
                await _userManager.AddToRoleAsync(user, roles.FirstOrDefault(u => u.Id == model.Role).Name);
                _context.SaveChanges();
                return RedirectToAction("Index"); 
            }
            return View(model);
        }
    }

 
}
