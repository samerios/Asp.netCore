using System;
using System.Collections.Generic;

#nullable disable

namespace project.Models
{
    public partial class Vegetable
    {
        private string name, color,size;
        public string Name { get { return name; } set { if (value != null && value.Length <= 50) name = value; } }
        public string Color { get { return color; } set { if (value != null && value.Length <= 7) color = value; } }
        public string Size { get { return size; } set { if ((value == "Small" || value == "Medium" || value == "Large") && value.Length <= 10) size = value; else size = null; } }
    }
}
