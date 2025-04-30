using CookbookApp.Models;
using CookbookApp.Services;
using CookbookApp.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CookbookApp.ViewModels
{
    internal class FavoriteRecipesViewModel : RecipesViewModel
    {
        public ObservableRangeCollection<Recipe> FavoriteRecipes { get; set; }
        public FavoriteRecipesViewModel(INavigation navigation) : base(navigation)
        {
            Navigation = navigation;
            Title = "Összes kedvenc";

            FavoriteRecipes = new ObservableRangeCollection<Recipe>();
        }

        public override async Task GetRecipes()
        {
            DisplayedRecipes.Clear();

            Title = "Összes kedvenc";
            await GetFavorites();
            DisplayedRecipes.AddRange(FavoriteRecipes);

            ingredients.Clear();
            ingredients.AddRange(await CookBookServer.GetAllRecipeIngs());
        }

        public override async Task OnSearch()
        {
            await base.OnSearch();
            await GetFilterIntersect();
        }

        public override async Task OnFilter()
        {
            await base.OnFilter();
            await GetFilterIntersect();
        }

        async Task GetFilterIntersect()
        {
            IEnumerable<Recipe> intersect = filteredRecipes.Intersect(FavoriteRecipes);
            if (!intersect.Any())
            {
                GetDisplayedRecipes(FavoriteRecipes);
                Title = "Összes kedvenc";
                await Application.Current.MainPage.DisplayAlert("Recept keresése",
                                                                "HIBA! Nincs megfelelő recept ami a megadott kritériumoknak megfelel.",
                                                                "Ok");              
            } else
                GetDisplayedRecipes(intersect);
        }
        public override async Task FilterRandom()
        {
            if (FavoriteRecipes.Count < 3)
                await Application.Current.MainPage.DisplayAlert("Lepj meg",
                                                                "HIBA! Nincs elég kedvenc recept, legalább 3-nak kell lennie, hogy ezt a funkciót használd",
                                                                "Oké");
            else
            {
                Title = "3 Véletlen recept";
                filteredRecipes.Clear();
                filteredRecipes.AddRange(await GetRandomRecipes());
            }
        }

        public override async Task GetRandomID(List<int> ids)
        {
            Random rand = new Random();
            int randNum = rand.Next(1, await CookBookServer.GetLastRecipeIndex() + 1);
            if (!await CookBookServer.RecipeExists(randNum) ||
                !(await CookBookServer.GetRecipe(randNum)).Favorited ||
                ids.Contains(randNum))
            {
                await GetRandomID(ids);
                return;
            }
            else
                ids.Add(randNum);
        }

        async Task GetFavorites()
        {
            FavoriteRecipes.Clear();
            FavoriteRecipes.AddRange(await CookBookServer.GetFavoriteRecipes());
        }
    } 
}