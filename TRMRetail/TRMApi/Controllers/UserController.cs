using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using TRMApi.Data;
using TRMApi.Models;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	[Route("api/User")]
	public class UserController : ControllerBase
	{

		private readonly ApplicationDbContext applicationdbcontext;
		private readonly UserManager<IdentityUser> userManager;

		#region CTOR
		public UserController (ApplicationDbContext applicationdbcontext, UserManager<IdentityUser> userManager)
		{
			this.applicationdbcontext = applicationdbcontext;
			this.userManager = userManager;
		}
        #endregion


        [HttpGet]
		public IActionResult GetById()
		{
			var userData = new UserData();

			string id = User.FindFirstValue(ClaimTypes.NameIdentifier); //RequestContext.Principal.Identity.GetUserId();
			UserModel result = userData.GetUserById(id);

			if (String.IsNullOrWhiteSpace(result.Id))
				return StatusCode((int)HttpStatusCode.NoContent);

			return Ok(result);

		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet]
		[Route("admin/users")]
		public IActionResult GetAllUsers()
		{
			List<ApplicationUserModel> output = new List<ApplicationUserModel>();
			IEnumerable<IdentityUser> users;

			users = applicationdbcontext.Users.ToList();
			var userRoles = from ur in applicationdbcontext.UserRoles
							join r in applicationdbcontext.Roles on ur.RoleId equals r.Id
							select new { ur.UserId, ur.RoleId, r.Name};

			foreach (var user in users)
			{
				ApplicationUserModel u = new ApplicationUserModel
				{
					Id = user.Id,
					Email = user.Email
				};

				u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId, val => val.Name);

				//foreach (IdentityUserRole r in user.Roles)
				//{
				//	u.Roles.Add(r.RoleId, roles.Where(x => x.Id == r.RoleId).First().Name);
				//}
				output.Add(u);

			}

			return Ok(output);
		}


		[Authorize(Roles = "ADMIN")]
		[HttpGet]
		[Route("admin/roles")]
		public IActionResult GetAllRoles()
		{
			var roles = applicationdbcontext.Roles.ToDictionary(x => x.Id, x => x.Name);

			return Ok(roles);
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPost]
		[Route("admin/roles/AddRole")]
		public async Task<IActionResult> AddRole(UserRolePairModel request)
		{
			IdentityResult result;
			IdentityUser user =  await userManager.FindByIdAsync(request.UserId);

			result = await userManager.AddToRoleAsync(user, request.RoleName);

			return Ok(result);
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPost]
		[Route("admin/roles/RemoveRole")]
		public async Task<IActionResult> RemoveRole(UserRolePairModel request)
		{
			IdentityResult result;

			IdentityUser user = await userManager.FindByIdAsync(request.UserId);
			result = await userManager.RemoveFromRoleAsync(user, request.RoleName);

			return Ok(result);
		}

	}
}
