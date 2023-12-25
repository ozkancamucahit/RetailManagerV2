﻿using Caliburn.Micro;
using TRMDesktopUI.EventModels;

namespace TRMDesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
	{
		#region Fields
		private readonly IEventAggregator _events;
		private readonly SalesViewModel _salesViewModel;
		private readonly LoginViewModel _loginViewModel;

		#endregion

		//public ShellViewModel(LoginViewModel loginViewModel)
		//{
		//	this.loginViewModel = loginViewModel;
		//	ActivateItem(this.loginViewModel);
		//}

		public ShellViewModel(
							  IEventAggregator events,
							  //SalesViewModel salesViewModel
							  LoginViewModel loginViewModel
			)
		{
			_events = events;
			//_salesViewModel = salesViewModel;
			_loginViewModel = loginViewModel;
			_events.Subscribe(this);
			ActivateItem(IoC.Get<LoginViewModel>());
		}

		public void Handle(LogOnEvent message)
		{
			ActivateItem(_loginViewModel);
		}



		//public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
		//{
		//	await ActivateItemAsync(_salesViewModel);
		//	//return Task.CompletedTask;
		//}

	}
}
