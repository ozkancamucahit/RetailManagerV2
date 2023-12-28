using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRMDataManager.Models
{
	public sealed class ApplicationUserModel
	{
        public string Id { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;

        public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();
    }
}