using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using System.Windows.Input;
using Xamarin.Forms;
using CookbookApp.Models;
using System.Linq;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using CookbookApp.Services;
using CookbookApp.Views;

namespace CookbookApp.ViewModels
{
    public class InventoryViewModel : BaseViewModel
    {
        InventoryItem selectedInvItem;
        string ammountDisplay;

        public ObservableRangeCollection<InventoryItem> InventoryItems { get; set; }

        public INavigation Navigation { get; set; }

        public AsyncCommand<InventoryItem> SelectCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand ClearCommand { get; }


        public InventoryViewModel(INavigation navigation)
        {
            Title = "Spájz";

            InventoryItems = new ObservableRangeCollection<InventoryItem>();

            SelectCommand = new AsyncCommand<InventoryItem>(OnSelectIngredient);
            AddCommand = new AsyncCommand(OnAddIngredient);
            ClearCommand = new AsyncCommand(OnClearInventory);
            Navigation = navigation;
        }

        public InventoryItem SelectedInvItem
        {
            get => selectedInvItem;
            set => SetProperty(ref selectedInvItem, value);
        }

        public string AmmountDisplay
        {
            get => ammountDisplay;
            set => SetProperty(ref ammountDisplay, value);
        }
        async Task GetInventory()
        {
            InventoryItems.Clear();

            var invItems = await CookBookServer.GetInventoryItems();
            InventoryItems.AddRange(invItems.OrderBy(ii => ii.Ingredient.Category.Name));
        }

        public async Task OnPageAppearing()
        {
            await GetInventory();
        }

        async Task OnSelectIngredient(InventoryItem invItem)
        {
            if (invItem == null)
                return;

            SelectedInvItem = null;

            await Navigation.PushAsync(new InvIngredientPage());
            MessagingCenter.Send(this, "ItemSent", invItem);
        }

        async Task OnAddIngredient()
        {
            await Navigation.PushAsync(new AddInvIngredientPage());
        }
        async Task OnClearInventory()
        {

            bool clearConfirmed = await Application.Current.MainPage.
                DisplayAlert("Spájz kiüritése", "Biztos kiakarod üriteni a spájzodat?", "Igen", "Nem");
            
            if (clearConfirmed)
            {
                InventoryItems.Clear();
                await CookBookServer.ClearInventory();
            }
        }

    }
}