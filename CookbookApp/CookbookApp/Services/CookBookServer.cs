using CookbookApp.Models;
using Realms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static Xamarin.Forms.Internals.GIFBitmap;
namespace CookbookApp.Services
{
    public static class CookBookServer
    {
        static Realm realm;
        static async Task Init()
        {
            RealmConfiguration config = new RealmConfiguration
            {
                ShouldDeleteIfMigrationNeeded = true
            };

            if (realm != null)
                return;

            realm = Realm.GetInstance(config);
        }
        public static async Task Refresh()
        {
            await Init();

            await realm.RefreshAsync();
        }
        #region Ingredient Category CRUD
        public static async Task AddIngCategory(string name, string allergen, string color)
        {
            await Init();

            var category = new IngCategory
            {
                Name = name,
                Allergen = allergen,
                BGColor = color
            };

            await realm.WriteAsync(() => { realm.Add(category); });
            await Refresh();
        }

        public static async Task<IngCategory> GetIngCategory(string name)
        {
            await Init();
            return realm.All<IngCategory>().Where(i => i.Name == name).FirstOrDefault();
        }

        public static async Task<IQueryable<IngCategory>> GetIngCategories()
        {
            await Init();

            return realm.All<IngCategory>();
        }

        public static async Task ClearIngCategories()
        {
            await Init();

            await realm.WriteAsync(() => { realm.RemoveAll<IngCategory>(); });

            await Refresh();
        }
        #endregion

        #region Ingredient CRUD
        public static async Task AddIngredient(string name, IngCategory category, string imgSrc, string unit)
        {
            await Init();

            var ingredient = new Ingredient
            {
                Name = name,
                Category = category,
                ImageSource = imgSrc,
                Unit = unit
            };

            await realm.WriteAsync(() => { realm.Add(ingredient); });
            await Refresh();
        }

        public static async Task<bool> IngredientsExists(string name)
        {
            await Init();

            var item = realm.Find<Ingredient>(name);
            return item != null;
        }

        public static async Task<Ingredient> GetIngredient(string name)
        {
            await Init();
            return realm.All<Ingredient>().Where(i => i.Name == name).FirstOrDefault();
        }

        public static async Task<IQueryable<Ingredient>> GetIngFromCategory(string categoryName)
        {
            await Init();
            var category = await GetIngCategory(categoryName);
            return realm.All<Ingredient>().Where(i => i.Category == category);
        }

        public static async Task<IQueryable<Ingredient>> GetIngredients()
        {
            await Init();

            return realm.All<Ingredient>();
        }

        public static async Task<string[]> GetIngredientNames(string subname)
        {
            await Init();

            var ingredients = realm.All<Ingredient>().ToList().Where(i => i.Name.IndexOf(subname, StringComparison.OrdinalIgnoreCase) != -1);

            return ingredients.Select(i => i.Name).ToArray();
        }

        public static async Task ClearIngredients()
        {
            await Init();

            await realm.WriteAsync(() => { realm.RemoveAll<Ingredient>(); });

            await Refresh();
        }
        #endregion
        
        #region Inventory CRUD
        public static async Task AddToInventory(string name, float ammount)
        {
            await Init();

            var ingredient = await GetIngredient(name);

            var invItem = new InventoryItem
            {
                IngName = name,
                Ingredient = ingredient,
                Ammount = ammount,
                AmmountDisplay = ammount + ingredient.Unit
                
            };

            await realm.WriteAsync(() => { realm.Add(invItem); });
            await Refresh();
        }

        public static async Task<bool> InventoryContains(string name)
        {
            await Init();

            var item = realm.Find<InventoryItem>(name);            
            return item != null;
        }

        public static async Task<InventoryItem> GetInventoryItem(string name)
        {
            await Init();
            var invItem = realm.All<InventoryItem>().ToList().Where(i => i.IngName == name).FirstOrDefault();

            return invItem;
        }

        public static async Task<IQueryable<InventoryItem>> GetInventoryItems()
        {
            await Init();

            var invItems = realm.All<InventoryItem>();
            return invItems;
        }

        public static async Task UpdateInventoryAmmount(string name, float ammount)
        {
            await Init();
            InventoryItem invItem = await GetInventoryItem(name);
            await realm.WriteAsync(() => {
                invItem.Ammount += ammount;
                invItem.AmmountDisplay = invItem.Ammount + invItem.Ingredient.Unit;
                realm.Add(invItem, update: true);
            });
            if (invItem.Ammount <= 0)
            {
                await RemoveFromInventory(name);
            }

            await Refresh();
        }
        
        public static async Task RemoveFromInventory(string name)
        {
            await Init();

            var invItem = await GetInventoryItem(name);
            await realm.WriteAsync(() => { realm.Remove(invItem); });

            await Refresh();
        }
        public static async Task ClearInventory()
        {
            await Init();

            await realm.WriteAsync(() => { realm.RemoveAll<InventoryItem>(); });

            await Refresh();
        }
        #endregion        
        
        #region Recipe CRUD
        public static async Task AddRecipe(string name, int id, string category, string imageSource, int cookTime)
        {
            await Init();

            var recipe = new Recipe
            {
                ID = (int)id,
                Name = name,
                Category = category,
                ImageSource = imageSource,
                CookTime = cookTime + " perc",
                Favorited = false,
                FavoriteIcon = "button_star_empty"
            };

            await realm.WriteAsync(() => { realm.Add(recipe); });
            await Refresh();
        }

        public static async Task<int> GetLastRecipeIndex()
        {
            await Init();

            var rec = realm.All<Recipe>().OrderBy(recipe => recipe.ID).LastOrDefault();
            return rec != null ? rec.ID : 0;
        }

        public static async Task<int> GetMissingRecipeIndex(int counter)
        {
            await Init();
            var rec = await GetRecipes();
            List<int> missingIDs = new List<int>();

            int min = 0;
            int max = rec.Count() - 1;

            while (min <= max && counter != 0)
            {
                int avg = Average(min, max);
                if (!await RecipeExists(avg + 1) && !missingIDs.Contains(avg + 1))
                {
                    counter--;
                    min = 0;
                    max = rec.Count() - 1;
                    missingIDs.Add(avg + 1);
                }
                
                else if (rec.ToList()
                            .Skip(min)
                            .Take(max - min + 1)
                            .Count(r => r.ID >= avg + 2) <= max - avg)
                    min = avg + 1;
                else
                    max = avg - 1;
            }

            if (missingIDs.Count() != 0)
                return missingIDs.OrderBy(n => n).First();
            return await GetLastRecipeIndex();
        }

        public static async Task<bool> RecipeExists(int id)
        {
            await Init();

            var recipe = realm.Find<Recipe>(id);

            return recipe != null;
        }

        public static async Task<IQueryable<Recipe>> GetRecipes()
        {
            await Init();

            var recipes = realm.All<Recipe>();
            return recipes.OrderBy(recipe => recipe.ID);
        }

        public static async Task<Recipe> GetRecipe(int id)
        {
            await Init();

            var recipe = realm.All<Recipe>().Where(i => i.ID == id).FirstOrDefault();
            return recipe;
        }

        public static async Task<IEnumerable<Recipe>> GetRecipesByName(string name)
        {
            await Init();

            var recipes = realm.All<Recipe>().ToList().Where(i => i.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1);
            return recipes;
        }

        public static async Task<IEnumerable<Recipe>> GetRecipesFromCategory(string category)
        {
            await Init();

            var recipes = realm.All<Recipe>().ToList().Where(i => i.Category.IndexOf(category, StringComparison.OrdinalIgnoreCase) != -1);
            return recipes;
        }
        public static async Task<IEnumerable<Recipe>> GetPreparebaleRecipes(Ingredient ing)
        {
            await Init();

            var rIDs = await GetRIDsContaining(ing);
            return realm.All<Recipe>().ToList().Where(r => rIDs.ToList().Contains(r.ID));
        }
        public static async Task<IEnumerable<Recipe>> GetRecipesBelowTime(int minutes)
        {
            await Init();

            var recipes = realm.All<Recipe>().ToList().Where(i => int.Parse(i.CookTime.Split(' ')[0]) <= minutes);
            return recipes;
        }
        public static async Task<IEnumerable<int>> GetPreparableRIDs()
        {
            await Init();

            var recipeings = realm.All<RecipeIngredient>().Where(ri => ri.Ammount == 1);
            return recipeings.ToList().Select(ri => ri.RecID);
        }
        public static async Task<IEnumerable<int>> GetRIDsContaining(Ingredient ing)
        {
            await Init();

            var recipeings = realm.All<RecipeIngredient>().Where(ri => ri.Ingredient == ing);
            return recipeings.ToList().Select(ri => ri.RecID);
        }
        public static async Task<IEnumerable<Recipe>> GetRecipesContaining(Ingredient ing)
        {
            await Init();

            var rIDs = await GetRIDsContaining(ing);
            return realm.All<Recipe>().ToList().Where(r => rIDs.ToList().Contains(r.ID));
        }
        public static async Task<IEnumerable<Recipe>> GetRecipesNotContaining(Ingredient ing)
        {
            await Init();

            var rIDs = await GetRIDsContaining(ing);
            return realm.All<Recipe>().ToList().Where(r => !rIDs.ToList().Contains(r.ID));
        }

        public static async Task<IQueryable<Recipe>> GetFavoriteRecipes()
        {
            await Init();
            var favRecipes = realm.All<Recipe>().Where(i => i.Favorited);
            return favRecipes;
        }

        public static async Task UpdateFavorite(int id)
        {
            await Init();
            Recipe recipe = await GetRecipe(id);
            await realm.WriteAsync(() => { recipe.Favorited = !recipe.Favorited; getFavoriteIcon(recipe); realm.Add(recipe, update: true); });
            await Refresh();
        }
        public static async Task RemoveRecipe(int id)
        {
            await Init();

            var recipe = await GetRecipe(id);
            await realm.WriteAsync(() => { realm.Remove(recipe); });

            await Refresh();
        }

        public static async Task ClearRecipes()
        {
            await Init();

            await realm.WriteAsync(() => { realm.RemoveAll<Recipe>(); });

            await Refresh();
        }

        static void getFavoriteIcon(Recipe recipe)
        {
            if (recipe.Favorited) recipe.FavoriteIcon = "button_star_full";
            else recipe.FavoriteIcon = "button_star_empty";
        }
        #endregion

        #region RecipeSteps CRUD
        public static async Task AddRecipeStep(int recID, string instruction)
        {
            await Init();

            var prevStep = (await GetRecipeSteps(recID)).LastOrDefault();
            var step = prevStep != null ? prevStep.Step + 1 : 1;

            var recipeStep = new RecipeStep
            {
                RecID = recID,
                Step = step,
                Instruction = instruction
            };

            await realm.WriteAsync(() => { realm.Add(recipeStep); });
            await Refresh();
        }

        public static async Task<IQueryable<RecipeStep>> GetRecipeSteps(int id)
        {
            await Init();
            return realm.All<RecipeStep>().Where(i => i.RecID == id);
        }

        public static async Task RemoveRecipeSteps(int id)
        {
            await Init();
            await realm.WriteAsync(async () => { realm.RemoveRange(await GetRecipeSteps(id)); });
            await Refresh();
        }

        public static async Task ClearRecipeSteps()
        {
            await Init();

            await realm.WriteAsync(() => { realm.RemoveAll<RecipeStep>(); });

            await Refresh();
        }
        #endregion

        #region RecipeIngredients CRUD
        public static async Task AddRecipeIng(int recID, Ingredient ingredient, float ammount)
        {
            await Init();

            var rec = realm.All<RecipeIngredient>().LastOrDefault();
            int id = rec != null ? rec.ID + 1 : 1;

            var recipeIng = new RecipeIngredient
            {
                ID = id,
                RecID = recID,
                Ingredient = ingredient,
                Ammount = ammount
            };

            await realm.WriteAsync(() => { realm.Add(recipeIng); });
            await Refresh();
        }

        public static async Task<IQueryable<RecipeIngredient>> GetAllRecipeIngs()
        {
            await Init();
            return realm.All<RecipeIngredient>();
        }

        public static async Task<IQueryable<RecipeIngredient>> GetRecipeIngs(int id)
        {
            await Init();
            return realm.All<RecipeIngredient>().Where(i => i.RecID == id);
        }

        public static async Task RemoveRecipeIngs(int id)
        {
            await Init();
            await realm.WriteAsync(async () => { realm.RemoveRange(await GetRecipeIngs(id)); });
            await Refresh();
        }

        public static async Task ClearRecipeIngs()
        {
            await Init();

            await realm.WriteAsync(() => { realm.RemoveAll<RecipeIngredient>(); });

            await Refresh();
        }
        #endregion

        #region ShoppingList CRUD
        public static async Task AddShoppingItem(string name, float ammount)
        {
            await Init();
            var ing = await GetIngredient(name);
            var item = new ShoppingItem
            {
                IngName = name,
                Ammount = ammount,
                ShoppingIng = ing,
                Selected = false,
                SelectIcon = "button_unselected"
            };
            await realm.WriteAsync(() => { realm.Add(item); });
            await Refresh();
        }

        public static async Task UpdateShoppingAmmount(string name, float ammount)
        {
            await Init();
            ShoppingItem shopItem = await GetShoppingItem(name);
            await realm.WriteAsync(() => {
                shopItem.Ammount += ammount;
                realm.Add(shopItem, update: true);
            });
            if (shopItem.Ammount <= 0)
            {
                await RemoveFromShoppingList(name);
            }
            await Refresh();
        }

        public static async Task<bool> ShoppingListContains(string name)
        {
            await Init();

            var item = realm.Find<ShoppingItem>(name);
            return item != null;
        }

        public static async Task<IQueryable<ShoppingItem>> GetShoppingItems()
        {
            await Init();

            return realm.All<ShoppingItem>();
        }

        public static async Task<ShoppingItem> GetShoppingItem(string name)
        {
            await Init();

            return realm.All<ShoppingItem>().Where(i => i.IngName == name).FirstOrDefault();
        }

        public static async Task<IEnumerable<ShoppingItem>> GetSelectedShoppingItems()
        {
            await Init();

            return realm.All<ShoppingItem>().Where(si => si.Selected);
        }

        public static async Task UpdateSelected(string name)
        {
            await Init();
            ShoppingItem item = await GetShoppingItem(name);
            await realm.WriteAsync(() => {
                item.Selected = !item.Selected;
                item.SelectIcon = item.Selected ? "button_selected" : "button_unselected";
                realm.Add(item, update: true);
            });
            await Refresh();
        }
        public static async Task RemoveFromShoppingList(string name)
        {
            await Init();

            await realm.WriteAsync(async () => { realm.Remove(await GetShoppingItem(name)); });

            await Refresh();
        }

        public static async Task ClearShoppingList()
        {
            await Init();

            await realm.WriteAsync(() => { realm.RemoveAll<ShoppingItem>(); });

            await Refresh();
        }
        #endregion

        public static async Task<Ingredient> ChooseIngredient(string title="Hozzávaló keresése", string body = "Hozzávaló...")
        {
            string ingQuerry = await Application.Current.MainPage.DisplayPromptAsync(title,
                                                                                     body,
                                                                                     "Oké",
                                                                                     "Mégse",
                                                                                     "Hozzávaló neve",
                                                                                     keyboard: Keyboard.Text);

            if (ingQuerry == null) return null;

            string[] ingChoices = await GetIngredientNames(ingQuerry.Trim());

            if (ingChoices.Count() == 0)
            {
                await Application.Current.MainPage.DisplayAlert("HIBA!", "Keresett hozzávaló nem található", "OK");
                return new Ingredient { Name = "Err"};
            }
            else if (ingChoices.Count() == 1)
            {
                return await GetIngredient(ingChoices.ElementAt(0));
            }

            string ingChoice = await Application.Current.MainPage.DisplayActionSheet("Hozzávaló kiválasztása",
                                                                                     "Vissza",
                                                                                     null,
                                                                                     ingChoices);

            if (ingChoice.Equals("Vissza"))
            {
                return await ChooseIngredient();
            }

            return await GetIngredient(ingChoice);
        }
        static int Average(int a, int b)
        {
            return (int)Math.Floor((a + b) / 2f);
        }
    }
}
