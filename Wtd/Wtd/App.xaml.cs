
using Wtd.Core.ViewModels;
using Wtd.Core.Views;
using Xamarin.Forms;

namespace Wtd
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            var mainPage = new MainPage
            {
                BindingContext = new MainViewModel()
            };

            MainPage = new NavigationPage(mainPage);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
