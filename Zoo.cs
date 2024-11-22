using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfZooManager
{

    public class Zoo
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public List<Animal> Animals { get; set; }

        public Zoo()
        {
            Animals = new List<Animal>();
        }
    }
}
