using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Security;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;
using TRMDataManager.Models;

namespace TRMDataManager.Controllers
{
	[Authorize]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {

		[HttpGet]
		public IHttpActionResult GetById()
		{
			var userData = new UserData();

			string id = RequestContext.Principal.Identity.GetUserId();
			UserModel result = userData.GetUserById(id);

			if (String.IsNullOrWhiteSpace(result.Id))
				return StatusCode(HttpStatusCode.NoContent);

			return Ok(result);

		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet]
		[Route("admin/users")]
		public IHttpActionResult GetAllUsers()
		{
			IEnumerable<ApplicationUser> users;
			IEnumerable<IdentityRole> roles;
			List<ApplicationUserModel> output = new List<ApplicationUserModel>();


			using (var context = new ApplicationDbContext())
			{
				var userStore = new UserStore<ApplicationUser>(context);
				var userManager = new UserManager<ApplicationUser>(userStore);

				users = userManager.Users.ToList();
				roles = context.Roles.ToList();

				foreach (var user in users)
				{
					ApplicationUserModel u = new ApplicationUserModel
					{
						Id = user.Id,
						Email = user.Email
					};

					foreach (IdentityUserRole r in user.Roles)
					{
						u.Roles.Add(r.RoleId, roles.Where(x => x.Id == r.RoleId).First().Name);
					}
					output.Add(u);

				}

			}

			return Ok(output);
		}


		[Authorize(Roles = "ADMIN")]
		[HttpGet]
		[Route("admin/roles")]
		public IHttpActionResult GetAllRoles()
		{
			using (var context = new ApplicationDbContext())
			{
				var roles = context.Roles.ToDictionary(x => x.Id, x => x.Name);

				return Ok(roles);
			}
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPost]
		[Route("admin/roles/AddRole")]
		public IHttpActionResult AddRole(UserRolePairModel request)
		{
			IdentityResult result;
			using (var context = new ApplicationDbContext())
			{
				var userStore = new UserStore<ApplicationUser>(context);
				var userManager = new UserManager<ApplicationUser>(userStore);


				result = userManager.AddToRole(request.UserId, request.RoleName);
			}

			return Ok(result);
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPost]
		[Route("admin/roles/RemoveRole")]
		public IHttpActionResult RemoveRole(UserRolePairModel request)
		{
			IdentityResult result;

			using (var context = new ApplicationDbContext())
			{
				var userStore = new UserStore<ApplicationUser>(context);
				var userManager = new UserManager<ApplicationUser>(userStore);


				result = userManager.RemoveFromRole(request.UserId, request.RoleName);
			}

			return Ok(result);
		}




	}
}
