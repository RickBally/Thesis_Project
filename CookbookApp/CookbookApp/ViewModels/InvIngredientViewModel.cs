using CookbookApp.Models;
using CookbookApp.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CookbookApp.ViewModels
{
    class InvIngredientViewModel : BaseViewModel
    {
        InventoryItem invItem;

        public InventoryItem InventoryItem
        {
            get => invItem;
            set => SetProperty(ref invItem, value);
        }

        public AsyncCommand AddAmmountCommand { get; }
        public AsyncCommand RemAmmountCommand { get; }
        public AsyncCommand DeleteCommand { get; }

        public InvIngredientViewModel()
        {
            Title = "Hozzávaló információi";

            AddAmmountCommand = new AsyncCommand(OnAddAmmount);
            RemAmmountCommand = new AsyncCommand(OnRemAmmount);
            DeleteCommand = new AsyncCommand(OnRemoveIngredient);

            MessagingCenter.Subscribe<InventoryViewModel, InventoryItem>
                (this, "ItemSent", (sender, arg) =>
            {
                InventoryItem = arg;
            });
        }

        async Task OnAddAmmount()
        {
            string addedAmount = await Application.Current.MainPage.DisplayPromptAsync("Hozzáadás",
                                                                                     "Hozzáadott mennyiség",
                                                                                     "Ok",
                                                                                     "Mégse",
                                                                                     "0",
                                                                                     keyboard: Keyboard.Numeric);
            if (addedAmount != null && !addedAmount.Equals(""))
                await CookBookServer.UpdateInventoryAmmount(InventoryItem.IngName, Math.Abs(float.Parse(addedAmount)));
        }
        async Task OnRemAmmount()
        {
            string removedquery = await Application.Current.MainPage.DisplayPromptAsync("Eltávolítás",
                                                                                     "Eltávolított mennyiség",
                                                                                     "Ok",
                                                                                     "Mégse",
                                                                                     "0",
                                                                                     keyboard: Keyboard.Numeric);
            if (removedquery == null || removedquery.Equals(""))
                return;

            float removedAmount = Math.Abs(float.Parse(removedquery));
            
            if (removedAmount < InventoryItem.Ammount)
                await CookBookServer.UpdateInventoryAmmount(InventoryItem.IngName, removedAmount * -1);
            else
                await OnRemoveIngredient();
        }
        async Task OnRemoveIngredient()
        {

            bool remConfirmed = await Application.Current.MainPage.
                DisplayAlert("Hozzávaló eltávolítása", "Biztos elakarod távolítani ezt a hozzávalót a spájzól?", "Igen", "Nem");

            if (remConfirmed)
            {
                await CookBookServer.RemoveFromInventory(InventoryItem.IngName);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }
    }
}
