﻿using Microsoft.AspNetCore.Authorization;
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
	public class UserController : ControllerBase
	{

		private readonly ApplicationDbContext applicationdbcontext;
		private readonly UserManager<IdentityUser> userManager;
		private readonly ILogger<UserController> logger;
		private readonly IUserData userData;

		#region CTOR
		public UserController (ApplicationDbContext applicationdbcontext, 
			UserManager<IdentityUser> userManager,
			ILogger<UserController> logger,
			IUserData userData)
		{
			this.applicationdbcontext = applicationdbcontext;
			this.userManager = userManager;
			this.logger = logger;
			this.userData = userData;
		}
        #endregion


        [HttpGet]
		[Route("GetById")]
		public IActionResult GetById()
		{

			string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
			UserModel result = userData.GetUserById(id);

			if (String.IsNullOrWhiteSpace(result.Id))
				return StatusCode((int)HttpStatusCode.NoContent);

			return Ok(result);

		}

		[Authorize(Roles = "Admin")]
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

				output.Add(u);

			}

			return Ok(output);
		}


		[Authorize(Roles = "Admin")]
		[HttpGet]
		[Route("admin/roles")]
		public IActionResult GetAllRoles()
		{
			var roles = applicationdbcontext.Roles.ToDictionary(x => x.Id, x => x.Name);

			return Ok(roles);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		[Route("admin/roles/AddRole")]
		public async Task<IActionResult> AddRole(UserRolePairModel request)
		{
			IdentityResult result;

			string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			UserModel loggedInUser = userData.GetUserById(loggedInUserId);

			IdentityUser user =  await userManager.FindByIdAsync(request.UserId);

			logger.LogInformation("Admin {Admin} added user {User} to Role {Role}"
				, loggedInUserId
				, user.Id, request.RoleName);

			result = await userManager.AddToRoleAsync(user, request.RoleName);

			return Ok(result);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		[Route("admin/roles/RemoveRole")]
		public async Task<IActionResult> RemoveRole(UserRolePairModel request)
		{
			IdentityResult result;

			IdentityUser user = await userManager.FindByIdAsync(request.UserId);
			string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


			logger.LogInformation("Admin {Admin} removed user {User} from Role {Role}"
				, loggedInUserId
				, user.Id, request.RoleName);

			result = await userManager.RemoveFromRoleAsync(user, request.RoleName);

			return Ok(result);
		}

	}
}
