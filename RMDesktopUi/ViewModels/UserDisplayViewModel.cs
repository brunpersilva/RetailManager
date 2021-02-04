using AutoMapper;
using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Helpers;
using RMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RMDesktopUi.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly IUserEndpoint _userEndpoint;
        //private readonly IMapper _mapper;
        private readonly IConfigHelper _configHelper;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;

        private BindingList<UserModel> _users;
        public BindingList<UserModel> Users
        {
            get { return _users; }
            set { _users = value; NotifyOfPropertyChange(() => Users); }
        }

        public UserDisplayViewModel(IUserEndpoint userEndpoint, IConfigHelper configHelper, StatusInfoViewModel status, IWindowManager window)
        {
            _userEndpoint = userEndpoint;
            _configHelper = configHelper;
            _status = status;
            _window = window;
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
                dynamic setting = new ExpandoObject();
                setting.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                setting.ResizeMode = ResizeMode.NoResize;
                setting.Title = "System Error";

                //var info = IoC.Get<StatusInfoViewModel>();

                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Unothorized Acess", "You do not have permition to interact with the sales form.");
                    _window.ShowDialog(_status, null, setting);
                }
                else
                {
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    _window.ShowDialog(_status, null, setting);
                }
                TryClose();
            }

        }
        private async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAll();
            Users = new BindingList<UserModel>(userList);
        }
    }
}
