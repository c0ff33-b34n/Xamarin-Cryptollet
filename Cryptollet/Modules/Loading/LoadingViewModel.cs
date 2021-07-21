using Cryptollet.Common.Base;
using Cryptollet.Common.Navigation;
using Cryptollet.Modules.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Cryptollet.Modules.Loading
{
    public class LoadingViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public LoadingViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override Task InitializeAsync(object parameter)
        {
            if (!Preferences.ContainsKey(Constants.SHOWN_ONBOARDING))
            {
                Preferences.Set(Constants.SHOWN_ONBOARDING, true);
                _navigationService.GoToLoginFlow();
                return Task.CompletedTask;
            }

            if(Preferences.ContainsKey(Constants.IS_USER_LOGGED_IN) && Preferences.Get(Constants.IS_USER_LOGGED_IN, false))
            {
                _navigationService.GoToMainFlow();
                return Task.CompletedTask;
            }

            _navigationService.GoToLoginFlow();
            return _navigationService.InsertAsRoot<LoginViewModel>();
        }
    }
}
