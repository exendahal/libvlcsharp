using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayervlc
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Home();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            try { DependencyService.Get<DeleteFile>().DeleteTemp("temp"); } catch { }
        }

        protected override void OnResume()
        {
        }
    }
}
