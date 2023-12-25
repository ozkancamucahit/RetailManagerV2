using System;
using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.API
{
	public sealed class SaleEndPoint : ISaleEndPoint
	{
		#region Fields
		private IAPIHelper _apiHelper;
		#endregion

		public SaleEndPoint(IAPIHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task PostSale(SaleModel sale)
		{
			using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
			{

				if (response.IsSuccessStatusCode)
				{
					//TODO: Log Call
				}
				else
				{
					throw new InvalidOperationException(response.ReasonPhrase);
				}
			}
		}
	}
}
