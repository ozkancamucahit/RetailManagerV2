using AutoMapper;
using Caliburn.Micro;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TRMDesktopUI.Library.API;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.ViewModels
{
	public sealed class SalesViewModel : Screen
	{
		#region Fields
		private BindingList<ProductDisplayModel> _products;
		private IProductEndPoint _productEndPoint;
		private ISaleEndPoint _saleEndPoint;
		private IConfigHelper _configHelper;
		private IMapper _mapper;

		public BindingList<ProductDisplayModel> Products
		{
			get { return _products; }
			set
			{
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		private ProductDisplayModel _selectedPRoduct;

		public ProductDisplayModel SelectedProduct
		{
			get { return _selectedPRoduct; }
			set
			{
				_selectedPRoduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);

			}
		}

		private CartItemDisplayModel _selectedCartItem;


		public CartItemDisplayModel SelectedCartItem
		{
			get { return _selectedCartItem; }
			set
			{
				_selectedCartItem = value;
				NotifyOfPropertyChange(() => SelectedCartItem);
				NotifyOfPropertyChange(() => CanRemoveFromCart);

			}
		}


		private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

		public BindingList<CartItemDisplayModel> Cart
		{
			get { return _cart; }
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}


		private int _itemQuantity = 1;

		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set
			{
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		public bool CanAddToCart
		{
			get
			{
				bool output = false;

				if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
				{
					output = true;
				}

				return output;
			}
		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;
				if(SelectedCartItem != null && SelectedCartItem?.Product.QuantityInStock > 0)
				{
					output = true;
				}

				return output;
			}
		}


		public string SubTotal
		{
			get
			{
				decimal subTotal = CalculateSubTotal();


				return subTotal.ToString("C");
			}
		}

		public string Total
		{
			get
			{
				decimal total = CalculateTax() + CalculateSubTotal();
				return total.ToString("C");
			}
		}

		public string Tax
		{
			get
			{
				decimal taxAmount = CalculateTax();

				return taxAmount.ToString("C");
			}
		}

		

		#endregion

		#region CTOR
		public SalesViewModel(
			IProductEndPoint productEndPoint,
			IConfigHelper configHelper,
			ISaleEndPoint saleEndPoint,
			IMapper mapper)
		{
			_productEndPoint = productEndPoint;
			_configHelper = configHelper;
			_saleEndPoint = saleEndPoint;
			_mapper = mapper;
		}
		#endregion


		#region Funcs

		private decimal CalculateSubTotal()
		{
			return Cart.Sum(M => M.Product.RetailPrice * M.QuantityInCart);
		}

		private decimal CalculateTax()
		{
			decimal taxAmount = 0;
			decimal taxRate = _configHelper.GetTaxRate() / 100;
			taxAmount = Cart
					.Where(m => m.Product.IsTaxable)
					.Sum(M => M.Product.RetailPrice * M.QuantityInCart * taxRate);

			return taxAmount;
		}


		public void AddToCart()
		{

			CartItemDisplayModel existingModel = Cart.FirstOrDefault(m => m.Product == SelectedProduct);

			if (existingModel != null)
			{
				existingModel.QuantityInCart += ItemQuantity;
				//Refresh the cart
				//Cart.Remove(existingModel);
				//Cart.Add(existingModel);
			}
			else
			{
				var item = new CartItemDisplayModel()
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity
				};

				Cart?.Add(item);
			}

			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Cart);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		public void RemoveFromCart()
		{


			SelectedCartItem.Product.QuantityInStock += 1;
			if (SelectedCartItem.QuantityInCart > 1)
			{
				SelectedCartItem.QuantityInCart -= 1;
			}
			else
			{
				Cart.Remove(SelectedCartItem);
			}



			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);

		}


		public bool CanCheckOut
		{
			get
			{
				bool output = false;

				if (Cart.Count > 0)
				{
					output = true;
				}

				return output;
			}
		}

		public async Task CheckOut()
		{
			//TODO: POST salemodel to API
			SaleModel sale = new SaleModel();

			foreach (var item in Cart)
			{
				sale.SaleDetails.Add(new SaleDetail()
				{
					ProductId = item.Product.Id,
					Quantity = item.QuantityInCart
				});
			}

			await _saleEndPoint.PostSale(sale);


		}

		private async Task LoadProducts()
		{

			var productsList = await _productEndPoint.GetProducts();
			var products = _mapper.Map<IEnumerable<ProductDisplayModel>>(productsList); 
			Products = new BindingList<ProductDisplayModel>(products.ToList());
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await LoadProducts();
		}

		#endregion


	}
}
