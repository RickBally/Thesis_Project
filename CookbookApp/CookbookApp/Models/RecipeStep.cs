using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookbookApp.Models
{
    public class RecipeStep : RealmObject
    {
        public int RecID { get; set; }
        public int Step { get; set; }
        public string Instruction { get; set; }

    }
}
