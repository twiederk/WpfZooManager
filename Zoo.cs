using Dapper.Contrib.Extensions;

namespace WpfZooManager
{

    [Table("Zoo")]
    public class Zoo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Write(false)]
        public List<Animal> Animals { get; set; }

        public Zoo()
        {
            Animals = new List<Animal>();
        }
    }
}
