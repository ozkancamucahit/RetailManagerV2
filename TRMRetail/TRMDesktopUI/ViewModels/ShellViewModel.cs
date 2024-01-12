using Caliburn.Micro;
using System;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.API;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
	{
		#region Fields
		private readonly IEventAggregator _events;
		private readonly ILoggedInUserModel _loggedInUserModel;
		private readonly IAPIHelper _aPIHelper;
		private readonly LoginViewModel _loginViewModel;

		#endregion

		public ShellViewModel(
							  IEventAggregator events,
							  //LoginViewModel loginViewModel
							  ILoggedInUserModel loggedInUserModel,
							  IAPIHelper aPIHelper
			)
		{
			_events = events;
			this._loggedInUserModel = loggedInUserModel;
			this._aPIHelper = aPIHelper;
			//_loginViewModel = loginViewModel;

			_events.SubscribeOnPublishedThread(this);
			ActivateItemAsync(IoC.Get<LoginViewModel>());
		}


		public bool IsLoggedIn
		{
			get
			{
				bool output = false;

				if ( String.IsNullOrWhiteSpace(_loggedInUserModel?.Token) == false )
				{
					output = true;
				}

				return output;
			}
		}

		public async Task ExitApplication()
		{
			await TryCloseAsync();
		}

		public async Task UserManagement()
		{
			await ActivateItemAsync(IoC.Get<UserDisplayViewModel>());

		}

		public async Task LogOut()
		{
			_loggedInUserModel.ResetUserModel();
			_aPIHelper.LogOffUser();
			await ActivateItemAsync(IoC.Get<LoginViewModel>());
			NotifyOfPropertyChange(() => IsLoggedIn);

		}

		public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
		{
			await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
			NotifyOfPropertyChange(() => IsLoggedIn);
		}
	}
}
