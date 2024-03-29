﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TRMApi.Data;

namespace TRMApi.Controllers
{
	public sealed class TokenController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly UserManager<IdentityUser> userManager;
		private readonly IConfiguration configuration;

		#region CTOR
		public TokenController(
			ApplicationDbContext context,
			UserManager<IdentityUser> userManager,
			IConfiguration configuration)
        {
			this.context = context;
			this.userManager = userManager;
			this.configuration = configuration;
		}
		#endregion

		[Route("/token")]
		[HttpPost]
		public async Task<IActionResult> Create (string username, string password, string grant_type)
		{
			if(await IsValidUserName(username, password))
			{
				return new ObjectResult(await GenerateToken(username));
			}
			else
			{
				return BadRequest();
			}
		}

		private async Task<bool> IsValidUserName(string username, string password)
		{
			var user = await userManager.FindByEmailAsync(username);
			return await userManager.CheckPasswordAsync(user, password);
		}

		private async Task<dynamic> GenerateToken(string username)
		{
			var user = await userManager.FindByEmailAsync(username);
			var roles = from ur in context.UserRoles
						join r in context.Roles on ur.RoleId equals r.Id
						where ur.UserId == user.Id
						select new { ur.UserId, ur.RoleId, r.Name};

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, username),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()), // not before right now
				new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
			};

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role.Name));
			}

			var credential = new SigningCredentials(
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Secrets:SecurityKey"))),
						SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				new JwtHeader(credential),
				new JwtPayload(claims));

			var output = new
			{
				Acces_Token = new JwtSecurityTokenHandler().WriteToken(token),
				UserName = username
			};

			return output;

		}



    }
}
