using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
	public class AuthenticatedUser
	{
		[JsonProperty("acces_Token")]
		public string access_token { get; set; }

		[JsonProperty("userName")]
		public string UserName { get; set; }
	}
}
