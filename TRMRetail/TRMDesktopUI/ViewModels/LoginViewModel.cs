using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
	public class LoginViewModel : Screen
	{
		private string _userName;
		private string _password;
		private IAPIHelper _apiHelper;
		private readonly IEventAggregator _events;

		public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
		{
			_apiHelper = apiHelper;
			_events = events;
		}

		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				NotifyOfPropertyChange(() => UserName);
				NotifyOfPropertyChange(() => CanLogin);

			}
		}



		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanLogin);
			}
		}

		public bool IsErrorVisible
		{
			get
			{
				bool output = false;

				if (ErrorMessage?.Length > 0)
				{
					output = true;
				}

				return output;
			}
		}

		private string _ErrorMessage;

		public string ErrorMessage
		{
			get { return _ErrorMessage; }
			set
			{
				_ErrorMessage = value;
				NotifyOfPropertyChange(() => ErrorMessage);
				NotifyOfPropertyChange(() => IsErrorVisible);
			}
		}



		public bool CanLogin
		{
			get
			{
				bool output = false;
				if (UserName?.Length > 0 && Password?.Length > 0)
				{
					output = true;
				}
				return output;
			}
		}

		public async Task btnLogin()
		{
			try
			{
				ErrorMessage = "";
				AuthenticatedUser result = await _apiHelper.Authenticate(UserName, Password);
				await _apiHelper.GetLoggedInUserInfo(result.access_token);

				await _events.PublishOnUIThreadAsync(new LogOnEvent());
			}
			catch (Exception ex)
			{

				ErrorMessage = ex.Message;
			}
		}

	}
}
