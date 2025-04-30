using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookbookApp.Models
{
    public class IngCategory : RealmObject
    {
        [PrimaryKey, MapTo("name")]
        public string Name { get; set; }
        [MapTo("allergen")]
        public string Allergen { get; set; }
        [MapTo("bgcolor")]
        public string BGColor { get; set; }

        public IngCategory()
        {
            
        }
    }
}
