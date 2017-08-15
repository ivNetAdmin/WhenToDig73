
using Realms;
using Wtd.Core.Views;
using Xamarin.Forms;

namespace Wtd.Core.ViewModels
{
    public class ReportViewModel : BaseModel
    {
        private readonly Realm _realm;

        public Command JobClickedCommand { get; }

        public ImageSource JobIcon { get { return ImageSource.FromFile("job.png"); } }

        public ReportViewModel()
        {
            _realm = Realm.GetInstance();

            JobClickedCommand = new Command(JobClicked);
        }

        internal void JobClicked()
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}
