
using Android.App;
using Android.Support.V7.App;

namespace Wtd.Droid
{
    [Activity(Label = "When To Dig", Icon = "@drawable/icon", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}