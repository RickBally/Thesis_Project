using CookbookApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CookbookApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                if (Current != null)
                {
                    Current.UserAppTheme = OSAppTheme.Light;
                }
            }

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
