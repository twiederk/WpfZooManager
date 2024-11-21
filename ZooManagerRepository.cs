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

        public Zoo UpdateZoo(Zoo zoo);

        public Animal UpdateAnimal(Animal animal);

        public void DeleteZoo(Zoo zoo);

        public void DeleteAnimal(Animal animal);
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

        public Zoo UpdateZoo(Zoo zoo)
        {
            var sql = "UPDATE zoo SET Location = @Location WHERE Id = @Id;";
            _db.Execute(sql, zoo);
            return zoo;
        }

        public Animal UpdateAnimal(Animal animal)
        {
            var sql = "UPDATE animal SET Name = @Name WHERE Id = @Id;";
            _db.Execute(sql, animal);
            return animal;
        }
            
        public void DeleteZoo(Zoo zoo)
        {
            var sql = "DELETE FROM zoo WHERE Id = @Id;";
            _db.Execute(sql, zoo);
            sql = "DELETE FROM zoo_animal WHERE zoo_id = @Id;";
            _db.Execute(sql, zoo);
        }
        
        public void DeleteAnimal(Animal animal)
        {
            var sql = "DELETE FROM animal WHERE Id = @Id;";
            _db.Execute(sql, animal);
            sql = "DELETE FROM zoo_animal WHERE animal_id = @Id;";
            _db.Execute(sql, animal);
        }
    }

}