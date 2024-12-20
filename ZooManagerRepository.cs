using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace WpfZooManager
{

    // https://www.dotnetmastery.com/Home/Details?courseId=13
    // https://github.com/bhrugen/DapperDemo/blob/master/DapperDemo/Repository/CompanyRepository.cs
    // https://github.com/bhrugen/DapperDemo/blob/master/DapperDemo/Repository/BonusRepository.cs
    // https://github.com/bhrugen/DapperDemo/blob/master/DapperDemo/Repository/CompanyRepositoryContib.cs
    // https://github.com/DapperLib/Dapper.Contrib
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

        public void RemoveAnimalFromZoo(Zoo zoo, Animal animal);
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
            return _db.GetAll<Zoo>().ToList();
            /*
            var sql = "Select zoo.*, animal.id as animal_id, animal.name as name FROM zoo, animal, zoo_animal " +
                "WHERE zoo.id = zoo_animal.zoo_id AND animal.id = zoo_animal.animal_id";
            var zooDic = new Dictionary<int, Zoo>();

            var zoo = _db.Query<Zoo, Animal, Zoo>(sql, (zoo, animal) =>
            {
                if (!zooDic.TryGetValue(zoo.Id, out var currentZoo))
                {
                    currentZoo = zoo;
                    zooDic.Add(currentZoo.Id, currentZoo);
                }
                currentZoo.Animals.Add(animal);
                return currentZoo;
            }, splitOn: "animal_id");

            return zoo.Distinct().ToList();
            */
        }

        public List<Animal> AllAnimals()
        {
            return _db.GetAll<Animal>().ToList();
        }

        public Zoo AddZoo(Zoo zoo)
        {
            var sql = "INSERT INTO zoo (Name) VALUES (@Name);";
            var id = _db.Execute(sql, zoo);
            zoo.Id = id;
            return zoo;
        }

        public Animal AddAnimal(Animal animal)
        {
            var id = _db.Insert(animal);
            animal.Id = (int)id;
            return animal;
        }

        public Zoo UpdateZoo(Zoo zoo)
        {
            var sql = "UPDATE zoo SET Name = @Name WHERE Id = @Id;";
            _db.Execute(sql, zoo);
            return zoo;
        }

        public Animal UpdateAnimal(Animal animal)
        {
            _db.Update(animal);
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
            // Dapper.Contrib
            _db.Delete(animal);

            // Dapper
            var sql = "DELETE FROM zoo_animal WHERE animal_id = @Id;";
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

        public void RemoveAnimalFromZoo(Zoo zoo, Animal animal)
        {
            var sql = "DELETE FROM zoo_animal WHERE zoo_id = @ZooId AND animal_id = @AnimalId;";
            _db.Execute(sql, new { ZooId = zoo.Id, AnimalId = animal.Id });
        }
    }

}