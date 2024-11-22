using System.Data;
using Dapper;

namespace WpfZooManager
{

    // https://www.dotnetmastery.com/Home/Details?courseId=13
    // https://github.com/bhrugen/DapperDemo/blob/master/DapperDemo/Repository/CompanyRepository.cs
    // https://github.com/bhrugen/DapperDemo/blob/master/DapperDemo/Repository/CompanyRepositoryContib.cs
    // https://github.com/bhrugen/DapperDemo/blob/master/DapperDemo/Repository/BonusRepository.cs
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

        public List<Animal> GetAssociatedAnimals(Zoo zoo);

        public void AddAnimalToZoo(Zoo zoo, Animal animal);
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

        public List<Animal> GetAssociatedAnimals(Zoo zoo)
        {
            var sql = "SELECT * FROM animal WHERE Id IN (SELECT animal_id FROM zoo_animal WHERE zoo_id = @Id);";
            var animals = _db.Query<Animal>(sql, zoo).ToList();
            return animals;
        }

        public void AddAnimalToZoo(Zoo zoo, Animal animal)
        {
            var sql = "INSERT INTO zoo_animal (zoo_id, animal_id) VALUES (@ZooId, @AnimalId);";
            _db.Execute(sql, new { ZooId = zoo.Id, AnimalId = animal.Id });
        }
    }

}