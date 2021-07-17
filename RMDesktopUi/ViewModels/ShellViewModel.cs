using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUi.EventModels;
using RMDesktopUi.Library.Api;
using RMDesktopUi.Views;
using RMDesktopUI.Library.Models;

namespace RMDesktopUi.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly IEventAggregator _events;
        private readonly ILoggedInUserModel _user;
        private readonly IAPIHelper _aPIHelper;

        public ShellViewModel(IEventAggregator events,
           ILoggedInUserModel user, IAPIHelper aPIHelper)
        {
            _events = events;
            _user = user;
            _aPIHelper = aPIHelper;
            _events.SubscribeOnPublishedThread(this);

           ActivateItemAsync(IoC.Get<LoginViewModel>());
        }
        public bool IsLoggedIn
        {
            get
            {
                bool output = false;
                if (!string.IsNullOrWhiteSpace(_user.Token))
                {
                    output = true;
                }
                return output;
            }
        }
        public bool IsLoggedOut
        {
            get { return !IsLoggedIn; }
        }

        public void ExitApplication()
        {
           TryCloseAsync();
        }
        public async Task UserManagment()
        {
           await ActivateItemAsync(IoC.Get<UserDisplayViewModel>());  
        }
        public async Task LogOut()
        {
            _user.ResetUSerModel();
            _aPIHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
            NotifyOfPropertyChange(() => IsLoggedOut);
        }
        public async Task Login()
        {
            await ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
            NotifyOfPropertyChange(() => IsLoggedOut);
        }
    }
}
