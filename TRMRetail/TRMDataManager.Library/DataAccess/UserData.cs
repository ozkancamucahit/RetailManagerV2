using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
	public sealed class UserData : IUserData
	{
		private readonly ISQLDataAccess sql;

		#region CTOR
		public UserData(ISQLDataAccess sql)
		{
			this.sql = sql;
		}
		#endregion

		public UserModel GetUserById(string Id)
		{
			IEnumerable<UserModel> result = null;
			try
			{

				result = sql.LoadData<UserModel, dynamic>("dbo.spUserLookUp", new { Id }, "TRMData");
			}
			catch
			{
				result = null;
			}

			return result?.First() ?? new UserModel();
		}
	}
}
