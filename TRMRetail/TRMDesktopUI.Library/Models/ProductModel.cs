using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
	public sealed class ProductModel
	{
		public int Id { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal RetailPrice { get; set; }
		public int QuantityInStock { get; set; }
		public bool IsTaxable { get; set; }

	}
}
