using CookbookApp.Models;
using CookbookApp.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CookbookApp.ViewModels
{
    class AddRecipeViewModel : BaseViewModel
    {
        private int recID;
        private string recipeName;
        private string recipeCategory;
        private string recipeImageSource;
        private int cookTime;
        private string btnText;

        private int currStep;

        public ObservableRangeCollection<RecipeIngredient> RecipeIngs { get; set; }
        public ObservableRangeCollection<RecipeStep> RecipeSteps { get; set; }

        public AsyncCommand UploadImgCommand { get; set; }
        public AsyncCommand AddRecipeCommand { get; set; }
        public AsyncCommand AddIngredientCommand { get; set; }
        public AsyncCommand RemIngredientCommand { get; set; }
        public AsyncCommand AddStepCommand { get; set; }
        public AsyncCommand RemStepCommand { get; set; }

        public AddRecipeViewModel()
        {
            currStep = 0;
            _ = GetID();

            RecipeSteps = new ObservableRangeCollection<RecipeStep>();
            RecipeIngs = new ObservableRangeCollection<RecipeIngredient>();

            UploadImgCommand = new AsyncCommand(OnUploadImage);
            AddRecipeCommand = new AsyncCommand(OnAddRecipe);
            AddIngredientCommand = new AsyncCommand(OnAddIngredient);
            RemIngredientCommand = new AsyncCommand(OnRemIng);
            AddStepCommand = new AsyncCommand(OnAddStep);
            RemStepCommand = new AsyncCommand(OnRemStep);
        }

        public string RecipeName
        {
            get => recipeName;
            set => SetProperty(ref recipeName, value);
        }
        public string RecipeCategory
        {
            get => recipeCategory;
            set => SetProperty(ref recipeCategory, value);
        }
        public string RecipeImageSource
        {
            get => recipeImageSource;
            set => SetProperty(ref recipeImageSource, value);
        }
        public int CookTime
        {
            get => cookTime;
            set => SetProperty(ref cookTime, value);
        }
        public string ButtonText
        {
            get => btnText;
            set => SetProperty(ref btnText, value);
        }
        // Created following Eduardo Rosas's blog: Selecting an Image from the Gallery
        async Task OnUploadImage()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("Nem támogatott", "Ez a funkció nem támogatott az eszközödön", "Ok");
                return;
            }

            var mediaOptions = new PickMediaOptions() { PhotoSize = PhotoSize.Full };
            var selectedImage = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (selectedImage == null)
            {
                await Application.Current.MainPage.DisplayAlert("HIBA", "Nem sikerült beolvasni a képet, kérjük próbáld meg újra", "Ok");
                return;
            }

            RecipeImageSource = selectedImage.Path;
        }

        async Task OnAddRecipe()
        {
            if (!await canAddRecipe())
                return;

            await CookBookServer.AddRecipe(RecipeName, recID, RecipeCategory, RecipeImageSource, CookTime);

            foreach (var ing in RecipeIngs)
            {
                await CookBookServer.AddRecipeIng(recID, ing.Ingredient, ing.Ammount);
            }

            foreach (var step in RecipeSteps)
            {
                await CookBookServer.AddRecipeStep(recID, step.Instruction);
            }

            await ResetPage();
        }

        async Task OnAddIngredient()
        {
            var ing = await CookBookServer.ChooseIngredient();
            if (ing != null)
            {
                if (ing.Name == "Err")
                {
                    await Task.Delay(100); // Delay to fix problem regarding awaitng 2 DisplayPrompts after each other
                    await OnAddIngredient();
                }
                else
                {
                    await AddIng(ing, "Szükséges mennyiség");
                    await Task.Delay(100); // Delay to fix problem regarding awaitng 2 DisplayPrompts after each other
                    await OnAddIngredient();
                }
            }
        }

        async Task OnRemIng()
        {
            if (RecipeIngs.Count == 0)
            {
                return;
            }
            RecipeIngs.RemoveAt(RecipeSteps.Count - 1);
        }

        async Task OnAddStep()
        {
            var instruction = await Application.Current.MainPage.DisplayPromptAsync("Lépés felvétel",
                                                                         "Következő lépés",
                                                                         "Ok",
                                                                         "Mégse",
                                                                         "Lépés...",
                                                                         keyboard: Keyboard.Text);

            if (instruction != null)
            {
                addStep(instruction);
                await Task.Delay(100); // Delay to fix problem regarding awaitng 2 DisplayPrompts after each other
                await OnAddStep();
            }
        }

        async Task OnRemStep()
        {
            if (RecipeSteps.Count == 0)
            {
                return;
            }
            RecipeSteps.RemoveAt(RecipeSteps.Count - 1);
        }
        async Task GetID()
        {
            recID = await CookBookServer.GetLastRecipeIndex();

            if (recID != (await CookBookServer.GetRecipes()).Count())
                recID = await CookBookServer.GetMissingRecipeIndex(recID - (await CookBookServer.GetRecipes()).Count());
            else
                recID++;

            ButtonText = string.Format("Recept #{0} felvétele", recID);
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

            if (RecipeIngs.Where(ri => ri.Ingredient.Name == ing.Name).Count() == 0)
            {
                var recipeIng = new RecipeIngredient()
                {
                    RecID = recID,
                    Ingredient = ing,
                    Ammount = ammount
                };
                RecipeIngs.Add(recipeIng);
            }
            else
                RecipeIngs.Where(ri => ri.Ingredient.Name == ing.Name)
                          .ForEach(ri => ri.Ammount += ammount);
        }

        async Task<float> GetAmmount(string ingName, string dispText)
        {
            await Task.Delay(100); // Delay to fix problem regarding awaitng 2 DisplayPrompts after each other

            string ammountquerry = await Application.Current.MainPage.DisplayPromptAsync(ingName,
                                                                                         dispText,
                                                                                         "Ok",
                                                                                         "Mégse",
                                                                                         "0",
                                                                                         keyboard: Keyboard.Numeric);
            if (ammountquerry != null && ammountquerry.Trim() != "")
                return Math.Abs(float.Parse(ammountquerry));
            
            return -1;
        }

        async Task ResetPage()
        {
            RecipeName = "";
            RecipeCategory = "";
            RecipeImageSource = "";
            CookTime = 0;
            currStep = 0;
            RecipeIngs.Clear();
            RecipeSteps.Clear();

            await GetID();
        }

        async Task<bool> canAddRecipe()
        {
            if (RecipeName == null || RecipeName.Trim() == "")
            {
                await Application.Current.MainPage.DisplayAlert("HIBA", "Nincs megadva a recept neve!", "Ok");
                return false;
            }
            //TODO No reference exception
            else if (RecipeCategory == null || RecipeCategory.Trim() == "")
            {
                await Application.Current.MainPage.DisplayAlert("HIBA", "Nincs megadva a recept kategóriája!", "Ok");
                return false;
            }
            else if (CookTime == 0)
            {
                await Application.Current.MainPage.DisplayAlert("HIBA", "Nem lehet 0 perc alatt elkészíteni egy receptet!", "Ok");
                return false;
            }
            else if (RecipeIngs.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("HIBA", "Nem lehet hozzávalók nélkül elkészíteni egy receptet!", "Ok");
                return false;
            }
            else if (currStep == 0)
            {
                await Application.Current.MainPage.DisplayAlert("HIBA", "Nincsnenek megadva utasítások a recpethez!", "Ok");
                return false;
            }

            return true;
        }
        public async Task OnPageAppearing()
        {
            await GetID();
        }

        void addStep(string instruction)
        {
            currStep++;

            var recipeStep = new RecipeStep()
            {
                RecID = (int)recID,
                Step = currStep,
                Instruction = instruction
            };

            RecipeSteps.Add(recipeStep);
        }
    }
}
