using System;

namespace TRMDesktopUI.Library.Models
{
	public interface ILoggedInUserModel
	{
		string Token { get; set; }
		DateTime CreatedDate { get; set; }
		string EmailAddress { get; set; }
		string FirstName { get; set; }
		string Id { get; set; }
		string LastName { get; set; }
	}
}