using System;
using System.Configuration;

namespace TRMDesktopUI.Library.Helpers
{
	public sealed class ConfigHelper : IConfigHelper
	{
		public decimal GetTaxRate()
		{
			decimal output = 0;

			var rateText = ConfigurationManager.AppSettings["taxRate"];

			bool isValid = decimal.TryParse(rateText, out output);

			return isValid ? output : throw new InvalidCastException("Geçersiz vergi değeri.");

		}
	}
}
