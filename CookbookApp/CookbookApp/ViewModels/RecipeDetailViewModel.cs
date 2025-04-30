using CookbookApp.Models;
using CookbookApp.Services;
using CookbookApp.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CookbookApp.ViewModels
{
    internal class RecipeDetailViewModel : BaseViewModel
    {
        Recipe currentRecipe;
        string prepButtonText;

        public ObservableRangeCollection<RecipeIngredient> Ingredients { get; set; }
        public INavigation Navigation { get; set; }
        public string PrepareButtonText
        { 
            get => prepButtonText;
            set => SetProperty(ref prepButtonText, value);
        }
        public Recipe CurrentRecipe
        {
            get => currentRecipe;
            set => SetProperty(ref currentRecipe, value);
        }

        public AsyncCommand FollowRecipeCommand { get; set; }
        public AsyncCommand ShoppingCommand { get; set; }
        public AsyncCommand DeleteCommand { get; set; }


        public RecipeDetailViewModel(INavigation navigation)
        {
            FollowRecipeCommand = new AsyncCommand(onRecipeFollow);
            ShoppingCommand = new AsyncCommand(OnShopping);
            DeleteCommand = new AsyncCommand(OnDelete);
            Ingredients = new ObservableRangeCollection<RecipeIngredient>();
            Title = "Recept Információ";

            MessagingCenter.Subscribe<RecipesViewModel, Recipe>
            (this, "RecipeSent", async (sender, arg) =>
            {
                CurrentRecipe = arg;
                await GetLists(arg.ID);
                PrepareButtonText = "Recept elkészítése (" + arg.CookTime + ")";
            });
            Navigation = navigation;
        }

        async Task GetLists(int id)
        {
            Ingredients.Clear();
            var ings = await CookBookServer.GetRecipeIngs(id);
            Ingredients.AddRange(ings);
        }

        async Task OnShopping()
        {
            bool shopConfirmed = await Application.Current.MainPage.DisplayAlert("Bevásráló lista készítése", "Biztos hozzá akarod adni ezeket a hozzávalókat a listádhoz?", "Igen", "Nem");

            if (shopConfirmed)
            {
                foreach (var ing in Ingredients)
                {
                    if (await CookBookServer.ShoppingListContains(ing.Ingredient.Name))
                        await CookBookServer.UpdateShoppingAmmount(ing.Ingredient.Name, ing.Ammount);
                    else
                    {
                        await CookBookServer.AddShoppingItem(ing.Ingredient.Name, ing.Ammount);
                        await InventoryDeduction(ing.Ingredient);
                    }
                }
            }
        }
        
        async Task OnDelete()
        {
            bool delConfirmed = await Application.Current.MainPage.DisplayAlert("Recept törlése", "Biztos törölni akarod ezt a receptet?", "Igen", "Nem");

            if(delConfirmed)
            {
                await CookBookServer.RemoveRecipeIngs(CurrentRecipe.ID);
                await CookBookServer.RemoveRecipeSteps(CurrentRecipe.ID);
                await CookBookServer.RemoveRecipe(CurrentRecipe.ID);

                await Navigation.PopAsync();
            }
        }

        async Task onRecipeFollow()
        {
            if (!await CanPrepare())
            {
                await Application.Current.MainPage.DisplayAlert(
                "Recept elkészítése",
                "HIBA! Nem rendelkezel minden szükséges hozzávalóval",
                "Ok");
                return;
            }
            bool prepConfirm = await Application.Current.MainPage.DisplayAlert(
                "Recept elkészítése",
                "Biztos elakarod készíteni ezt a receptet? A spájzban lévő hozzávalók automatikusan ellesznek távolítva.",
                "Ok",
                "Mégse");

            if (prepConfirm)
            {
                foreach (RecipeIngredient ing in Ingredients)
                {
                    await CookBookServer.UpdateInventoryAmmount(ing.Ingredient.Name, -ing.Ammount);
                }

                await Navigation.PushAsync(new RecipeFollowPage());
                MessagingCenter.Send(this, "RecipeIDSent", currentRecipe.ID);
            }
        }

        async Task InventoryDeduction(Ingredient ing)
        {
            if (await CookBookServer.InventoryContains(ing.Name))
            {
                var invItem = await CookBookServer.GetInventoryItem(ing.Name);

                await CookBookServer.UpdateShoppingAmmount(ing.Name, -invItem.Ammount);
            }
        }

        async Task<bool> CanPrepare()
        {
            foreach (RecipeIngredient ing in Ingredients)
            {
                if (!await CookBookServer.InventoryContains(ing.Ingredient.Name))
                    return false;
                else if ((await CookBookServer.GetInventoryItem(ing.Ingredient.Name)).Ammount < ing.Ammount)
                    return false;
            }

            return true;
        }
    }
}
