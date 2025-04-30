using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookbookApp.Models
{
    public class RecipeIngredient: RealmObject
    {
        [PrimaryKey, MapTo("id")]
        public int ID { get; set; }
        [MapTo("rec_id")]
        public int RecID { get; set; }
        [MapTo("ingredient")]
        public Ingredient Ingredient { get; set; }
        [MapTo("ammount")]
        public float Ammount { get; set; }
    }
}
