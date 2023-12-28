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

		//public async Task<IEnumerable<ProductModel>> GetProducts()
		//{

		//	using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Product"))
		//	{
		//		if (response.IsSuccessStatusCode)
		//		{
		//			var result = await response.Content.ReadAsAsync<IEnumerable<ProductModel>>();

		//			return result;
		//		}
		//		else
		//			throw new InvalidOperationException(response.ReasonPhrase);
		//	}
		//	//return Enumerable.Empty<string>();
		//}


	}
}
