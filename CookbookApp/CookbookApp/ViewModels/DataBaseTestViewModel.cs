using CookbookApp.Models;
using CookbookApp.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CookbookApp.ViewModels
{
    internal class DataBaseTestViewModel : BaseViewModel
    {
        public ObservableRangeCollection<RecipeIngredient> DBItems { get; set; }

        public DataBaseTestViewModel()
        {
            Title = "DATABASE TEST";

            DBItems = new ObservableRangeCollection<RecipeIngredient>();

            _ = Test();
        }

        async Task Test()
        {
            //var category = await CookBookServer.GetIngCategory("Zöldség");

            //await CookBookServer.AddIngredient("Paradicsom", category, "tomato.png", "g");

            var dbItems = await CookBookServer.GetRecipeIngs(3);
            DBItems.AddRange(dbItems);
        }
    }
}