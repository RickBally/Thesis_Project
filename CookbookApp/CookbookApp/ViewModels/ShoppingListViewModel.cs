using CookbookApp.Models;
using CookbookApp.Services;
using CookbookApp.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Xamarin.Forms.Internals.GIFBitmap;

namespace CookbookApp.ViewModels
{
    internal class ShoppingListViewModel : BaseViewModel
    {
        public ObservableRangeCollection<ShoppingItem> ShoppingItems { get; set; }

        public AsyncCommand<ShoppingItem> SelectCommand { get; }
        public AsyncCommand ToPantryCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand DeleteCommand { get; }

        ShoppingItem selectedItem;
        List<ShoppingItem> selectedItems;
        bool pantryButtonVisible;
        bool clearButtonVisible;

        public ShoppingItem SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }
        public bool PantryButtonVisible
        {
            get => pantryButtonVisible;
            set => SetProperty(ref pantryButtonVisible, value);
        }
        public bool ClearButtonVisible
        {
            get => clearButtonVisible;
            set => SetProperty(ref clearButtonVisible, value);
        }

        public ShoppingListViewModel()
        {
            Title = "Bevásárló lista";

            selectedItems = new List<ShoppingItem>();
            ShoppingItems = new ObservableRangeCollection<ShoppingItem>();
            SelectCommand = new AsyncCommand<ShoppingItem>(OnSelect);
            ToPantryCommand = new AsyncCommand(OnToPantry);
            AddCommand = new AsyncCommand(OnAdd);
            DeleteCommand = new AsyncCommand(OnDelete);
        }

        async Task GetShoppingList()
        {
            ShoppingItems.Clear();
            ShoppingItems.AddRange((await CookBookServer.GetShoppingItems()).OrderBy(si => si.ShoppingIng.Category.Name));

            await UpdateButtonVisibility();
        }

        async Task OnAdd()
        {
            //TODO add Delay to open again, cause it is glitchy if you tap it fast
            var ing = await CookBookServer.ChooseIngredient();
            if (ing == null)
                return;
            
            //Todo Catch
            if (ing.Name == "Err")
                await OnAdd();
            else
            {
                await AddIng(ing, "Szükséges mennyiség");
                await OnAdd();
            }
        }

        async Task AddIng(Ingredient ing, string dispText)
        {
            float ammount = await GetAmmount(ing.Name, dispText);

            if (ammount == 0)
            {
                await AddIng(ing, "Mennyiség nem lehet 0, adj meg újat!");
                return;
            }
            else if (ammount == -1)
                return;

            if (ShoppingItems.Where(si => si.IngName == ing.Name).Any())
                await CookBookServer.UpdateShoppingAmmount(ing.Name, ammount);
            else
                await CookBookServer.AddShoppingItem(ing.Name, ammount);

            await GetShoppingList();
        }

        async Task<float> GetAmmount(string ingName, string dispText)
        {
            await Task.Delay(100); // Delay to fix problem regarding awaitng 2 DisplayPrompts after each other
            string ammountquerry = await Application.Current.MainPage.DisplayPromptAsync(ingName,
                                                                                         dispText,
                                                                                         "Oké",
                                                                                         "Más hozzávaló",
                                                                                         "0",
                                                                                         keyboard: Keyboard.Numeric);
            if (ammountquerry == null)
                return -1;
            
            return Math.Abs(float.Parse(ammountquerry));
        }

        async Task OnSelect(ShoppingItem item)
        {
            if (item == null)
                return;

            SelectedItem = null;

            await CookBookServer.UpdateSelected(item.IngName);

            await UpdateButtonVisibility();
        }

        async Task OnDelete()
        {
            if (selectedItems.Count > 0)
                await OnRemove();
            else
                await OnClear();
        }
        async Task OnClear()
        {
            bool remConfirmed = await Application.Current.MainPage.
               DisplayAlert("Kiürítés", "Bevásárló lista kiütítése?", "Igen", "Nem");

            if (!remConfirmed)
                return;
            
            await CookBookServer.ClearShoppingList();
            selectedItems.Clear();
            await GetShoppingList();
        }

        async Task OnRemove()
        {
            bool remConfirmed = await Application.Current.MainPage.
               DisplayAlert("Eltávolítás", "Kiejlölt elemek eltávolítása listáról?", "Igen", "Nem");

            if (!remConfirmed)
                return;
            
            await RemoveItems();
            await GetShoppingList();
        }
        async Task OnToPantry()
        {
            foreach ( var item in selectedItems )
            {
                if (!await CookBookServer.InventoryContains(item.IngName))
                    await CookBookServer.AddToInventory(item.IngName, item.Ammount);
                else
                    await CookBookServer.UpdateInventoryAmmount(item.IngName, item.Ammount);
            }

            await RemoveItems();
            
            await GetShoppingList();
        }
        async Task UpdateButtonVisibility()
        {
            selectedItems.Clear();
            selectedItems.AddRange(await CookBookServer.GetSelectedShoppingItems());

            if (selectedItems.Count > 0)
                PantryButtonVisible = true;
            else
                PantryButtonVisible = false;

            if (ShoppingItems.Count > 0)
                ClearButtonVisible = true;
            else
                ClearButtonVisible = false;
        }

        public async Task OnPageAppearing()
        {
            await GetShoppingList();
        }

        async Task RemoveItems()
        {
            if (selectedItems.Count == ShoppingItems.Count)
            {
                await OnClear();
                return;
            }

            foreach (var item in new List<ShoppingItem>(selectedItems))
                await CookBookServer.RemoveFromShoppingList(item.IngName);
            selectedItems.Clear();
        }
    }
}
