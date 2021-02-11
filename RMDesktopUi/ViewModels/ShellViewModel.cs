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
        private readonly SalesViewModel _salesVM;
        private readonly ILoggedInUserModel _user;
        private readonly IAPIHelper _aPIHelper;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM,
           ILoggedInUserModel user, IAPIHelper aPIHelper)
        {
            _events = events;
            _salesVM = salesVM;
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
        }

        //public void Handle(LogOnEvent message)
        //{
            
        //}

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
