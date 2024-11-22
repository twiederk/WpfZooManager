using Dapper.Contrib.Extensions;

namespace WpfZooManager
{

    public class Zoo
    {
        public int Id { get; set; }
        public string Location { get; set; }
        [Write(false)]
        public List<Animal> Animals { get; set; }

        public Zoo()
        {
            Animals = new List<Animal>();
        }
    }
}
