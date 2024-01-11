using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
	public sealed class StatusInfoViewModel : Screen
	{

        public string Header { get; private set; } = String.Empty;
        public string Message { get; private set; } = String.Empty;


        public void UpdateMessage(string header, string message)
        {
            Header = header;
            Message = message;

            NotifyOfPropertyChange(() => Header);
            NotifyOfPropertyChange(() => Message);
        }

        public void Close()
        {
            TryCloseAsync();
        }

    }
}
