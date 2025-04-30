using CookbookApp.Models;
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
    public partial class RecipesPage : ContentPage
    {
        public RecipesPage()
        {
            InitializeComponent();

            BindingContext = new RecipesViewModel(Navigation);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is RecipesViewModel vm)
            {
                _ = vm.OnPageAppearing();
            }
        }
    }
}