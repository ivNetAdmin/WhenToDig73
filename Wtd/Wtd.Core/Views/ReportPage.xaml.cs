using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wtd.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wtd.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage : ContentPage
    {
        public ReportPage()
        {
            InitializeComponent();
            ((BaseModel)BindingContext).Init(this);
        }
    }
}