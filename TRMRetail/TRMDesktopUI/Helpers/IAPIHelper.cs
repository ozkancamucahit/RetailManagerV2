using TRMDesktopUI.Models;

namespace TRMDesktopUI.Helpers
{
	public interface IAPIHelper
	{
		System.Threading.Tasks.Task<AuthenticatedUser> Authenticate(string userName, string password);
	}
}