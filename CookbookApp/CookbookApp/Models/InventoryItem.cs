using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookbookApp.Models
{
    public class InventoryItem : RealmObject
    {
        [PrimaryKey, MapTo("ingname")]
        public string IngName{ get; set; }
        [MapTo("ingredient")]
        public Ingredient Ingredient { get; set; }
        [MapTo("ammount")]
        public float Ammount { get; set; }
        [MapTo("ammountDisplay")]
        public string AmmountDisplay { get; set; }
    }
}
