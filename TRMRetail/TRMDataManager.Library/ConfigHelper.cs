using System;
using System.Configuration;

namespace TRMDataManager.Library
{
	public sealed class ConfigHelper
	{
		public static decimal GetTaxRate()
		{
			decimal output = 0;

			var rateText = ConfigurationManager.AppSettings["taxRate"];

			bool isValid = decimal.TryParse(rateText, out output);

			return isValid ? output : throw new InvalidCastException("Geçersiz vergi değeri.");

		}
	}
}
