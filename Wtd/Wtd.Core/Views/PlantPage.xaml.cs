
using Wtd.Core.Models;
using Wtd.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wtd.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlantPage : ContentPage
    {
        public PlantPage()
        {
            InitializeComponent();
            ((BaseModel)BindingContext).Init(this);
        }

        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as PlantViewModel).AddOrUpdatePlant((Plant)e.Item);
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}