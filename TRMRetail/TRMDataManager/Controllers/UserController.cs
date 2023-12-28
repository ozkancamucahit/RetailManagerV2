using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;
using TRMDataManager.Models;

namespace TRMDataManager.Controllers
{
	[Authorize]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {

		// GET: api/User/id
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

		[Authorize(Roles ="ADMIN")]
		[HttpGet]
		[Route("admin/users")]
		public async Task<IHttpActionResult> GetAllusers()
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
						u.Roles.Add(r.RoleId, roles.Where( x => x.Id == r.RoleId).First().Name );
					}
					output.Add(u);

				}

			}

			return Ok(output);
		}

	}
}
