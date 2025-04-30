using CookbookApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CookbookApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoriteRecipesPage : ContentPage
    {
        public FavoriteRecipesPage()
        {
            InitializeComponent();
            BindingContext = new FavoriteRecipesViewModel(Navigation);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is FavoriteRecipesViewModel vm)
            {
                _ = vm.OnPageAppearing();
            }
        }
    }
}