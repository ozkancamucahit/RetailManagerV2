using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.API
{
	public sealed class ProductEndPoint : IProductEndPoint
	{
		#region Fields
		private IAPIHelper _apiHelper;
		#endregion

		public ProductEndPoint(IAPIHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task<IEnumerable<ProductModel>> GetProducts()
		{

			using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Product"))
			{
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsAsync<IEnumerable<ProductModel>>();

					return result;
				}
				else
					throw new InvalidOperationException(response.ReasonPhrase);
			}
			//return Enumerable.Empty<string>();
		}


	}
}
