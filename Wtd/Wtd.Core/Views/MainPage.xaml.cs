using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wtd.Core.Models;
using Wtd.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wtd.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as MainViewModel).AddOrUpdateJob((Job)e.Item);
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}