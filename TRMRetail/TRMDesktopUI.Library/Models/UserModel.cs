using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
	public sealed class UserModel
	{
		public string Id { get; set; } = String.Empty;
		public string Email { get; set; } = String.Empty;

		public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();

        public string RoleList { 
			get
			{
				return String.Join(", ", Roles.Select(x=> x.Value));
			}
		}


    }
}
