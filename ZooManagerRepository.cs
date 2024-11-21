using System.Data;
using Dapper;

namespace WpfZooManager
{

    public interface IZooManagerRepository
    {
        public List<Zoo> AllZoos();

        public List<Animal> AllAnimals();

        public Zoo AddZoo(Zoo zoo);

        public Animal AddAnimal(Animal animal);
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

        public Zoo AddZoo(Zoo zoo)
        {
            var sql = "INSERT INTO zoo (Location) VALUES (@Location);";
            var id = _db.Execute(sql, zoo);
            zoo.Id = id;
            return zoo;
        }

        public Animal AddAnimal(Animal animal)
        {
            var sql = "INSERT INTO animal (Name) VALUES (@Name);";
            var id = _db.Execute(sql, animal);
            animal.Id = id;
            return animal;
        }
            
    }

}