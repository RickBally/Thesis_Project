using CookbookApp.Models;
using CookbookApp.Services;
using CookbookApp.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CookbookApp.ViewModels
{
    internal class RecipesViewModel : BaseViewModel
    {
        ObservableRangeCollection<Recipe> Recipes { get; set; }
        protected List<RecipeIngredient> ingredients;
        protected List<Recipe> filteredRecipes;

        public ObservableRangeCollection<Recipe> DisplayedRecipes { get; set; }
        public AsyncCommand<Recipe> SelectCommand { get; }
        public AsyncCommand<Recipe> FavoriteCommand { get; }
        public AsyncCommand SearchCommand { get; }
        public AsyncCommand FilterCommand { get; }
        public INavigation Navigation { get; set; }

        Recipe selectedRecipe;
        bool pageCreated;
        public RecipesViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "Összes recept";
            filteredRecipes = new List<Recipe>();
            ingredients = new List<RecipeIngredient>();
            Recipes = new ObservableRangeCollection<Recipe>();
            DisplayedRecipes = new ObservableRangeCollection<Recipe>();

            SelectCommand = new AsyncCommand<Recipe>(OnSelect);
            FavoriteCommand = new AsyncCommand<Recipe>(OnFavorited);
            SearchCommand = new AsyncCommand(OnSearch);
            FilterCommand = new AsyncCommand(OnFilter);
        }

        public Recipe SelectedRecipe
        {
            get => selectedRecipe;
            set => SetProperty(ref selectedRecipe, value);
        }
        #region Commands
        async Task OnSelect(Recipe recipe)
        {
            if (recipe == null)
                return;

            SelectedRecipe = null;

            await Navigation.PushAsync(new RecipeDetailPage());
            MessagingCenter.Send(this, "RecipeSent", recipe);
        }

        async Task OnFavorited(Recipe recipe)
        {
            await CookBookServer.UpdateFavorite(recipe.ID);
        }
        public virtual async Task OnSearch()
        {
            var querry = await Application.Current.MainPage.DisplayPromptAsync("Recept keresése",
                                                                               "Adja meg egy recept nevét, vagy azonosítóját!",
                                                                               "Ok",
                                                                               "Mégse",
                                                                               "Recept neve vagy azonosítója...",
                                                                               keyboard: Keyboard.Text);

            if (querry == null) return;
            else
            {
                int id;
                bool isID = int.TryParse(querry, out id);
                if (isID)
                    await SearchID(id);
                else
                    await SearchName(querry);
            }
            GetDisplayedRecipes(filteredRecipes);
        }

        public virtual async Task OnFilter()
        {
            string[] filterOptions = new string[] { "Kategória szerint",
                                                    "Elkészíthető most",
                                                    "Tartalmaz...",
                                                    "Tartalmaz több...",
                                                    "Nem tartalmaz...",
                                                    "Nem tartalmaz több...",
                                                    "Elkészíthető X perc alatt",
                                                    "Lepj meg",
                                                    "Összes"};

            var filter = await Application.Current.MainPage.DisplayActionSheet("Receptek szűrése",
                                                                               "Mégse",
                                                                               null,
                                                                               filterOptions);

            switch (filter)
            {
                case "Kategória szerint":
                    await FilterCategory();
                    break;
                case "Elkészíthető most":
                    await FilterInventory();
                    break;
                case "Tartalmaz...":
                    await FilterContains();
                    break;
                case "Tartalmaz több...":
                    await FilterContainsMulti();
                    break;
                case "Nem tartalmaz...":
                    await FilterDoesntContain();
                    break;
                case "Nem tartalmaz több...":
                    await FilterDoesntContainsMulti();
                    break;
                case "Elkészíthető X perc alatt":
                    await FilterPrepTime();
                    break;
                case "Lepj meg":
                    await FilterRandom();
                    break;
                case "Összes":
                    await GetRecipes();
                    break;
            }
            if (filter == null || filter.Equals("Mégse"))
                return;

            if (!filter.Equals("Összes"))
                GetDisplayedRecipes(filteredRecipes);
        }
        #endregion

        public virtual async Task GetRecipes()
        {
            Title = "Összes recept";
            GetDisplayedRecipes(Recipes);

            ingredients.Clear();
            ingredients.AddRange(await CookBookServer.GetAllRecipeIngs());
        }

        public async Task OnPageAppearing()
        {
            if (!pageCreated)
            {
                await DataBaseFiller.LoadAll();
                pageCreated = true;
            }

            Recipes.Clear();
            Recipes.AddRange(await CookBookServer.GetRecipes());

            await GetRecipes();
        }
        #region Searches
        async Task SearchID(int id)
        {
            if (await CookBookServer.RecipeExists(id))
            {
                Title = "Keresett recept azonosító: " + id;
                filteredRecipes.Clear();
                filteredRecipes.Add(await CookBookServer.GetRecipe(id));
            }
            else
                await Application.Current.MainPage.DisplayAlert("Recept keresése",
                                                                "HIBA! A keresett recept nem található",
                                                                "Ok");
        }
        public async virtual Task SearchName(string name)
        {
            var recipes = await CookBookServer.GetRecipesByName(name);

            if (recipes.Count() == 0)
                await Application.Current.MainPage.DisplayAlert("Recept keresése",
                                                                "HIBA! Nem található recept ezzel a névvel",
                                                                "Ok");
            else
            {
                Title = string.Format("Keresett recept(ek): '{0}'", name);
                filteredRecipes.Clear();
                filteredRecipes.AddRange(recipes);
            }
        }
        #endregion

        #region Filters
        async Task FilterCategory()
        {
            string category = await Application.Current.MainPage.DisplayPromptAsync("Kategória",
                                                                                    "Adja meg a kategóriát amibe tartozik a keresett recept!",
                                                                                    "Ok",
                                                                                    "Mégse",
                                                                                    "Recept kategóriája",
                                                                                    keyboard: Keyboard.Text);
            if (category != null)
            {
                var recipes = await CookBookServer.GetRecipesFromCategory(category);
                if (recipes.Count() == 0)
                    await Application.Current.MainPage.DisplayAlert("Kategória",
                                                                    "HIBA! Nem található recept ebben a kategóriában!",
                                                                    "Ok");
                else
                {
                    Title = "Kategória: " + category;
                    filteredRecipes.Clear();
                    filteredRecipes.AddRange(recipes);
                }
            }
        }
        async Task FilterInventory()
        {
            await GetRecipes();
            List<Recipe> prepareableRecipes = Recipes.ToList();

            foreach (RecipeIngredient ing in ingredients)
            {
                if (!await CookBookServer.InventoryContains(ing.Ingredient.Name))
                {
                    prepareableRecipes.RemoveAll(x => x.ID == ing.RecID);
                }
                else if ((await CookBookServer.GetInventoryItem(ing.Ingredient.Name)).Ammount < ing.Ammount)
                {
                    prepareableRecipes.RemoveAll(x => x.ID == ing.RecID);
                }
            }

            if (prepareableRecipes.Count() > 0)
            {
                Title = "Elkészíthető most";
                filteredRecipes.Clear();
                filteredRecipes.AddRange(prepareableRecipes);
            }
            else
                await Application.Current.MainPage.DisplayAlert("Elkészíthető most",
                                                                "HIBA! Jelenleg egy recept sem készíthető el!",
                                                                "Ok");
        }
        async Task FilterContains()
        {
            Ingredient searchedIng = await CookBookServer.ChooseIngredient("Tartalmaz...");

            if (searchedIng == null || searchedIng.Name.Equals("Err"))
                return;

            List<Recipe> matches = (await CookBookServer.GetRecipesContaining(searchedIng)).ToList();

            await DisplayContain(matches, new List<string> { searchedIng.Name });
        }

        async Task FilterContainsMulti()
        {
            List<Recipe> matches = new List<Recipe>();
            List<string> searchIngNames = new List<string>();

            while (true)
            {
                Ingredient searchedIng = await CookBookServer.ChooseIngredient("Tartalmaz több...", string.Join(", ", searchIngNames));

                if (searchedIng == null)
                    break;

                searchIngNames.Add(searchedIng.Name);
                matches.AddRange((await CookBookServer.GetRecipesContaining(searchedIng)).ToList());
            }

            await DisplayContain(GetMatchRecipesIntersect(matches), searchIngNames);
        }

        async Task FilterDoesntContain()
        {
            Ingredient searchedIng = await CookBookServer.ChooseIngredient("Nem tartalmaz...");

            if (searchedIng == null || searchedIng.Name.Equals("Err"))
                return;

            List<Recipe> matches = (await CookBookServer.GetRecipesNotContaining(searchedIng)).ToList();

            await DisplayContain(matches, new List<string> { searchedIng.Name });
        }
        async Task FilterDoesntContainsMulti()
        {
            List<Recipe> matches = new List<Recipe>();
            List<string> searchIngNames = new List<string>();

            while (true)
            {
                Ingredient searchedIng = await CookBookServer.ChooseIngredient("Nem tartalmaz több...", string.Join(", ", searchIngNames));

                if (searchedIng == null)
                    break;

                searchIngNames.Add(searchedIng.Name);
                matches.AddRange((await CookBookServer.GetRecipesNotContaining(searchedIng)).ToList());
            }
            
            await DisplayContain(GetMatchRecipesIntersect(matches), searchIngNames);
        }
        async Task FilterPrepTime()
        {
            string prepQuerry = await Application.Current.MainPage.DisplayPromptAsync("Elkészítés idő",
                                                                                "Adja meg mennyi idő allat készíthető el a keresett recept!",
                                                                                "Ok",
                                                                                "Mégse",
                                                                                "Elkészítés ideje (percben)",
                                                                                keyboard: Keyboard.Numeric);
            int preptime;
            bool shouldFilter = int.TryParse(prepQuerry, out preptime);
            if (shouldFilter)
            {
                var recipes = await CookBookServer.GetRecipesBelowTime(preptime);
                if (recipes.Count() == 0)
                    await Application.Current.MainPage.DisplayAlert("Elkészítés idő",
                                                                    "HIBA! Nem található recept amit ennyi idő alat el lehet készíteni",
                                                                    "Ok");
                else
                {
                    Title = "Max: " + preptime + " perc";
                    filteredRecipes.Clear();
                    filteredRecipes.AddRange(recipes);
                }
            }
        }
        public virtual async Task FilterRandom()
        {
            if (Recipes.Count < 3)
                await Application.Current.MainPage.DisplayAlert("Lepj meg",
                                                                "HIBA! Nincs elég recept a rendszerben, legalább 3-nak kell lennie, hogy ezt a funkciót használd",
                                                                "Ok");
            else
            {
                Title = "3 Véletlen recept";
                filteredRecipes.Clear();
                filteredRecipes.AddRange(await GetRandomRecipes());
            }
        }
        #endregion
        protected void GetDisplayedRecipes(IEnumerable<Recipe> recipes)
        {
            if (recipes.Count() <= 0)
                return;
            DisplayedRecipes.Clear();
            DisplayedRecipes.AddRange(recipes.OrderBy(r => r.ID));
        }

        async Task DisplayContain(List<Recipe> matches, List<string> ings)
        {
            if (matches.Count > 0)
            {
                filteredRecipes.Clear();
                Title = GetContainsTitle(ings);
                filteredRecipes.AddRange(matches);
            }
            else
                await Application.Current.MainPage.DisplayAlert("Tartalmaz...",
                                                                "HIBA!, nem található recept ezzel a hozzávalóval",
                                                                "Ok");
        }

        protected async Task<ObservableRangeCollection<Recipe>> GetRandomRecipes()
        {
            var recipes = new ObservableRangeCollection<Recipe>();
            List<int> randIDs = new List<int>();
            for (int i = 0; i < 3; i++)
                await GetRandomID(randIDs);

            foreach (int id in randIDs)
                recipes.Add(await CookBookServer.GetRecipe(id));

            return recipes;
        }
        public virtual async Task GetRandomID(List<int> ids)
        {
            Random rand = new Random();
            int randNum = rand.Next(1, await CookBookServer.GetLastRecipeIndex() + 1);
            if (!await CookBookServer.RecipeExists(randNum) || ids.Contains(randNum))
            {
                await GetRandomID(ids);
                return;
            }
            else
                ids.Add(randNum);
        }

        string GetContainsTitle(List<string> ingNames)
        {
            if (ingNames.Count() == 1)
                return "Tartalmaz: " + ingNames.ElementAt(0);
            return "Tartalmaz több...";
        }

        List<Recipe> GetMatchRecipesIntersect(List<Recipe> mr)
        {
            var matchesgrouped = mr.GroupBy(r => r).ToList();
            int max = matchesgrouped.Max(r => r.Count());

            return matchesgrouped.Where(r => r.Count() == max).Select(r => r.Key).ToList();
        }
    } 
}