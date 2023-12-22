

using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.API
{
	public interface IAPIHelper
	{
		Task<AuthenticatedUser> Authenticate(string userName, string password);

		Task<LoggedInUserModel> GetLoggedInUserInfo(string token);

		HttpClient ApiClient { get; }
	}
}