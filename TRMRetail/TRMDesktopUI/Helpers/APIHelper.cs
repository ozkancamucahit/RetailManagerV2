using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.Helpers
{
	public sealed class APIHelper : IAPIHelper
	{

		#region Fields
		private HttpClient apiClient;
		private ILoggedInUserModel _LoggedInUserModel;

		//public HttpClient HttpClient { 
		//    get
		//    {
		//        return apiClient;
		//    }
		//}

		public HttpClient ApiClient => apiClient;

		#endregion

		public APIHelper(ILoggedInUserModel loggedInUserModel)
		{
			InitializeClient();
			_LoggedInUserModel = loggedInUserModel;
		}

		private void InitializeClient()
		{
			string api = ConfigurationManager.AppSettings["api"];
			apiClient = new HttpClient();
			apiClient.BaseAddress = new Uri(api);
			apiClient.DefaultRequestHeaders.Accept.Clear();
			apiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<AuthenticatedUser> Authenticate(string userName, string password)
		{
			var data = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("grant_type", "password"),
				new KeyValuePair<string, string>("username", userName),
				new KeyValuePair<string, string>("password", password),
			});

			using (HttpResponseMessage response = await apiClient.PostAsync("/Token", data))
			{
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
					return result;
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}

		public async Task/*<LoggedInUserModel>*/ GetLoggedInUserInfo(string token)
		{
			IEnumerable<int> userIds = Enumerable.Range(1, 20);
			apiClient.DefaultRequestHeaders.Clear();
			apiClient.DefaultRequestHeaders.Accept.Clear();
			apiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
			apiClient.DefaultRequestHeaders.Add("Authorization", String.Concat("Bearer ", token));
			apiClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("RMClient", "1.1"));

			using (HttpResponseMessage response = await apiClient.GetAsync("/api/User"))
			{
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
					_LoggedInUserModel.CreatedDate = result.CreatedDate;
					_LoggedInUserModel.EmailAddress = result.EmailAddress;
					_LoggedInUserModel.FirstName = result.FirstName;
					_LoggedInUserModel.LastName = result.LastName;
					_LoggedInUserModel.Id = result.Id;
					_LoggedInUserModel.Token = token;
					//return result;
				}
				else
					throw new InvalidOperationException(response.ReasonPhrase);
			}
		}



	}
}
