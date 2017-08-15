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
    public partial class BasketPage : ContentPage
    {
        public BasketPage()
        {
            InitializeComponent();
            ((BaseModel)BindingContext).Init(this);
        }

        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as BasketViewModel).AddOrUpdateBasket((Basket)e.Item);
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}