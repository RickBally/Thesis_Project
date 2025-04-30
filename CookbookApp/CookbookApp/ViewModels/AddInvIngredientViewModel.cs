using CookbookApp.Models;
using CookbookApp.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CookbookApp.ViewModels
{
    internal class AddInvIngredientViewModel : BaseViewModel
    {
        Ingredient selectedIng;
        IngCategory selectedCategory;

        public ObservableRangeCollection<Ingredient> Ingredients { get; set; }
        public ObservableRangeCollection<IngCategory> Categories { get; set; }
        public string Ammount { get; set; }

        public AsyncCommand AddIngredient { get; }
        public AsyncCommand IngChooseCommand { get; }

        public AddInvIngredientViewModel()
        {
            Title = "Hozzávaló felvétele";
            Ingredients = new ObservableRangeCollection<Ingredient>();
            Categories = new ObservableRangeCollection<IngCategory>();

            AddIngredient = new AsyncCommand(OnAddIngredient);
            IngChooseCommand = new AsyncCommand(OnIngChoose);

            _ = GetProperties();
        }

       public Ingredient SelectedIngredient
        {
            get => selectedIng;
            set => SetProperty(ref selectedIng, value);
        }
       
        public IngCategory SelectedCategory
        {
            get => selectedCategory;
            set
            { 
                SetProperty(ref selectedCategory, value);
                _ = UpdateCategory(selectedCategory.Name);
            }
        }

        async Task UpdateCategory(string category)
        {
            Ingredients.Clear();

            if (category != "Minden")
                Ingredients.AddRange(await CookBookServer.GetIngFromCategory(category));
            else
                Ingredients.AddRange(await CookBookServer.GetIngredients());
        }

        async Task GetProperties()
        {
            Ingredients.AddRange(await CookBookServer.GetIngredients());
            Categories.AddRange(await CookBookServer.GetIngCategories());
        }

        async Task OnIngChoose()
        {
            Ingredient ingredient = await CookBookServer.ChooseIngredient();
            if (ingredient != null)
                SelectedIngredient = ingredient;
        }

        async Task OnAddIngredient()
        {
            if (SelectedIngredient == null)
            {
                await Application.Current.MainPage.DisplayAlert("HIBA", "Nem volt hozzávaló kijelölve!", "OK");
                return;
            }

            if (Ammount == null)
            {
                await Application.Current.MainPage.DisplayAlert("HIBA", "Nincs mennyiség megadva!", "OK");
                return;
            }

            float ammount = Math.Abs(float.Parse(Ammount));
            string ingName = SelectedIngredient.Name;

            if (ammount == 0)
            {
                await Application.Current.MainPage.DisplayAlert("HIBA", "Nem lehet a mennyiség 0!", "OK");
                return;
            }

            if (await CookBookServer.InventoryContains(ingName))
            {
                await CookBookServer.UpdateInventoryAmmount(ingName, ammount);
                await Application.Current.MainPage.DisplayAlert("Frissités", "Mivel a kijelölt hozzávaló már szerepel a leltárban, ezért frissítettük a tárolt mennyiségét!", "OK");
                return;
            }

            await CookBookServer.AddToInventory(ingName, ammount);
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
