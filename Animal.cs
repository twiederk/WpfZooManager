using Dapper.Contrib.Extensions;

namespace WpfZooManager
{
    [Table("Animal")]
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
