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

		private UserModel _selectedUser;

		public UserModel SelectedUser
		{
			get { return _selectedUser; }
			set { 
				_selectedUser = value;
				SelectedUserName = value.Email;
				UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
				LoadRoles();
				NotifyOfPropertyChange(() => SelectedUser);
			}
		}

		private string _selectedUserRole;

		public string SelectedUserRole
		{
			get { return _selectedUserRole; }
			set { 
				_selectedUserRole = value; 
				NotifyOfPropertyChange(() => SelectedUserRole);
			}
		}

		private string _selectedAvailableRole;

		public string SelectedAvailableRole
		{
			get { return _selectedAvailableRole; }
			set { 
				_selectedAvailableRole = value;
				NotifyOfPropertyChange(() => SelectedAvailableRole);
			}
		}




		private string _selectedUserName;

		public string SelectedUserName
		{
			get { 
				return _selectedUserName;
			}
			set { 
				_selectedUserName = value;
				NotifyOfPropertyChange(() => SelectedUserName);
			}
		}

		private BindingList<string> _UserRoles = new BindingList<string>();

		public BindingList<string> UserRoles
		{
			get { return _UserRoles; }
			set { 
				_UserRoles = value;
				NotifyOfPropertyChange(() => UserRoles);
			}
		}
		
		private BindingList<string> _availableRoles = new BindingList<string>();

		public BindingList<string> AvailableRoles
		{
			get { return _availableRoles; }
			set {
				_availableRoles = value;
				NotifyOfPropertyChange(() => AvailableRoles);
			}
		}

		private async Task LoadRoles()
		{
			var roles = await userEndpoint.GetAllRoles();

			foreach (var role in roles)
			{
				if (UserRoles.IndexOf(role.Value) < 0)
				{
					AvailableRoles.Add(role.Value);
				}
			}
		}

		public async void AddSelectedRole()
		{
			await userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);

			UserRoles.Add(SelectedAvailableRole);
			AvailableRoles.Remove(SelectedAvailableRole);
		}
		
		public async void RemoveSelectedRole()
		{
			await userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);

			AvailableRoles.Add(SelectedUserRole);
			UserRoles.Remove(SelectedUserRole);
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
					await windowManager.ShowDialogAsync(statusInfoViewModel, null, settings);
				}
				else
				{
					statusInfoViewModel.UpdateMessage("Fatal Exception", ex.Message);
					await windowManager.ShowDialogAsync(statusInfoViewModel, null, settings);
				}
				TryCloseAsync();
			}
		}
	}
}
