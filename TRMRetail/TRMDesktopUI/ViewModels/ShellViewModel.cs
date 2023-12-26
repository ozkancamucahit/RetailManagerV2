﻿using Caliburn.Micro;
using System;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
	{
		#region Fields
		private readonly IEventAggregator _events;
		private readonly SalesViewModel _salesViewModel;
		private readonly ILoggedInUserModel _loggedInUserModel;
		private readonly LoginViewModel _loginViewModel;

		#endregion

		public ShellViewModel(
							  IEventAggregator events,
							  SalesViewModel salesViewModel,
							  //LoginViewModel loginViewModel
							  ILoggedInUserModel loggedInUserModel
			)
		{
			_events = events;
			_salesViewModel = salesViewModel;
			this._loggedInUserModel = loggedInUserModel;
			//_loginViewModel = loginViewModel;

			_events.Subscribe(this);
			ActivateItem(IoC.Get<LoginViewModel>());
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

		public void Handle(LogOnEvent message)
		{
			ActivateItem(_salesViewModel);
			NotifyOfPropertyChange(() => IsLoggedIn);
		}

		public void ExitApplication()
		{
			TryClose();
		}

		public void LogOut()
		{
			_loggedInUserModel.LogOffUser();
			ActivateItem(IoC.Get<LoginViewModel>());
			NotifyOfPropertyChange(() => IsLoggedIn);

		}

	}
}
