using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.Library.API;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.ViewModels
{
	public sealed class UserDisplayViewModel : Screen
	{

		#region FIELDS
		private readonly StatusInfoViewModel statusInfoViewModel;
		private readonly IWindowManager windowManager;
		private readonly IUserEndpoint userEndpoint;

		#endregion

		#region CTOR
		public UserDisplayViewModel(StatusInfoViewModel statusInfoViewModel, IWindowManager windowManager, IUserEndpoint userEndpoint)
        {
			this.statusInfoViewModel = statusInfoViewModel;
			this.windowManager = windowManager;
			this.userEndpoint = userEndpoint;
		}

		#endregion

		BindingList<UserModel> _users;

		public BindingList<UserModel> Users
		{
			get
			{
				return _users;
			}

			set { 

				_users = value; 
				NotifyOfPropertyChange(() => Users);
			}
		}
        private async Task LoadUsers()
		{
			var userList = await userEndpoint.GetAll();
			Users = new BindingList<UserModel>(userList.ToList());
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			try
			{
				await LoadUsers();
			}
			catch (Exception ex)
			{
				dynamic settings = new ExpandoObject();
				settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				settings.ResizeMode = ResizeMode.NoResize;
				settings.Title = "System Error";

				if (ex.Message == "Unauthorized")
				{
					statusInfoViewModel.UpdateMessage("Unauthorized Access", "You do not have the right permissions.");
					windowManager.ShowDialog(statusInfoViewModel, null, settings);
				}
				else
				{
					statusInfoViewModel.UpdateMessage("Fatal Exception", ex.Message);
					windowManager.ShowDialog(statusInfoViewModel, null, settings);
				}
				TryClose();
			}
		}
	}
}
