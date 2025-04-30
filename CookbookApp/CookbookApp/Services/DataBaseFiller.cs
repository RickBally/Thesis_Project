using CookbookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CookbookApp.Services
{
    public static class DataBaseFiller
    {
        public static bool forceReload;
        public static async Task LoadAll()
        {
            forceReload = false;
            await LoadIngredients();
        }

        public static async Task LoadCategories()
        {
            var categories = await CookBookServer.GetIngCategories();

            if (categories.Count() == 10 && !forceReload)
                return;
            await CookBookServer.ClearIngCategories();

            await CookBookServer.AddIngCategory("Zöldség", "Nincsen", "#90ee90");
            await CookBookServer.AddIngCategory("Gyümölcs", "Nincsen", "#ff6347");
            await CookBookServer.AddIngCategory("Gabona termék", "Glutén", "#f5deb3");
            await CookBookServer.AddIngCategory("Húsféle", "Nincsen", "#ffc0cb");
            await CookBookServer.AddIngCategory("Tojás", "Tojás", "#deb887");
            await CookBookServer.AddIngCategory("Tejtermék", "Tejérzékenység", "#ffffff");
            await CookBookServer.AddIngCategory("Fűszer", "Nincs", "#dda0dd");
            await CookBookServer.AddIngCategory("Alkohol", "Nincs", "#8b0000");
            await CookBookServer.AddIngCategory("Egyéb", "Nincs", "#6495ed");
            await CookBookServer.AddIngCategory("Minden", "Nincs", "#000000");
        }

        public static async Task LoadIngredients()
        {
            if ((await CookBookServer.GetIngredients()).Count() == 84 && !forceReload)
                return;
            await LoadCategories();

            await CookBookServer.ClearIngredients();
            #region Kategória lehívások
            var vegetable = await CookBookServer.GetIngCategory("Zöldség");
            var fruit = await CookBookServer.GetIngCategory("Gyümölcs");
            var wheatProduct = await CookBookServer.GetIngCategory("Gabona termék");
            var meat = await CookBookServer.GetIngCategory("Húsféle");
            var egg = await CookBookServer.GetIngCategory("Tojás");
            var milkProduct = await CookBookServer.GetIngCategory("Tejtermék");
            var spice = await CookBookServer.GetIngCategory("Fűszer");
            var alcohol = await CookBookServer.GetIngCategory("Alkohol");
            var misc = await CookBookServer.GetIngCategory("Egyéb");
            #endregion

            #region Zöldség (20)
            await CookBookServer.AddIngredient("Bab", vegetable, "ing_beans.png", "dkg");
            await CookBookServer.AddIngredient("Borsó", vegetable, "ing_peas.png", "dkg");
            await CookBookServer.AddIngredient("Brokkoli", vegetable, "ing_broccoli.png", "dkg");
            await CookBookServer.AddIngredient("Burgonya", vegetable, "ing_potato.png", "kg");
            await CookBookServer.AddIngredient("Cukkini", vegetable, "ing_zucchini.png", "dkg");
            await CookBookServer.AddIngredient("Csili paprika", vegetable, "ing_chili.png", "db");
            await CookBookServer.AddIngredient("Fokhagyma", vegetable, "ing_garlic.png", "gerezd");
            await CookBookServer.AddIngredient("Gomba", vegetable, "ing_mushroom.png", "dkg");
            await CookBookServer.AddIngredient("Hagyma", vegetable, "ing_onion.png", "db");
            await CookBookServer.AddIngredient("Káposzta", vegetable, "ing_cabbage.png", "dkg");
            await CookBookServer.AddIngredient("Karfiol", vegetable, "ing_cauliflower.png", "fej");
            await CookBookServer.AddIngredient("Kukorica", vegetable, "ing_corn.png", "dkg");
            await CookBookServer.AddIngredient("Paprika", vegetable, "ing_bell_pepper.png", "db");
            await CookBookServer.AddIngredient("Paradicsom", vegetable, "ing_tomato.png", "dkg");
            await CookBookServer.AddIngredient("Répa", vegetable, "ing_carrot.png", "db");
            await CookBookServer.AddIngredient("Saláta", vegetable, "ing_lettuce.png", "dkg");
            await CookBookServer.AddIngredient("Spenót", vegetable, "ing_spinach.png", "dkg");
            await CookBookServer.AddIngredient("Tök", vegetable, "ing_pumpkin.png", "dkg");
            await CookBookServer.AddIngredient("Uborka", vegetable, "ing_cucumber.png", "dkg");
            await CookBookServer.AddIngredient("Zeller", vegetable, "ing_celery.png", "dkg");
            #endregion

            #region Gyümölcs (12)
            await CookBookServer.AddIngredient("Alma", fruit, "ing_apple.png", "db");
            await CookBookServer.AddIngredient("Ananász", fruit, "ing_pineapple.png", "db");
            await CookBookServer.AddIngredient("Banán", fruit, "ing_banana.png", "db");
            await CookBookServer.AddIngredient("Citrom", fruit, "ing_lemon.png", "db");
            await CookBookServer.AddIngredient("Cseresznye", fruit, "ing_cherry.png", "dkg");
            await CookBookServer.AddIngredient("Dinnye", fruit, "ing_watermelon.png", "dkg");
            await CookBookServer.AddIngredient("Eper", fruit, "ing_strawberry.png", "dkg");
            await CookBookServer.AddIngredient("Körte", fruit, "ing_pear.png", "db");
            await CookBookServer.AddIngredient("Lájm", fruit, "ing_lime.png", "db");
            await CookBookServer.AddIngredient("Mangó", fruit, "ing_mango.png", "db");
            await CookBookServer.AddIngredient("Narancs", fruit, "ing_orange.png", "db");
            await CookBookServer.AddIngredient("Szőlő", fruit, "ing_grapes.png", "dkg");
            #endregion

            #region Gabona termék (7)
            await CookBookServer.AddIngredient("Kenyér", wheatProduct, "ing_bread.png", "kg");
            await CookBookServer.AddIngredient("Kifli", wheatProduct, "ing_baguette.png", "db");
            await CookBookServer.AddIngredient("Liszt", wheatProduct, "ing_flour.png", "dkg");
            await CookBookServer.AddIngredient("Rízs", wheatProduct, "ing_rice.png", "g");
            await CookBookServer.AddIngredient("Tészta", wheatProduct, "ing_pasta.png", "g");
            await CookBookServer.AddIngredient("Zsemle", wheatProduct, "ing_rolls.png", "db");
            await CookBookServer.AddIngredient("Zsemlemorzsa", wheatProduct, "ing_breadcrumbs.png", "dkg");
            #endregion

            #region Húsféle (7)
            await CookBookServer.AddIngredient("Csirkehús", meat, "ing_chicken.png", "kg");
            await CookBookServer.AddIngredient("Marhahús", meat, "ing_beef.png", "kg");
            await CookBookServer.AddIngredient("Sertéshús", meat, "ing_pork.png", "kg");
            await CookBookServer.AddIngredient("Sonka", meat, "ing_ham.png", "dkg");
            await CookBookServer.AddIngredient("Szalámi", meat, "ing_salami.png", "dkg");
            await CookBookServer.AddIngredient("Szalonna", meat, "ing_bacon.png", "dkg");
            await CookBookServer.AddIngredient("Virsli", meat, "ing_sausage.png", "db"); 
            #endregion

            await CookBookServer.AddIngredient("Tojás", egg, "ing_egg.png", "db");

            #region Tejtermék (7)
            await CookBookServer.AddIngredient("Joghurt", milkProduct, "ing_yoghurt.png", "g");
            await CookBookServer.AddIngredient("Sajt", milkProduct, "ing_cheese.png", "g");
            await CookBookServer.AddIngredient("Tej", milkProduct, "ing_milk.png", "dl");
            await CookBookServer.AddIngredient("Tejszín", milkProduct, "ing_cream.png", "dl");
            await CookBookServer.AddIngredient("Tejszinhab", milkProduct, "ing_whipped_cream.png", "g");
            await CookBookServer.AddIngredient("Túró", milkProduct, "ing_cottage_cheese.png", "dkg");
            await CookBookServer.AddIngredient("Vaj", milkProduct, "ing_butter.png", "g");
            #endregion

            #region Fűszer (12)
            await CookBookServer.AddIngredient("Babérlevél", spice, "ing_bay.png", "db");
            await CookBookServer.AddIngredient("Bazsalikom", spice, "ing_basil.png", "g");
            await CookBookServer.AddIngredient("Fahéj", spice, "ing_cinnamon.png", "g");
            await CookBookServer.AddIngredient("Fűszer paprika", spice, "ing_paprika.png", "g");
            await CookBookServer.AddIngredient("Gyömbér", spice, "ing_ginger.png", "g");
            await CookBookServer.AddIngredient("Koriander", spice, "ing_coriander.png", "g");
            await CookBookServer.AddIngredient("Kömény", spice, "ing_cumin.png", "g");
            await CookBookServer.AddIngredient("Kurkuma", spice, "ing_turmeric.png", "g");
            await CookBookServer.AddIngredient("Oregánó", spice, "ing_oregano.png", "g");
            await CookBookServer.AddIngredient("Petrezselyem", spice, "ing_parsley.png", "g");
            await CookBookServer.AddIngredient("Szerecsendió", spice, "ing_nutmeg.png", "g");
            await CookBookServer.AddIngredient("Vanília", spice, "ing_vanila.png", "g"); 
            #endregion

            await CookBookServer.AddIngredient("Bor", alcohol, "ing_wine.png", "dl");
            await CookBookServer.AddIngredient("Sör", alcohol, "ing_beer.png", "dl");

            #region Egyéb (16)
            await CookBookServer.AddIngredient("Bors", misc, "ing_pepper.png", "g");
            await CookBookServer.AddIngredient("Cukor", misc, "ing_sugar.png", "dkg");
            await CookBookServer.AddIngredient("Csokoládé", misc, "ing_chocolate.png", "dkg");
            await CookBookServer.AddIngredient("Ecet", misc, "ing_vinegar.png", "dl");
            await CookBookServer.AddIngredient("Étolaj", misc, "ing_oil.png", "dl");
            await CookBookServer.AddIngredient("Élesztő", misc, "ing_yeast.png", "g");
            await CookBookServer.AddIngredient("Húsleves alap", misc, "ing_stock.png", "dl");
            await CookBookServer.AddIngredient("Juhar szirup", misc, "ing_maple.png", "dl");
            await CookBookServer.AddIngredient("Kakaópor", misc, "ing_cocoa.png", "g");
            await CookBookServer.AddIngredient("Kávé", misc, "ing_coffee.png", "g");
            await CookBookServer.AddIngredient("Ketchup", misc, "ing_ketchup.png", "g");
            await CookBookServer.AddIngredient("Majonéz", misc, "ing_mayo.png", "g");
            await CookBookServer.AddIngredient("Méz", misc, "ing_honey.png", "g");
            await CookBookServer.AddIngredient("Mustár", misc, "ing_mustard.png", "g");
            await CookBookServer.AddIngredient("Só", misc, "ing_salt.png", "g");
            await CookBookServer.AddIngredient("Sütőpor", misc, "ing_baking_powder.png", "g");
            #endregion

            await LoadRecipes();
        }

        public static async Task LoadRecipes()
        {
            if ((await CookBookServer.GetRecipes()).Count() == 15 && !forceReload)
                return;

            await CookBookServer.ClearRecipes();
            await CookBookServer.ClearRecipeIngs();
            await CookBookServer.ClearRecipeSteps();

            await CookBookServer.AddRecipe("Svéd burgonya", 1, "Köret", "rec_potatoes.png", 65);
            await CookBookServer.AddRecipe("Lasagne", 2, "Egytálétel", "rec_lasagne.png", 175);
            await CookBookServer.AddRecipe("Spagetti ala Carbonara", 3, "Egytálétel", "rec_carbonara.png", 25);
            await CookBookServer.AddRecipe("Sajtos Pizza", 4, "Pizza", "rec_pizza.png", 70);
            await CookBookServer.AddRecipe("Karamellás Puding", 5, "Desszert", "rec_pudding.png", 60);
            await CookBookServer.AddRecipe("Csokitorta", 6, "Desszert", "rec_cake.png", 100);
            await CookBookServer.AddRecipe("Hamburger", 7, "Húsétel", "rec_burger.png", 80);
            await CookBookServer.AddRecipe("Gofri", 8, "Desszert", "rec_waffles.png", 40);
            await CookBookServer.AddRecipe("Sertés curry", 9, "Egytálétel", "rec_curry.png", 35);
            await CookBookServer.AddRecipe("Húsleves", 10, "Leves", "rec_csoup.png", 275);
            await CookBookServer.AddRecipe("Paradicsomleves", 11, "Leves", "rec_tsoup.png", 25);
            await CookBookServer.AddRecipe("Párolt rízs", 12, "Egytálétel", "rec_frice.png", 40);
            await CookBookServer.AddRecipe("Grillezet sajtos szendvics", 13, "Szendvics", "rec_gcheese.png", 10);
            await CookBookServer.AddRecipe("Rántott hús", 14, "Húsétel", "rec_fchicken.png", 90);
            await CookBookServer.AddRecipe("Töltött káposzta", 15, "Egytálétel", "rec_cabbage.png", 165);

            await Task.Delay(200);
            await LoadRecipeIngs();

            await LoadSteps();
        }

        public static async Task LoadSteps()
        {
            if (!forceReload) return;

            #region Svéd burgonya
            await CookBookServer.AddRecipeStep(1, "Hámozzon meg fél kiló burgonyát! A meghámozott burgonyákat helyezze egy mély vízzel teli edénybe, hogy megakadályozza a burgonya barnulását.");
            await CookBookServer.AddRecipeStep(1, "A meghámozott burgonyákat vágja ketté, aztán vagdossa be őket(vágjon csikokat bele úgy, hogy ne vágja át az alját), hogy nagyobb ropogós felület legyen");
            await CookBookServer.AddRecipeStep(1, "A bevagdosott burgonyákat helyezze be egy enyhén vajazott tepsibe, majd helyezze be egy 200 °C-ra elő melegített sütőbe!");
            await CookBookServer.AddRecipeStep(1, "A burgonyák legyenek a sütőben, míg meg nem pirulnak!");
            await CookBookServer.AddRecipeStep(1, "Miután kivette a burgonyát a sütőből, fűszerezze meg izlés szerint (ajánlott fűszerek: Só, bors, füstölt paprika) Jó étvágyat!");
            #endregion

            #region Lasagne
            await CookBookServer.AddRecipeStep(2, "Húsos szózs: Daráljon le 20 dkg sertés húst");
            await CookBookServer.AddRecipeStep(2, "Hámozzon meg, és szeleteljen apró darabokra egy hagymát és fokhagymát");
            await CookBookServer.AddRecipeStep(2, "Szeleteljen vékony csíkokra 20 dkg gombát");
            await CookBookServer.AddRecipeStep(2, "Egy kevés olajban kezdje el megpírítani a hagymát és a fokhagymát");
            await CookBookServer.AddRecipeStep(2, "Mikor a hagymák kezdenek üvegesek lenni, adja hozzá a húst és a gombát");
            await CookBookServer.AddRecipeStep(2, "Hagyja főzni néhány percig, majd adjon hozzá 20 dkg hámozott paradicsomot");
            await CookBookServer.AddRecipeStep(2, "Adjon hozzá 10 dl húsleves alapot");
            await CookBookServer.AddRecipeStep(2, "Fűszerezze ízlés szerint (ajánlott fűszerek: oregánó, só, bors)");
            await CookBookServer.AddRecipeStep(2, "Hagyja főni 1 óránt át! Ameddik a szósz fő, elkezdheti a besamelmártást");
            await CookBookServer.AddRecipeStep(2, "Besamel: 4dkg vajon pirítson meg 4dkg lisztet");
            await CookBookServer.AddRecipeStep(2, "Állandóan keverve adja hozzá fokozatosan 2 liter tejet");
            await CookBookServer.AddRecipeStep(2, "Amint a mártás besűrösödik adjon hozzá 1dl tejszínt, és egy kevés reszelt szerecsendiót");
            await CookBookServer.AddRecipeStep(2, "Végső lépések: Kezdje előmelegíteni a sütőt 225°C-ra");
            await CookBookServer.AddRecipeStep(2, "Főzzön meg 200g lasange tésztát sós forró vízben");
            await CookBookServer.AddRecipeStep(2, "Vajazzon meg egy tepsit, majd kenje meg az alját besamellel");
            await CookBookServer.AddRecipeStep(2, "A besamel rétegre tegyen 3 szelet tésztát");
            await CookBookServer.AddRecipeStep(2, "A tésztát kenje meg besamellel, majd a húsos szóssal, és reszeljen egy kevés sajtot");
            await CookBookServer.AddRecipeStep(2, "Ezt a sorrendet ismételje meg, míg az edény tetejére nem ér");
            await CookBookServer.AddRecipeStep(2, "A legfelső réteget kenje meg a maradék besamellel és reszeljen rá sajtot");
            await CookBookServer.AddRecipeStep(2, "Tegye be a tepsit a sütőbe és süsse 30 percig, vagy míg meg nem pirul a teteje és a széle"); 
            await CookBookServer.AddRecipeStep(2, "Vegye ki a sütőből, és vágja fel kockákra. Jó étvágyat!"); 
            #endregion

            #region Carbonara
            await CookBookServer.AddRecipeStep(3, "Tegyen oda főni egy nagy fazék vizet, jól megsózva!");
            await CookBookServer.AddRecipeStep(3, "Miután fellfort a víz tegye oda a tésztát(350g) (spagetti ajánlott) főni!");
            await CookBookServer.AddRecipeStep(3, "Míg fő a tészta, hámozzon meg 1 gerezd fokhagymát és szeletelje fel apró darabokra a szalonnával (10dkg) együtt!");
            await CookBookServer.AddRecipeStep(3, "Ezután kezdje el megpirítani a felszeletelt fokhagymát és szalonnát egy kevés olajjal bekent serpenyőben!");
            await CookBookServer.AddRecipeStep(3, "Ha a szalonna és fokhagyma darabok megpirulnak, és a tészta is elkészül, helyezze át a tésztát a serpenyőbe! (Ajánlott megtartani a vizet amiben a tésztát főzte, mivel állíthatja azzal a szósz állagát.)");
            await CookBookServer.AddRecipeStep(3, "Ezután vegye le a gázról/főzőlapról a serpenyőt és verjen bele 3 tojást!");
            await CookBookServer.AddRecipeStep(3, "Óvatosan keverje a tésztához, mert ha túl nagy erővel csinálja a tojás nem fog szószt alkotni, hanem csak rántottás tésztát fog készíteni");
            await CookBookServer.AddRecipeStep(3, "Ha túl tömény a adjon hozzá egy kevesett a még megtartott tészta vízből, ha pedig túl lágy akkor adjon hozzá egy kevés sajtot!");
            await CookBookServer.AddRecipeStep(3, "Végezetül sóval/borsal meszórhatja izléséhez megfelelően. Jó étvágyat!");
            #endregion

            #region Pizza
            await CookBookServer.AddRecipeStep(4, "Pizza tészta: Keverjen el (10g) élesztőt egy pohár langyos vízben majd, adjon hozzá egy evőkanál lisztet, majd hagyja 10 percig kelni!");
            await CookBookServer.AddRecipeStep(4, "Öntse egy mély edénybe a maradék lisztet, majd adja hozzá az élesztős vizet!");
            await CookBookServer.AddRecipeStep(4, "Gyúrja körülbelül 10 percig míg a tészta sima és rugalmas nem lesz!");
            await CookBookServer.AddRecipeStep(4, "A tésztát helyezze egy enyhén lisztezett tálba pihenni, míg kétszeresére nem nő!");
            await CookBookServer.AddRecipeStep(4, "Kezdje el elő melegíteni a sütőt a maximum hőmérsékletre amire képes!");
            await CookBookServer.AddRecipeStep(4, "Pizza szósz: Hámozzon meg 40 dkg paradicsomot, majd egy edénybe préselje össze!");
            await CookBookServer.AddRecipeStep(4, "A préselt paradicsomokhoz adjon egy csipet sót, és darált oregánót");
            await CookBookServer.AddRecipeStep(4, "Végső lépések: A tésztát nyújtsa ki kerek alakúra, majd terítse be paradicsom szósszal");
            await CookBookServer.AddRecipeStep(4, "Reszeljen rá egy vastag réteg sajtot (kb 35dkg), majd rakjon rá feltéteket az ön ízlése szerint");
            await CookBookServer.AddRecipeStep(4, "Tegye be a pizzát a sütőbe, és hagyja sülni amíg a sajt és a tészta a szélén meg nem pirul");
            await CookBookServer.AddRecipeStep(4, "Vegye ki a pizzát és tálalja esetleg egy pár bazsalkiom levéllel, vagy egy kis reszelt parmezánnal és szeletelje fel! Jó étvágyat!");
            #endregion

            #region Puding
            await CookBookServer.AddRecipeStep(5, "Forraljon fel 60dl tejet egy rúd vaníliával");
            await CookBookServer.AddRecipeStep(5, "Ha a karamell kész, vegye le a gázról, és adjon hozzá 2ek vizet");
            await CookBookServer.AddRecipeStep(5, "Keverjen össze 100g cukrot 3 tojással és 3 tojás sárgájával");
            await CookBookServer.AddRecipeStep(5, "Adja hozzá a keveréket a tejhez");
            await CookBookServer.AddRecipeStep(5, "Helyezze a tej és cukor keveréket kisseb tálkákba");
            await CookBookServer.AddRecipeStep(5, "Egy tepsibe helyezzen annyi vizet, hogy a tálkák feléig érjen");
            await CookBookServer.AddRecipeStep(5, "Melegítse elő a stütőt 180°C-ra");
            await CookBookServer.AddRecipeStep(5, "Tegye be a sütőbe légkeverésre 35-40 percig");
            await CookBookServer.AddRecipeStep(5, "Fogyasztás előtt várjon 30 percig míg kihűl, Jó étvágyat!"); 
            #endregion

            #region Torta
            await CookBookServer.AddRecipeStep(6, "Forraljon fel 2.5 dl vizet");
            await CookBookServer.AddRecipeStep(6, "Miután felforrt, keverjen hozzá a 50g kakaót és 100g cukrot, és hadja kihűlni");
            await CookBookServer.AddRecipeStep(6, "Keverjen össze a 125g vajat 150g cukorral, és adjon hozzá 2 tojást egyesével");
            await CookBookServer.AddRecipeStep(6, "Keverje el a száraz hozzávalókat a tojásos keverékhez");
            await CookBookServer.AddRecipeStep(6, "Adja hozzá a kakaós vizet, majd 22.5 dkg lisztet és jól eldolgozza el");
            await CookBookServer.AddRecipeStep(6, "Vajazzon ki és lisztezzen ki egy torta formát, aztán tegye bele a keveréket");
            await CookBookServer.AddRecipeStep(6, "Süsse a tortát TODO percig");
            await CookBookServer.AddRecipeStep(6, "10 percig hadja hűlni majd távolítsa el a torta formát");
            await CookBookServer.AddRecipeStep(6, "A krémhez felhevítsen fel 4dl és 40dkg csokit");
            await CookBookServer.AddRecipeStep(6, "Keverje össze, majd hadja, hogy kihűljön");
            await CookBookServer.AddRecipeStep(6, "Végezetül töltse meg a tortát, majd tálalja! Jó étvágyat!"); 
            #endregion

            #region Hamburger
            await CookBookServer.AddRecipeStep(7, "Hamburger zsemle: Futasson fel 25g élesztőt");
            await CookBookServer.AddRecipeStep(7, "Adja hozzá a következőkhöz: 1 dl tej, 4dl tejszín, 1ek cukor, fél tk só, 1 tojás, 50g joghurt, 2dl olaj és 30dkg liszt");
            await CookBookServer.AddRecipeStep(7, "Gyúrja őket össze, míg sima és rugalmas nem lesz");
            await CookBookServer.AddRecipeStep(7, "Ossza 4 egyenlő részre, és zsemléket fórmázzon belőle");
            await CookBookServer.AddRecipeStep(7, "Verjen fel még 1 tojást, amivel megkenheti a zsemlék tetejét");
            await CookBookServer.AddRecipeStep(7, "Helyezze őket egy tepsibe, és kenje meg a tetejüket tojással");
            await CookBookServer.AddRecipeStep(7, "Kelessze 30 percig egy 60°Cos légkeveréses sütőben, utána megint kennje meg őket tojással");
            await CookBookServer.AddRecipeStep(7, "200°C-on süsse őket 20percig");
            await CookBookServer.AddRecipeStep(7, "Húspogácsa: Daráljon 1kg marhahúst és 1kg szalonnát");
            await CookBookServer.AddRecipeStep(7, "Hámozzon meg egy hagymát és fokhagymát, majd szeletelje apró darabokra, majd adja hozzá a húsokhoz");
            await CookBookServer.AddRecipeStep(7, "Verje fel az utolsó tojást és keverje be a húshoz");
            await CookBookServer.AddRecipeStep(7, "Adjon hozzá 1ek petrezselymet");
            await CookBookServer.AddRecipeStep(7, "Fűszerezze ízlés szerint (ajánlott fűszerek: fűszer paprika, só, bors)");
            await CookBookServer.AddRecipeStep(7, "Formázzon húspogácsákat, majd olajos serpenyőben süsse meg őket");
            await CookBookServer.AddRecipeStep(7, "Végezetül, a zsemléket vágja ketté és pirítsa meg őket, majd az ízlése szerint építse meg a kívánt hamburgerét! Jó étvágyat!");
            #endregion

            #region Gofri
            await CookBookServer.AddRecipeStep(8, "Keverjen össze 25dkg listet, 1/4ek sót, 6g sütőport, és 6 dl tejet");
            await CookBookServer.AddRecipeStep(8, "Adjon hozzá 3 tojás sárgáját, 1dl étolajat, 40g cukrot és keverje össze");
            await CookBookServer.AddRecipeStep(8, "A maradék 3 tojás fehérjét verje habosra, majd adja hozzá a keverékhez");
            await CookBookServer.AddRecipeStep(8, "A gofri sütőt kennje ki vajjal, helyezzen gofrira elegendő keveréket majd süsse 4 percig");
            await CookBookServer.AddRecipeStep(8, "Végezetül tálalja a gofrikat az ízlésének megfeleően! Jó étvágyat!");
            #endregion

            #region Curry
            await CookBookServer.AddRecipeStep(9, "Kockázzon fel fél kg sertés húst, és egy hagymát");
            await CookBookServer.AddRecipeStep(9, "Egy mozsárban törjön össze 1tk kurkumát, fűszer paprikát és szerecsendiót, 2tk fahéjt, köményt, gyömbért és koriandert");
            await CookBookServer.AddRecipeStep(9, "Az összetört fűszer keveréket keverjen össze fél dl étolajat");
            await CookBookServer.AddRecipeStep(9, "A fűszeres olajba pirítsa meg a hagymát");
            await CookBookServer.AddRecipeStep(9, "Miután a hagyma kezd üveges lenni, adjon hozzá 20dkg paradicsomot összeturmixolva");
            await CookBookServer.AddRecipeStep(9, "Öntse le egy kis vízzel és adja hozzá a húst");
            await CookBookServer.AddRecipeStep(9, "Adjon hozzá 2dl tejet és hadja forrni míg mártás állagú nem lesz");
            await CookBookServer.AddRecipeStep(9, "Végezetül, tálalja a curryt az ön által választott körettel! Jó étvágyat!");
            #endregion

            #region Húsleves
            await CookBookServer.AddRecipeStep(10, "Készítsen elő 1,3 kg csirke húst");
            await CookBookServer.AddRecipeStep(10, "Egy nagy fazékba töltsön 4l vízet és helyezze be ide az előkészített húst");
            await CookBookServer.AddRecipeStep(10, "Közepes lángon forralja fel");
            await CookBookServer.AddRecipeStep(10, "Miután felforrt, adjon hozzá 1 hagymát és 40g borsot, majd hagyja főni másfél órán át");
            await CookBookServer.AddRecipeStep(10, "Másfél óra eltelte után, adjon hozzá 30dkg zellert és 0,3kg burgonyát");
            await CookBookServer.AddRecipeStep(10, "Hagyja főni még egy másfél órára");
            await CookBookServer.AddRecipeStep(10, "3 répát hámozzon meg, vágja hasábokra és adja hozzá a leveshez");
            await CookBookServer.AddRecipeStep(10, "Hagyja főni még egy órát");
            await CookBookServer.AddRecipeStep(10, "Adjon a leveshez 30g petrezselymet");
            await CookBookServer.AddRecipeStep(10, "Főzzön a leveshez 1kg tésztát forró vízben");
            await CookBookServer.AddRecipeStep(10, "Végezetül, tálolja a levest az elkészített tésztával. Jó étvágyat!"); 
            #endregion

            #region Paradicsomleves
            await CookBookServer.AddRecipeStep(11, "Turmixoljon össze 15dkg paradicsomot");
            await CookBookServer.AddRecipeStep(11, "Helyezzen bele egy babérlevelet");
            await CookBookServer.AddRecipeStep(11, "Ízlés szerint adjon hozzá cukrot sót borsot");
            await CookBookServer.AddRecipeStep(11, "Főzze alacsony fokon 10 percig");
            await CookBookServer.AddRecipeStep(11, "Tegyen oda 150g betű, vagy csiga tészát főzni");
            await CookBookServer.AddRecipeStep(11, "Ha a leves egy kicsit még sűrű, adjon hozzá egy kis vizet");
            await CookBookServer.AddRecipeStep(11, "Végezetül tálalja a levest a tésztával! Jó étvágyat!");
            #endregion

            #region Párolt rízs
            await CookBookServer.AddRecipeStep(12, "Rízs: Mosson át alaposan és főzzőn 60 dkg rizset");
            await CookBookServer.AddRecipeStep(12, "A legjobb eredmények érdekében hagyja a riszet legalább 1 napig, mert akkor kevésbé fog ragadni");
            await CookBookServer.AddRecipeStep(12, "Karikázzon fel 2 hagymát és kockázzon fel 4 gerezd fokhagymát és egy paprikát, ezután reszeljen 30g gyömbért és egy répát");
            await CookBookServer.AddRecipeStep(12, "Egy nagy wokba helyezzen 5dl olajat, majd kezddje el pirítani az előkészített zöldségeket fűszereket folyamatosan keverje");
            await CookBookServer.AddRecipeStep(12, "Egy pár perc pirítás után adja hozzá a felkockázott paprikát, borsót és a rizset");
            await CookBookServer.AddRecipeStep(12, "Keverje tovább jó alaposan és pirítja még egy pár percig");
            await CookBookServer.AddRecipeStep(12, "Verjen fel 4 tojást");
            await CookBookServer.AddRecipeStep(12, "Tolja fel a rízset a wok felső részére, majd a felvert tojásokat a wokba, összekeverve a rízssel");
            await CookBookServer.AddRecipeStep(12, "A szósz elkészítéséhez keverjen össze 5dl olajat, 5g borsot, 7g cukrot és egy lájm levét");
            await CookBookServer.AddRecipeStep(12, "Végezetül keverje össze a szószt a rízzsel! Jó étvágyat");
            #endregion

            #region Grilled Cheese
            await CookBookServer.AddRecipeStep(13, "Vajazzon meg 4 szelet kenyeret");
            await CookBookServer.AddRecipeStep(13, "Vágjon 2 végony szelet sajtot");
            await CookBookServer.AddRecipeStep(13, "Olvasszon meg egy kevés vajat egy tapadás mentes serpenyőben");
            await CookBookServer.AddRecipeStep(13, "Tegyen 1 szelet sajtot 2 szelet kenyér közé");
            await CookBookServer.AddRecipeStep(13, "Végezeül pirítsa meg a szendvicseket a serpenyőben mindkét oldalon! Jó étvágyat!");
            #endregion

            #region Rántott hús
            /*
             Hús előkészítése (vékony, só, bors)
            Liszt, tojás, zsemlemorzsa
            közepes forró olajban arany barnáig
            papír törlőre hogy leszívjuk a felesleges olajat

             */
            await CookBookServer.AddRecipeStep(14, "Vágjon fel 70dkg csirkét vékony szeletekre, majd sózza meg borsozza meg őket");
            await CookBookServer.AddRecipeStep(14, "Készítsen elő 3 edényt amiben végrehajtja majd a rántás lépéseit");
            await CookBookServer.AddRecipeStep(14, "Az első edénybe helyezzen lisztet");
            await CookBookServer.AddRecipeStep(14, "A második edénybe verjen fel tojást és őröljön hozzá egy kevés borsot és sozza meg");
            await CookBookServer.AddRecipeStep(14, "Az utolsó edénybe zsemlemorzsát kell felhasználnia egy kis sóval össze keverve");
            await CookBookServer.AddRecipeStep(14, "A vékony hús szeleteket lisztezze be őket, majd egy vékony tojás réteget adjon nekik és végül a zsemlemorzsába forgassa meg úgy, hogy teljesen beborítsa");
            await CookBookServer.AddRecipeStep(14, "Ezt a lépés sorozatot végezze el az összes hús szelettel, ha valamiből többre van szükség, nyugodtan használjon fel többet");
            await CookBookServer.AddRecipeStep(14, "A rántott húsokat, vékony forró olajban kell megsütni míg arany barnák nem lesznek"); 
            await CookBookServer.AddRecipeStep(14, "Végezetül, egy papírtörlő kendővel beborított edénybe helyezze a megsült rántott hús szeleteket, így leitattva a felesleges zsírt. Jó étvágyat!");
            #endregion

            #region Töltött káposzta
            await CookBookServer.AddRecipeStep(15, "Készístsen elő 1 kg káposztát! Ha túl savanyú, alapossan mossa át, ha pedig túl nagy vágja kisebb darabokra");
            await CookBookServer.AddRecipeStep(15, "Egy nagy lábasban kezdjen el pirítani 10dkg szalonnát");
            await CookBookServer.AddRecipeStep(15, "Vágjon apró darabokra 1 hagymát");
            await CookBookServer.AddRecipeStep(15, "Ha a szalonna kezd pirulni, adja hozzá a felszeletelt hagymát");
            await CookBookServer.AddRecipeStep(15, "Amint a hagyma kezd üveges lenni, adjon hozzá 2 gerezd fokhagymát, majd pár másodperc múlva vegye le a lábast a gázról");
            await CookBookServer.AddRecipeStep(15, "Adjon a lábashoz 8g fűszerpaprikát, és a káposztát");
            await CookBookServer.AddRecipeStep(15, "Keverjen össze 4g köményt, 2 babérlevelet, 20g sót, 20g borsot");
            await CookBookServer.AddRecipeStep(15, "Adja a keveréket 3 össze préselt virslihez");
            await CookBookServer.AddRecipeStep(15, "Adjon hozzá 200g húsleves alapot, és öntse le annyi vízzel, míg be nem fedi a lábas tartalmát");
            await CookBookServer.AddRecipeStep(15, "Hagyja alacsony hőn");
            await CookBookServer.AddRecipeStep(15, "Daráljon fél kiló sertés húst");
            await CookBookServer.AddRecipeStep(15, "Mosson 100g rizset");
            await CookBookServer.AddRecipeStep(15, "Adjon a darált húshoz 1gerezd fokhagymát, 1 tojást, petrezselymet, 15 sót és 15 borsot");
            await CookBookServer.AddRecipeStep(15, "Nedves kézzel kezdjen formázni dió méretű gombócokat a töltelékből");
            await CookBookServer.AddRecipeStep(15, "A forró káposztás alaphoz adja a gombócokat");
            await CookBookServer.AddRecipeStep(15, "Alacsony hőn főzze lefedve 40-50 percig");
            await CookBookServer.AddRecipeStep(15, "Tálalásként, adjon hozzá egy kis tejfölt. Jó étvágyat!"); 
            #endregion
        }

        public static async Task LoadRecipeIngs()
        {
            if (!forceReload) return;

            // Svéd burgonya
            await CookBookServer.AddRecipeIng(1, await CookBookServer.GetIngredient("Burgonya"), 0.5f);

            #region Lasagne
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Sertéshús"), 0.2f);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Hagyma"), 1);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Fokhagyma"), 1);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Gomba"), 20);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Paradicsom"), 20);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Húsleves alap"), 10);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Vaj"), 5);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Liszt"), 4);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Tej"), 20);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Tejszín"), 1);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Szerecsendió"), 2.5f);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Só"), 10);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Bors"), 2.5f);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Oregánó"), 3);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Tészta"), 200);
            await CookBookServer.AddRecipeIng(2, await CookBookServer.GetIngredient("Sajt"), 50);
            #endregion

            #region Carbonara
            await CookBookServer.AddRecipeIng(3, await CookBookServer.GetIngredient("Só"), 12);
            await CookBookServer.AddRecipeIng(3, await CookBookServer.GetIngredient("Tészta"), 350);
            await CookBookServer.AddRecipeIng(3, await CookBookServer.GetIngredient("Tojás"), 3);
            await CookBookServer.AddRecipeIng(3, await CookBookServer.GetIngredient("Szalonna"), 10);
            await CookBookServer.AddRecipeIng(3, await CookBookServer.GetIngredient("Tojás"), 3);
            await CookBookServer.AddRecipeIng(3, await CookBookServer.GetIngredient("Sajt"), 10);

            #endregion

            #region Pizza
            await CookBookServer.AddRecipeIng(4, await CookBookServer.GetIngredient("Élesztő"), 20);
            await CookBookServer.AddRecipeIng(4, await CookBookServer.GetIngredient("Liszt"), 25);
            await CookBookServer.AddRecipeIng(4, await CookBookServer.GetIngredient("Étolaj"), 10);
            await CookBookServer.AddRecipeIng(4, await CookBookServer.GetIngredient("Só"), 0.5f);
            await CookBookServer.AddRecipeIng(4, await CookBookServer.GetIngredient("Paradicsom"), 40);
            await CookBookServer.AddRecipeIng(4, await CookBookServer.GetIngredient("Sajt"), 35);
            await CookBookServer.AddRecipeIng(4, await CookBookServer.GetIngredient("Bazsalikom"), 2.5f);
            
            await CookBookServer.AddRecipeIng(4, await CookBookServer.GetIngredient("Oregánó"), 1);
            #endregion

            #region Puding
            await CookBookServer.AddRecipeIng(5, await CookBookServer.GetIngredient("Tej"), 60);
            await CookBookServer.AddRecipeIng(5, await CookBookServer.GetIngredient("Vanília"), 1.5f);
            await CookBookServer.AddRecipeIng(5, await CookBookServer.GetIngredient("Tojás"), 6);
            await CookBookServer.AddRecipeIng(5, await CookBookServer.GetIngredient("Cukor"), 100); 
            #endregion

            #region Torta
            await CookBookServer.AddRecipeIng(6, await CookBookServer.GetIngredient("Kakaópor"), 50);
            await CookBookServer.AddRecipeIng(6, await CookBookServer.GetIngredient("Cukor"), 250);
            await CookBookServer.AddRecipeIng(6, await CookBookServer.GetIngredient("Vaj"), 125);
            await CookBookServer.AddRecipeIng(6, await CookBookServer.GetIngredient("Tojás"), 2);
            await CookBookServer.AddRecipeIng(6, await CookBookServer.GetIngredient("Liszt"), 22.5f);
            await CookBookServer.AddRecipeIng(6, await CookBookServer.GetIngredient("Sütőpor"), 3.8f);
            await CookBookServer.AddRecipeIng(6, await CookBookServer.GetIngredient("Só"), 2.8f);
            await CookBookServer.AddRecipeIng(6, await CookBookServer.GetIngredient("Csokoládé"), 40);
            await CookBookServer.AddRecipeIng(6, await CookBookServer.GetIngredient("Tejszín"), 4);
            #endregion

            #region Hamburger
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Élesztő"), 25);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Tej"), 1);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Tejszín"), 4);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Cukor"), 150);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Só"), 2.8f);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Étolaj"), 2);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Tojás"), 3);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Joghurt"), 50);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Liszt"), 30);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Marhahús"), 1);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Szalonna"), 100);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Hagyma"), 1);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Fokhagyma"), 1);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Petrezselyem"), 15);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Fűszer paprika"), 4);
            await CookBookServer.AddRecipeIng(7, await CookBookServer.GetIngredient("Bors"), 3);
            #endregion

            #region Gofri
            await CookBookServer.AddRecipeIng(8, await CookBookServer.GetIngredient("Liszt"), 25);
            await CookBookServer.AddRecipeIng(8, await CookBookServer.GetIngredient("Só"), 0.75f);
            await CookBookServer.AddRecipeIng(8, await CookBookServer.GetIngredient("Sütőpor"), 6);
            await CookBookServer.AddRecipeIng(8, await CookBookServer.GetIngredient("Tej"), 4);
            await CookBookServer.AddRecipeIng(8, await CookBookServer.GetIngredient("Tojás"), 3);
            await CookBookServer.AddRecipeIng(8, await CookBookServer.GetIngredient("Étolaj"), 1f);
            await CookBookServer.AddRecipeIng(8, await CookBookServer.GetIngredient("Cukor"), 40);
            #endregion

            #region Curry
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Sertéshús"), 0.5f);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Paradicsom"), 20);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Hagyma"), 1);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Fokhagyma"), 1);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Szerecsendió"), 5);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Fűszer paprika"), 6);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Fahéj"), 8);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Só"), 16);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Kömény"), 8);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Csili paprika"), 1);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Kurkuma"), 4);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Gyömbér"), 8);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Koriander"), 8);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Babérlevél"), 1);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Étolaj"), 0.5f);
            await CookBookServer.AddRecipeIng(9, await CookBookServer.GetIngredient("Tej"), 2);
            #endregion

            #region Húsleves
            await CookBookServer.AddRecipeIng(10, await CookBookServer.GetIngredient("Csirkehús"), 1.3f);
            await CookBookServer.AddRecipeIng(10, await CookBookServer.GetIngredient("Hagyma"), 1);
            await CookBookServer.AddRecipeIng(10, await CookBookServer.GetIngredient("Só"), 50);
            await CookBookServer.AddRecipeIng(10, await CookBookServer.GetIngredient("Bors"), 40);
            await CookBookServer.AddRecipeIng(10, await CookBookServer.GetIngredient("Zeller"), 30);
            await CookBookServer.AddRecipeIng(10, await CookBookServer.GetIngredient("Répa"), 3);
            await CookBookServer.AddRecipeIng(10, await CookBookServer.GetIngredient("Petrezselyem"), 30);
            await CookBookServer.AddRecipeIng(10, await CookBookServer.GetIngredient("Tészta"), 1000);
            await CookBookServer.AddRecipeIng(10, await CookBookServer.GetIngredient("Burgonya"), 0.3f);
            #endregion

            #region Paradicsomleves
            await CookBookServer.AddRecipeIng(11, await CookBookServer.GetIngredient("Paradicsom"), 15);
            await CookBookServer.AddRecipeIng(11, await CookBookServer.GetIngredient("Babérlevél"), 1);
            await CookBookServer.AddRecipeIng(11, await CookBookServer.GetIngredient("Só"), 8);
            await CookBookServer.AddRecipeIng(11, await CookBookServer.GetIngredient("Bors"), 8);
            await CookBookServer.AddRecipeIng(11, await CookBookServer.GetIngredient("Cukor"), 15);
            await CookBookServer.AddRecipeIng(11, await CookBookServer.GetIngredient("Tészta"), 150);
            #endregion

            #region Párolt rízs
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Rízs"), 60);
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Só"), 8);
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Étolaj"), 10);
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Hagyma"), 2);
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Fokhagyma"), 4);
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Gyömbér"), 30);
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Répa"), 1);
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Borsó"), 7);
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Paprika"), 1);
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Bors"), 5);
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Cukor"), 7);
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Lájm"), 1);
            await CookBookServer.AddRecipeIng(12, await CookBookServer.GetIngredient("Ecet"), 5);
            #endregion

            #region Grilled Cheese
            await CookBookServer.AddRecipeIng(13, await CookBookServer.GetIngredient("Kenyér"), 0.25f);
            await CookBookServer.AddRecipeIng(13, await CookBookServer.GetIngredient("Vaj"), 7);
            await CookBookServer.AddRecipeIng(13, await CookBookServer.GetIngredient("Sajt"), 15);
            #endregion

            #region Rántott hús
            await CookBookServer.AddRecipeIng(14, await CookBookServer.GetIngredient("Csirkehús"), 0.7f);
            await CookBookServer.AddRecipeIng(14, await CookBookServer.GetIngredient("Zsemlemorzsa"), 25);
            await CookBookServer.AddRecipeIng(14, await CookBookServer.GetIngredient("Liszt"), 25);
            await CookBookServer.AddRecipeIng(14, await CookBookServer.GetIngredient("Tojás"), 3);
            await CookBookServer.AddRecipeIng(14, await CookBookServer.GetIngredient("Só"), 56);
            await CookBookServer.AddRecipeIng(14, await CookBookServer.GetIngredient("Bors"), 56);
            await CookBookServer.AddRecipeIng(14, await CookBookServer.GetIngredient("Étolaj"), 5);
            #endregion

            #region Töltött káposzta
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Káposzta"), 100);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Húsleves alap"), 20);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Fűszer paprika"), 8);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Babérlevél"), 2);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Kömény"), 4);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Hagyma"), 1);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Szalonna"), 10);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Virsli"), 3);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Fokhagyma"), 3);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Só"), 35);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Bors"), 35);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Sertéshús"), 0.5f);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Rízs"), 100);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Tojás"), 1);
            await CookBookServer.AddRecipeIng(15, await CookBookServer.GetIngredient("Petrezselyem"), 4); 
            #endregion
        }
    }
}
