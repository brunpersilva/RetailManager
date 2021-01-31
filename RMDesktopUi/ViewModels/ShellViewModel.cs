using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private ILoggedInUserModel _user;
        private IAPIHelper _aPIHelper;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM,
           ILoggedInUserModel user, IAPIHelper aPIHelper)
        {
            _events = events;
            _salesVM = salesVM;
            _user = user;
            _aPIHelper = aPIHelper;
            _events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
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
            TryClose();
        }
        public void LogOut()
        {
            _user.ResetUSerModel();
            _aPIHelper.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

    }
}
