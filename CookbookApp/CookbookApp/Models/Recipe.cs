using System;
using System.Collections.Generic;
using System.Text;
using Realms;

namespace CookbookApp.Models
{
    public class Recipe : RealmObject
    {
        [PrimaryKey, MapTo("id")]
        public int ID { get; set; }
        [MapTo("name")]
        public string Name { get; set; }
        [MapTo("category")]
        public string Category { get; set; }
        [MapTo("image_source")]
        public string ImageSource { get; set; }
        [MapTo("cook_time")]
        public string CookTime { get; set; }
        [MapTo("favorited")]
        public bool Favorited { get; set; }
        [MapTo("fav_icon")]
        public string FavoriteIcon { get; set; }
    }
}
