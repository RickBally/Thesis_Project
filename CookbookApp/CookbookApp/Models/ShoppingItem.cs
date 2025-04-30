using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookbookApp.Models
{
    public class ShoppingItem : RealmObject
    {
        [PrimaryKey, MapTo("ing_name")]
        public string IngName { get; set; }

        [MapTo("ammount")]
        public float Ammount { get; set; }

        [MapTo("shopping_ing")]
        public Ingredient ShoppingIng { get; set; }

        [MapTo("selected")]
        public bool Selected { get; set; }

        [MapTo("select_icon")]
        public string SelectIcon { get; set; }
    }
}
