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

		// GET: api/User/5
		public string Get(int id)
		{
			return "value";
		}

		// POST: api/User
		public void Post([FromBody] string value)
		{
		}

		// PUT: api/User/5
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE: api/User/5
		public void Delete(int id)
		{
		}
	}
}
