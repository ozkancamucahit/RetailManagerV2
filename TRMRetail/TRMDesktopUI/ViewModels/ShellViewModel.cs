using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<object>//, IHandle<LogOnEvent>
	{
		#region Fields
		private readonly IEventAggregator _events;
		private readonly SalesViewModel _salesViewModel;
		private readonly LoginViewModel loginViewModel;

		#endregion

		public ShellViewModel(LoginViewModel loginViewModel)
		{
			this.loginViewModel = loginViewModel;
			ActivateItem(this.loginViewModel);
		}


		//public ShellViewModel(
		//					  IEventAggregator events,
		//					  SalesViewModel salesViewModel)
		//{
		//	_events = events;
		//	_salesViewModel = salesViewModel;

		//	//_events.SubscribeOnPublishedThread(this);
		//	//ActivateItemAsync(IoC.Get<SalesViewModel>());
		//}

		//public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
		//{
		//	await ActivateItemAsync(_salesViewModel);
		//	//return Task.CompletedTask;
		//}

	}
}
