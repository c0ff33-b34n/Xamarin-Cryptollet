using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptollet.Modules.Assets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssetsView : ContentPage
    {
        public AssetsView()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<AssetsViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as AssetsViewModel).InitializeAsync(null);
        }
    }
}