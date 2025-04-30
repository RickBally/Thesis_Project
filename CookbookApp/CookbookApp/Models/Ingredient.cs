using Realms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace CookbookApp.Models
{
    public class Ingredient : RealmObject
    {
        [PrimaryKey, MapTo("name")]
        public string Name { get; set; }
        [MapTo("category")]
        public IngCategory Category { get; set; }
        [MapTo("imgsrc")]
        public string ImageSource { get; set; }
        [MapTo("unit")]
        public string Unit { get; set; }

        public Ingredient()
        {

        }
    }
}
