using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.API
{
	public sealed class UserEndpoint : IUserEndpoint
	{
		#region FIELDS
		private readonly IAPIHelper _aPIHelper;

		#endregion

		public UserEndpoint(IAPIHelper aPIHelper)
		{
			_aPIHelper = aPIHelper;
		}

		public async Task<IEnumerable<UserModel>> GetAll()
		{
			using (HttpResponseMessage response = await _aPIHelper.ApiClient.GetAsync("/api/User/admin/users"))
			{
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsAsync<IEnumerable<UserModel>>();
					return result;
				}
				else
				{
					throw new InvalidOperationException(response.ReasonPhrase);
				}
			}
		}

		public async Task<Dictionary<string, string>> GetAllRoles()
		{
			using (HttpResponseMessage response = await _aPIHelper.ApiClient.GetAsync("/api/User/admin/roles"))
			{
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsAsync<Dictionary<string, string>>();
					return result;
				}
				else
				{
					throw new InvalidOperationException(response.ReasonPhrase);
				}
			}
		}


		public async Task AddUserToRole(string userId, string roleName)
		{

			var request = new { userId, roleName };

			using (HttpResponseMessage response = await _aPIHelper.ApiClient.PostAsJsonAsync("/api/User/admin/roles/AddRole", request))
			{
				if (!response.IsSuccessStatusCode)
				{
					throw new InvalidOperationException(response.ReasonPhrase);
				}
			}
		}
		
		public async Task RemoveUserFromRole(string userId, string roleName)
		{

			var request = new { userId, roleName };

			using (HttpResponseMessage response = await _aPIHelper.ApiClient.PostAsJsonAsync("/api/User/admin/roles/RemoveRole", request))
			{
				if (!response.IsSuccessStatusCode)
				{
					throw new InvalidOperationException(response.ReasonPhrase);
				}
			}
		}

	}
}
