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
	public sealed class UserData
	{
		private readonly IConfiguration configuration;

		#region CTOR
		public UserData(IConfiguration configuration)
        {
			this.configuration = configuration;
		}
        #endregion

        public UserModel GetUserById(string id)
		{
			IEnumerable<UserModel> result = null;
			try
			{
				SQLDataAccess sql = new SQLDataAccess(configuration);
				var p = new { Id = id };

				result = sql.LoadData<UserModel, dynamic>("dbo.spUserLookUp", p, "TRMData");
			}
			catch
			{
				result = null;
			}

			return result?.First() ?? new UserModel();
		}
	}
}
