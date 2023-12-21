using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManager.Library.Models
{
	public sealed class UserModel
	{
		public string Id { get; set; } = String.Empty;
		public string FirstName { get; set; } = String.Empty;
		public string LastName { get; set; } = String.Empty;
		public string EmailAddress { get; set; } = String.Empty;
		private DateTime _CreatedDate;

		public DateTime CreatedDate
		{
			get { return _CreatedDate.ToLocalTime(); }
			set { _CreatedDate = value; }
		}


	}
}
