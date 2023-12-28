using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

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
	}
}
