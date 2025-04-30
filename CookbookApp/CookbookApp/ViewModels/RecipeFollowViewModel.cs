using CookbookApp.Models;
using CookbookApp.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CookbookApp.ViewModels
{
    internal class RecipeFollowViewModel : BaseViewModel
    {
        public ObservableRangeCollection<RecipeStep> RecipeSteps { get; set; }

        public RecipeFollowViewModel()
        {
            RecipeSteps = new ObservableRangeCollection<RecipeStep>();

            MessagingCenter.Subscribe<RecipeDetailViewModel, int>
                (this, "RecipeIDSent", async (sender, arg) =>
                {
                    var rec = await CookBookServer.GetRecipe(arg);
                    Title = rec.Name;
                    var steps = await CookBookServer.GetRecipeSteps(arg);
                    RecipeSteps.AddRange(steps);
                });
        }
    }
}
