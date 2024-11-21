using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using Dapper;


namespace WpfZooManager
{
    public partial class MainWindow : Window
    {
        SQLiteConnection sqliteConnection;
        IZooManagerRepository zooManagerRepository;

        public MainWindow(SQLiteConnection sqliteConnection, IZooManagerRepository zooManagerRepository)
        {
            InitializeComponent();

            this.sqliteConnection = sqliteConnection;
            this.zooManagerRepository = zooManagerRepository;

            ShowZoos();
            ShowAnimals();
        }

        private void ShowZoos()
        {
            try
            {
                var zoos = zooManagerRepository.AllZoos();

                listZoos.DisplayMemberPath = "Location";
                listZoos.SelectedValuePath = "Id";
                listZoos.ItemsSource = zoos;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        private void ShowAnimals()
        {
            try
            {
                var animals = zooManagerRepository.AllAnimals();

                listAnimals.DisplayMemberPath = "Name";
                listAnimals.SelectedValuePath = "Id";
                listAnimals.ItemsSource = animals;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        private void ShowAssociatedAnimals()
        {
            try
            {

                if (listZoos.SelectedValue == null)
                {
                    listAssociatedAnimals.ItemsSource = null;
                    return;
                }

                string query = "SELECT za.id, a.name FROM animal a, zoo_animal za WHERE a.id = za.animal_id AND za.zoo_id = ?";

                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                SQLiteDataAdapter sqliteDataAdapter = new SQLiteDataAdapter(sqliteCommand);


                using (sqliteDataAdapter)
                {
                    sqliteCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable animalTable = new DataTable();
                    sqliteDataAdapter.Fill(animalTable);

                    listAssociatedAnimals.DisplayMemberPath = "name";
                    listAssociatedAnimals.SelectedValuePath = "id";
                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }

        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listZoos.SelectedItem != null)
            {
                Zoo selectedZoo = (Zoo)listZoos.SelectedItem;
                myTextBox.Text = selectedZoo.Location;
            }
            else
            {
                myTextBox.Text = string.Empty;
            }
            ShowAssociatedAnimals();

        }

        private void listAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listAnimals.SelectedItem != null)
            {
                Animal selectedAnimal = (Animal)listAnimals.SelectedItem;
                myTextBox.Text = selectedAnimal.Name;
            }
            else
            {
                myTextBox.Text = string.Empty;
            }

        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM zoo WHERE id = ?";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqliteCommand.ExecuteNonQuery();

                string query2 = "DELETE FROM zoo_animal WHERE zoo_id = ?";
                SQLiteCommand sqliteCommand2 = new SQLiteCommand(query2, sqliteConnection);
                sqliteCommand2.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqliteCommand2.ExecuteNonQuery();
                ShowZoos();
                ShowAssociatedAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM animal WHERE id = ?";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
                sqliteCommand.ExecuteNonQuery();               

                string query2 = "DELETE FROM zoo_animal WHERE animal_id = ?";
                SQLiteCommand sqliteCommand2 = new SQLiteCommand(query2, sqliteConnection);
                sqliteCommand2.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
                sqliteCommand2.ExecuteNonQuery();               
                ShowAnimals();
                ShowAssociatedAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var zoo = new Zoo { Location = myTextBox.Text };
                zooManagerRepository.AddZoo(zoo);
                ShowZoos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var animal = new Animal { Name = myTextBox.Text };
                zooManagerRepository.AddAnimal(animal);
                ShowAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void RemoveAnimalFromZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM zoo_animal WHERE id = ?";

                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@Id", listAssociatedAnimals.SelectedValue);

                sqliteCommand.ExecuteNonQuery();               
                ShowAssociatedAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO zoo_animal (zoo_id, animal_id) VALUES (?, ?)";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@zoo_id", listZoos.SelectedValue);
                sqliteCommand.Parameters.AddWithValue("@animal_id", listAnimals.SelectedValue);
                sqliteCommand.ExecuteNonQuery();
                ShowAssociatedAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }




        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var zoo = new Zoo { Id = (int)listZoos.SelectedValue, Location = myTextBox.Text };
                zooManagerRepository.UpdateZoo(zoo);
                ShowZoos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var animal = new Animal { Id = (int)listAnimals.SelectedValue, Name = myTextBox.Text };
                zooManagerRepository.UpdateAnimal(animal);
                ShowAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New Menu Item Clicked");
        }
    }

}