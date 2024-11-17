using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;


namespace WpfZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteConnection sqliteConnection;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = "Data Source=C:\\Daten\\sourcecode\\csharp\\WpfZooManager\\ZooManager.db";
            sqliteConnection = new SQLiteConnection(connectionString);
            sqliteConnection.Open();
            ShowZoos();
            ShowAnimals();

        }

        private void ShowZoos()
        {
            try
            {
                string query = "SELECT * FROM zoo";
                SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(query, sqliteConnection);

                using (sqLiteDataAdapter)
                {
                    DataTable zooTable = new DataTable();
                    sqLiteDataAdapter.Fill(zooTable);

                    listZoos.DisplayMemberPath = "location";
                    listZoos.SelectedValuePath = "id";
                    listZoos.ItemsSource = zooTable.DefaultView;
                }
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
                string query = "SELECT * FROM animal";
                SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(query, sqliteConnection);

                using (sqLiteDataAdapter)
                {
                    DataTable animalTable = new DataTable();
                    sqLiteDataAdapter.Fill(animalTable);

                    listAnimals.DisplayMemberPath = "name";
                    listAnimals.SelectedValuePath = "id";
                    listAnimals.ItemsSource = animalTable.DefaultView;
                }
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
            ShowAssociatedAnimals();
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM zoo WHERE id = ?";

                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                sqliteCommand.ExecuteNonQuery();
                ShowZoos();
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
                ShowAnimals();
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
                MessageBox.Show("AddZoo_Click", "Error");
                string query = "INSERT INTO zoo (location) VALUES (?)";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@location", myTextBox.Text);
                sqliteCommand.ExecuteNonQuery();
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
                string query = "INSERT INTO animal (name) VALUES (?)";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqliteCommand.ExecuteNonQuery();
                ShowAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }

}