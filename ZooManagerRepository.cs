using System.Data;
using Dapper;

namespace WpfZooManager
{

    public interface IZooManagerRepository
    {
        public List<Zoo> AllZoos();

        public List<Animal> AllAnimals();
    }

    public class ZooManagerRepository() : IZooManagerRepository
    {

        private IDbConnection _db;

        public ZooManagerRepository(IDbConnection db) : this()
        {
            _db = db;
        }


        public List<Zoo> AllZoos()
        {
            var sql = "SELECT * FROM zoo";
            var zoos = _db.Query<Zoo>(sql).ToList();
            return zoos;
        }

        public List<Animal> AllAnimals()
        {
            var sql = "SELECT * FROM animal";
            var animals = _db.Query<Animal>(sql).ToList();
            return animals;
        }
    }

}