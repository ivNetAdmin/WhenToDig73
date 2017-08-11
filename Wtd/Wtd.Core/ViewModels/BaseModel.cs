
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Wtd.Core.ViewModels
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        protected Page CurrentPage { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Init(Page page)
        {
            CurrentPage = page;
            NavigationPage.SetHasBackButton(page, false);
            CurrentPage.Appearing += CurrentPageOnAppearing;           
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void CurrentPageOnAppearing(object sender, EventArgs eventArgs) { }
    }
}
