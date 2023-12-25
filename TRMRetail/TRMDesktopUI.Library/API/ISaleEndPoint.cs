using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.API
{
	public interface ISaleEndPoint
	{
		Task PostSale(SaleModel sale);
	}
}