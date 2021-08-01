using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MonefyWPF.Model
{
    public class Category
    {
        public Category(string name, string color)
        {
            Name = name;
            Color = color;
           
        }
        public Category()
        {

        }
        public string Name { get; set; }
        public string Color { get; set; }
        public double Sum { get; set; } = 0;


    }
}
